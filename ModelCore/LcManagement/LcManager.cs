using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelCore.DataModel;
using CommonLib.DataAccess;
using ModelCore.Locale;
using ModelCore.Properties;

namespace ModelCore.LcManagement
{
    public class LcManager : LcEntityManager<LetterOfCredit>
	{
		public LcManager() : base()
		{
			//
			// TODO: 在此加入建構函式的程式碼
			//
		}

        public LcManager(GenericManager<LcEntityDataContext> mgr) : base(mgr) { }

        public IQueryable<LetterOfCredit> CheckReadyToCancelLc()
        {
            DateTime twoWeeksBefore = DateTime.Today.AddDays(-14);
            var items = this.GetTable<LetterOfCreditVersion>()
                .GroupBy(l => l.LcID)
                .Select(g => g.OrderByDescending(l => l.VersionID).FirstOrDefault())
                .Where(l => /*l.LcItem.有效期限 >= twoWeeksBefore &&*/ l.LcItem.有效期限 < twoWeeksBefore.AddDays(1))
                .Select(l => l.LetterOfCredit)
                .Where(l => l.可用餘額 > 0)
                .Where(l => !l.CreditCancellation.Any());

            return items;
        }

        public CreditCancellation CreateActiveCancellation(LetterOfCredit lcItem)
        {
            DateTime now = DateTime.Now;
            CreditCancellation item = new CreditCancellation
            {
                註銷申請號碼 = lcItem.LcNo + "(主動餘額註銷)",
                Documentary = new Documentary
                {
                    DocType = (int)Naming.DocumentTypeDefinition.信用狀註銷申請書,
                    DocDate = now,
                    CurrentLevel = (int)Naming.DocumentLevel.主動餘額註銷_待登錄
                },
                LcID = lcItem.LcID,
                申請日期 = now
            };

            item.Documentary.DoApprove(Naming.DocumentLevel.主動餘額註銷_待登錄, Settings.Default.SystemID, null);
            this.GetTable<CreditCancellation>().InsertOnSubmit(item);
            this.SubmitChanges();
            return item;
        }

    }
}
