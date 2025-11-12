using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CommonLib.Core.DataWork;
using EAI.Service.Transaction;
using ModelCore.BankManagement;
using ModelCore.DataModel;
using ModelCore.Locale;
using ModelCore.Properties;
using ModelCore.UserManagement;
using CommonLib.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ModelCore.Helper
{
    public static partial class ExtensionMethods
    {
        public static String JoinEmail(this String email, params string[] addr)
        {
            List<String> items = new List<string>();
            if (!String.IsNullOrEmpty(email))
            {
                items.AddRange(email.Split(',', ';'));
            }

            if (addr != null && addr.Length > 0)
            {
                foreach (var s in addr)
                {
                    if (!String.IsNullOrEmpty(s))
                        items.AddRange(s.Split(',', ';'));
                }
            }

            var all = items.Where(s => s != null).Select(s => s.Trim()).Where(s => s.Length > 0).ToArray();
            return all.Length > 0 ? String.Join(",", all) : null;
        }

        #region Check number
        public static string CheckNumber(this String numValue)
        {
            string strCheckNo = numValue?.Replace("-", "");

            if (strCheckNo != null && strCheckNo.Length == 11)
            {
                string strCheckKey = "21907654321";
                int[] intArrayKey = new int[11];
                int[] intArrayNo = new int[11];
                int[] intArrayAns = new int[11];
                int intAns = 0;

                for (int i = 0; i < 11; i++)
                {
                    intArrayNo[i] = strCheckNo[i] - '0';    //int.Parse(strCheckNo.Substring(i, 1));
                    intArrayKey[i] = strCheckKey[i] - '0';  //int.Parse(strCheckKey.Substring(i, 1));
                    intArrayAns[i] = (intArrayNo[i] * intArrayKey[i]) % 10;
                    intAns += intArrayAns[i];
                }
                intAns %= 10;
                if (intAns != 0)
                    intAns = 10 - intAns;

                return String.Join("-",
                    strCheckNo.Substring(0, 4),
                    strCheckNo.Substring(4, 2),
                    strCheckNo.Substring(6, 5),
                    intAns.ToString());
                
            }
            return null;
        }
        #endregion		

        public static string ReadErrCodeDis(this GenericManager<LcEntityDbContext> mgr, IEAITransaction transaction)
        {
            return transaction.GetResponseDescription();
            //String result = desc != null ? desc.Trim() : null;
            //if (String.IsNullOrEmpty(result))
            //{
            //    var item = mgr.GetTable<viewEaiErrCode>().Where(e => e.ErrCode == strErrCode).FirstOrDefault();

            //    if (item != null)
            //    {
            //        return item.ErrMsg;
            //    }
            //    else
            //        return strErrCode;
            //}
            //else
            //    return result;
        }

        public static IQueryable<BankData> GetActionBranch(this int userID, GenericManager<LcEntityDbContext> models)
        {
            return userID.GetActionBranch(models.DataContext);
        }

        public static IQueryable<BankData> GetActionBranch(this UserProfile userProfile, GenericManager<LcEntityDbContext> models)
        {
            return userProfile.ProfileData.USER_ID.GetActionBranch(models.DataContext);
        }

        public static IQueryable<BankData> GetActionBranch(this UserProfile userProfile, DbContext dbContext)
        {
            return userProfile.ProfileData.USER_ID.GetActionBranch(dbContext);
        }

        public static IQueryable<BankData> GetActionBranch(this int userID, DbContext dbContext)
        {
            return dbContext.Set<BankUserBranch>().Where(b => b.USER_ID == userID)
                .Select(b => b.BRANCH);
        }

        public static IQueryable<NegoDraft> GetEffectiveNegoDraft(this UserProfile userProfile, GenericManager<LcEntityDbContext> mgr)
        {
            return mgr.GetTable<NegoDraft>()
                .Join(mgr.GetTable<LetterOfCreditVersion>()
                    .Join(mgr.GetTable<LetterOfCredit>()
                        .Join(mgr.GetTable<CreditApplicationDocumentary>()
                            .Join(userProfile.GetActionBranch(mgr), a => a.AdvisingBankCode, b => b.BankCode, (a, b) => a)
                        , l => l.ApplicationID, c => c.DocumentaryID, (l, c) => l)
                    , v => v.LcID, l => l.LcID, (v, l) => v)
                , n => n.NegoLcVersionID, l => l.VersionID, (n, l) => n);
        }

        //public static IQueryable<NegoLcVersion> GetValidLc(this IQueryable<NegoLcVersion> items)
        //{
        //    return items.Where(l => l.可用餘額 > 0 && l.LcItems.有效期限 >= DateTime.Today);
        //}



        public static IQueryable<NegoDraft> GetNegoDraftTodoList(this UserProfile userProfile, GenericManager<LcEntityDbContext> mgr)
        {
            return mgr.GetTable<Documentary>()
                .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待經辦審核
                    || d.CurrentLevel == (int)Naming.DocumentLevel.已退回_CRC退回
                    || d.CurrentLevel == (int)Naming.DocumentLevel.已退回_主管退回)
                .Join(mgr.GetTable<NegoDraft>()
                        .Join(mgr.GetTable<NegoDraftExtension>()
                            .Join(userProfile.GetActionBranch(mgr), a => a.NegoBranch, b => b.BankCode, (a, b) => a)
                        , n => n.DocumentaryID, l => l.NegoDraftID, (n, l) => n)
                , d => d.DocID, n => n.DocumentaryID, (d, n) => n);
        }

        public static IQueryable<NegoDraft> GetNegoDraftToVerifyList(this UserProfile userProfile, GenericManager<LcEntityDbContext> mgr)
        {
            return mgr.GetTable<Documentary>()
                .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待主管審核)
                .Join(mgr.GetTable<NegoDraft>()
                        .Join(mgr.GetTable<NegoDraftExtension>()
                            .Join(userProfile.GetActionBranch(mgr), a => a.NegoBranch, b => b.BankCode, (a, b) => a)
                        , n => n.DocumentaryID, l => l.NegoDraftID, (n, l) => n)
                , d => d.DocID, n => n.DocumentaryID, (d, n) => n);
        }

        public static IQueryable<NegoDraft> GetNegoDraftToRegisterList(this UserProfile userProfile, GenericManager<LcEntityDbContext> mgr)
        {
            return mgr.GetTable<Documentary>()
                .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待CRC登錄
                    || d.CurrentLevel == (int)Naming.DocumentLevel.已退回_CRC主管退回)
                .Join(mgr.GetTable<NegoDraft>()
                        .Join(mgr.GetTable<NegoDraftExtension>()
                            .Join(userProfile.GetActionBranch(mgr), a => a.LcBranch, b => b.BankCode, (a, b) => a)
                        , n => n.DocumentaryID, l => l.NegoDraftID, (n, l) => n)
                , d => d.DocID, n => n.DocumentaryID, (d, n) => n);
        }

        public static IQueryable<NegoDraft> GetNegoDraftToAllowList(this UserProfile userProfile, GenericManager<LcEntityDbContext> mgr)
        {
            return mgr.GetTable<Documentary>()
                .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待放行)
                .Join(mgr.GetTable<NegoDraft>()
                        .Join(mgr.GetTable<NegoDraftExtension>()
                            .Join(userProfile.GetActionBranch(mgr), a => a.LcBranch, b => b.BankCode, (a, b) => a)
                        , n => n.DocumentaryID, l => l.NegoDraftID, (n, l) => n)
                , d => d.DocID, n => n.DocumentaryID, (d, n) => n);
        }

        public static IQueryable<NegoDraft> GetNegoDraftToMarkList(this UserProfile userProfile, GenericManager<LcEntityDbContext> mgr)
        {
            return mgr.GetTable<Documentary>()
                .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待註記)
                .Join(mgr.GetTable<NegoDraft>()
                        .Join(mgr.GetTable<NegoDraftExtension>()
                            .Join(userProfile.GetActionBranch(mgr), a => a.LcBranch, b => b.BankCode, (a, b) => a)
                        , n => n.DocumentaryID, l => l.NegoDraftID, (n, l) => n)
                , d => d.DocID, n => n.DocumentaryID, (d, n) => n);
        }

        public static IQueryable<NegoDraft> FilterNegoDraftByServiceType(this IQueryable<NegoDraft> items, GenericManager<LcEntityDbContext> models, Naming.DraftType? draftType)
        {
            if (draftType.HasValue)
            {
                items = items.Join(models.GetTable<NegoDraftExtension>().Where(t => t.DraftType == (int?)draftType),
                                n => n.DocumentaryID, t => t.NegoDraftID, (n, t) => n);
            }
            return items;
        }

        public static IQueryable<NegoDraft> FilterNegoDraftByRemitting(this IQueryable<NegoDraft> items, GenericManager<LcEntityDbContext> models, Naming.DraftType draftType)
        {
            items = items.Join(models.GetTable<NegoDraftExtension>().Where(t => t.DraftType == (int)draftType),
                            n => n.DocumentaryID, t => t.NegoDraftID, (n, t) => n)
                        .Join(models.GetTable<FpgNegoDraft>().Where(f => !f.匯入銀行代碼.StartsWith("009")),
                            n => n.DocumentaryID, f => f.NegoDraftID, (n, f) => n);

            return items;
        }

        public static IQueryable<NegoDraftAcceptance> GetDraftAcceptanceTodoList(this UserProfile userProfile, GenericManager<LcEntityDbContext> mgr)
        {
            return mgr.GetTable<Documentary>()
                .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待經辦審核
                    || d.CurrentLevel == (int)Naming.DocumentLevel.已退回_CRC退回
                    || d.CurrentLevel == (int)Naming.DocumentLevel.已退回_主管退回)
                .Join(mgr.GetTable<NegoDraftAcceptance>()
                , d => d.DocID, r => r.AcceptanceID, (d, r) => r)
                .Join(mgr.GetTable<NegoDraft>()
                        .Where(n => n.NegoDraftExtension.DueDate < DateTime.Today.AddDays(1))
                        .Join(mgr.GetTable<LetterOfCredit>()
                            .Join(mgr.GetTable<CreditApplicationDocumentary>()
                                .Join(userProfile.GetActionBranch(mgr), a => a.IssuingBankCode, b => b.BankCode, (a, b) => a)
                            , l => l.ApplicationID, c => c.DocumentaryID, (l, c) => l)
                        , n => n.NegoLcVersionID, l => l.LcID, (n, l) => n)
                , r => r.DraftID, n => n.DocumentaryID, (r, n) => r);
        }

        public static IQueryable<NegoDraftAcceptance> GetDraftAcceptanceToVerifyList(this UserProfile userProfile, GenericManager<LcEntityDbContext> mgr)
        {
            return mgr.GetTable<Documentary>()
                .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待主管審核)
                .Join(mgr.GetTable<NegoDraftAcceptance>()
                , d => d.DocID, r => r.AcceptanceID, (d, r) => r)
                .Join(mgr.GetTable<NegoDraft>()
                        .Where(n => n.NegoDraftExtension.DueDate < DateTime.Today.AddDays(1))
                        .Join(mgr.GetTable<LetterOfCredit>()
                            .Join(mgr.GetTable<CreditApplicationDocumentary>()
                                .Join(userProfile.GetActionBranch(mgr), a => a.IssuingBankCode, b => b.BankCode, (a, b) => a)
                            , l => l.ApplicationID, c => c.DocumentaryID, (l, c) => l)
                        , n => n.NegoLcVersionID, l => l.LcID, (n, l) => n)
                , r => r.DraftID, n => n.DocumentaryID, (r, n) => r);
        }

        public static IQueryable<NegoDraftAcceptance> GetDraftAcceptanceToMarkList(this UserProfile userProfile, GenericManager<LcEntityDbContext> mgr)
        {
            return mgr.GetTable<Documentary>()
                .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待註記)
                .Join(mgr.GetTable<NegoDraftAcceptance>()
                , d => d.DocID, r => r.AcceptanceID, (d, r) => r)
                .Join(mgr.GetTable<NegoDraft>()
                        .Where(n => n.NegoDraftExtension.DueDate < DateTime.Today.AddDays(1))
                        .Join(mgr.GetTable<LetterOfCredit>()
                            .Join(mgr.GetTable<CreditApplicationDocumentary>()
                                .Join(userProfile.GetActionBranch(mgr), a => a.IssuingBankCode, b => b.BankCode, (a, b) => a)
                            , l => l.ApplicationID, c => c.DocumentaryID, (l, c) => l)
                        , n => n.NegoLcVersionID, l => l.LcID, (n, l) => n)
                , r => r.DraftID, n => n.DocumentaryID, (r, n) => r);
        }


        public static IQueryable<NegoDraft> GetDefaultQueryByUser(this UserProfile userProfile, DbSet<NegoDraft> table, Expression<Func<NegoDraftExtension, bool>> queryExpr = null)
        {
            var dbContext = table.GetService<DbContext>();
            var bankCode = userProfile.GetActionBranch(dbContext).Select(b => b.BankCode);
            if (queryExpr != null)
            {
                return table
                    .Join(dbContext.Set<NegoDraftExtension>().Where(queryExpr)
                        .Where(l => bankCode.Contains(l.NegoBranch) || bankCode.Contains(l.LcBranch))
                    , n => n.DocumentaryID, l => l.NegoDraftID, (n, l) => n);
            }
            else
            {
                return table
                .Join(dbContext.Set<NegoDraftExtension>()
                        .Where(l => bankCode.Contains(l.NegoBranch) || bankCode.Contains(l.LcBranch))
                , n => n.DocumentaryID, l => l.NegoDraftID, (n, l) => n);
            }
        }

        public static IQueryable<NegoDraft> GetDefaultQueryByUser(this UserProfile userProfile, GenericManager<LcEntityDbContext> mgr, IQueryable<NegoDraft> items, IQueryable<NegoDraftExtension> extItems)
        {
            var bankCode = userProfile.GetActionBranch(mgr).Select(b => b.BankCode);
            extItems = extItems
                        .Where(l => bankCode.Contains(l.NegoBranch) || bankCode.Contains(l.LcBranch));

            return items
                .Join(extItems, n => n.DocumentaryID, l => l.NegoDraftID, (n, l) => n);
        }

        public static IQueryable<BankData> GetEffectiveBranches(this GenericManager<LcEntityDbContext> models)
        {
            return models.GetTable<BankData>().Where(b => b.DisabledBranch == null);
        }

        public static NegoDraft UpdateAppSeq(this NegoDraft draftItem,GenericManager<LcEntityDbContext> models)
        {
            models.ExecuteCommand(@"UPDATE          NegoDraft
                                    SET                   AppSeq = (select MAX(n.AppSeq) + 1 from NegoDraft n where n.AppYear = AppYear)
                                    FROM              NegoDraft 
                                    WHERE          (NegoDraft.DocumentaryID = {0})", draftItem.DocumentaryID);
            return draftItem;
        }

    }
}
