﻿using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CommonLib.DataAccess;
using EAI.Service.Transaction;
using ModelCore.BankManagement;
using ModelCore.DataModel;
using ModelCore.Locale;
using ModelCore.Properties;
using ModelCore.UserManagement;
using CommonLib.Utility;

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

        public static string ReadErrCodeDis(this GenericManager<LcEntityDataContext> mgr, IEAITransaction transaction)
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

        public static IQueryable<BankData> GetActionBranch(this int userID, GenericManager<LcEntityDataContext> models)
        {
            return userID.GetActionBranch(models.DataContext);
        }

        public static IQueryable<BankData> GetActionBranch(this UserProfile userProfile, GenericManager<LcEntityDataContext> models)
        {
            return userProfile.ProfileData.USER_ID.GetActionBranch(models.DataContext);
        }

        public static IQueryable<BankData> GetActionBranch(this UserProfile userProfile, DataContext dbContext)
        {
            return userProfile.ProfileData.USER_ID.GetActionBranch(dbContext);
        }

        public static IQueryable<BankData> GetActionBranch(this int userID, DataContext dbContext)
        {
            return dbContext.GetTable<BankUserBranch>().Where(b => b.USER_ID == userID)
                .Select(b => b.BankData);
        }

        public static IQueryable<NegoDraft> GetEffectiveNegoDraft(this UserProfile userProfile, GenericManager<LcEntityDataContext> mgr)
        {
            return mgr.GetTable<NegoDraft>()
                .Join(mgr.GetTable<LetterOfCredit>()
                    .Join(mgr.GetTable<CreditApplicationDocumentary>()
                        .Join(userProfile.GetActionBranch(mgr), a => a.通知行, b => b.BankCode, (a, b) => a)
                    , l => l.AppID, c => c.AppID, (l, c) => l)
                , n => n.LcID, l => l.LcID, (n, l) => n);
        }

        //public static IQueryable<LetterOfCredit> GetValidLc(this IQueryable<LetterOfCredit> items)
        //{
        //    return items.Where(l => l.可用餘額 > 0 && l.LcItem.有效期限 >= DateTime.Today);
        //}

        public static IQueryable<CreditApplicationDocumentary> GetLcPrintNoticeList(this UserProfile userProfile, GenericManager<LcEntityDataContext> mgr)
        {
            var bankItems = userProfile.GetActionBranch(mgr);
            return mgr.GetTable<LetterOfCredit>()
                .Select(l=>l.CreditApplicationDocumentary)
                .Where(d => (d.BeneficiaryData.DraftType == (int)Naming.BeneficiaryDraftType.非電子押匯 
                    || d.BeneficiaryData.DraftType == (int)Naming.BeneficiaryDraftType.電子押匯_電子申請
                    || d.BeneficiaryData.DraftType == (int)Naming.BeneficiaryDraftType.電子押匯_台塑集團)
                        && bankItems.Any(b=>b.BankCode == d.通知行))
                .Join(
                    mgr.GetTable<Documentary>().Where(o => o.LcPrintNotice == null), d => d.AppID, o => o.DocID, (d, o) => d);
        }

        public static IQueryable<AmendingLcApplication> GetAmendingInfoPrintNoticeList(this UserProfile userProfile, GenericManager<LcEntityDataContext> mgr)
        {
            var bankItems = userProfile.GetActionBranch(mgr);
            // 取得所有符合使用者分行的信用狀開狀申請
            var appItems = mgr.GetTable<CreditApplicationDocumentary>()
                        .Where(d => (d.BeneficiaryData.DraftType == (int)Naming.BeneficiaryDraftType.非電子押匯
                            || d.BeneficiaryData.DraftType == (int)Naming.BeneficiaryDraftType.電子押匯_電子申請
                            || d.BeneficiaryData.DraftType == (int)Naming.BeneficiaryDraftType.電子押匯_台塑集團)
                                && bankItems.Any(b => b.BankCode == d.通知行));
            // 取得所有信用狀的開狀申請
            var lcItems = mgr.GetTable<LetterOfCredit>()
                    .Join(appItems,
                    l => l.AppID, d => d.AppID, (l, d) => l);

            var versionItems = mgr.GetTable<LetterOfCreditVersion>()
                .Join(lcItems, a => a.LcID, l => l.LcID, (a, l) => a);

            return mgr.GetTable<AmendingLcInformation>()
                .Join(versionItems,
                    a => a.AmendingID, l => l.AmendingID, (a, l) => a)
                .Select(l => l.AmendingLcApplication)
                .Join(mgr.GetTable<Documentary>().Where(o => o.LcPrintNotice == null), a => a.AmendingID, o => o.DocID, (a, o) => a);
        }

        public static IQueryable<NegoDraft> GetNegoDraftTodoList(this UserProfile userProfile, GenericManager<LcEntityDataContext> mgr)
        {
            return mgr.GetTable<Documentary>()
                .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待經辦審核
                    || d.CurrentLevel == (int)Naming.DocumentLevel.已退回_CRC退回
                    || d.CurrentLevel == (int)Naming.DocumentLevel.已退回_主管退回)
                .Join(mgr.GetTable<NegoDraft>()
                        .Join(mgr.GetTable<NegoDraftExtension>()
                            .Join(userProfile.GetActionBranch(mgr), a => a.NegoBranch, b => b.BankCode, (a, b) => a)
                        , n => n.DraftID, l => l.DraftID, (n, l) => n)
                , d => d.DocID, n => n.DraftID, (d, n) => n);
        }

        public static IQueryable<NegoDraft> GetNegoDraftToVerifyList(this UserProfile userProfile, GenericManager<LcEntityDataContext> mgr)
        {
            return mgr.GetTable<Documentary>()
                .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待主管審核)
                .Join(mgr.GetTable<NegoDraft>()
                        .Join(mgr.GetTable<NegoDraftExtension>()
                            .Join(userProfile.GetActionBranch(mgr), a => a.NegoBranch, b => b.BankCode, (a, b) => a)
                        , n => n.DraftID, l => l.DraftID, (n, l) => n)
                , d => d.DocID, n => n.DraftID, (d, n) => n);
        }

        public static IQueryable<NegoDraft> GetNegoDraftToRegisterList(this UserProfile userProfile, GenericManager<LcEntityDataContext> mgr)
        {
            return mgr.GetTable<Documentary>()
                .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待CRC登錄
                    || d.CurrentLevel == (int)Naming.DocumentLevel.已退回_CRC主管退回)
                .Join(mgr.GetTable<NegoDraft>()
                        .Join(mgr.GetTable<NegoDraftExtension>()
                            .Join(userProfile.GetActionBranch(mgr), a => a.LcBranch, b => b.BankCode, (a, b) => a)
                        , n => n.DraftID, l => l.DraftID, (n, l) => n)
                , d => d.DocID, n => n.DraftID, (d, n) => n);
        }

        public static IQueryable<NegoDraft> GetNegoDraftToAllowList(this UserProfile userProfile, GenericManager<LcEntityDataContext> mgr)
        {
            return mgr.GetTable<Documentary>()
                .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待放行)
                .Join(mgr.GetTable<NegoDraft>()
                        .Join(mgr.GetTable<NegoDraftExtension>()
                            .Join(userProfile.GetActionBranch(mgr), a => a.LcBranch, b => b.BankCode, (a, b) => a)
                        , n => n.DraftID, l => l.DraftID, (n, l) => n)
                , d => d.DocID, n => n.DraftID, (d, n) => n);
        }

        public static IQueryable<NegoDraft> GetNegoDraftToMarkList(this UserProfile userProfile, GenericManager<LcEntityDataContext> mgr)
        {
            return mgr.GetTable<Documentary>()
                .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待註記)
                .Join(mgr.GetTable<NegoDraft>()
                        .Join(mgr.GetTable<NegoDraftExtension>()
                            .Join(userProfile.GetActionBranch(mgr), a => a.LcBranch, b => b.BankCode, (a, b) => a)
                        , n => n.DraftID, l => l.DraftID, (n, l) => n)
                , d => d.DocID, n => n.DraftID, (d, n) => n);
        }

        public static IQueryable<NegoDraft> FilterNegoDraftByServiceType(this IQueryable<NegoDraft> items, GenericManager<LcEntityDataContext> models, Naming.DraftType? draftType)
        {
            if (draftType.HasValue)
            {
                items = items.Join(models.GetTable<NegoDraftExtension>().Where(t => t.DraftType == (int?)draftType),
                                n => n.DraftID, t => t.DraftID, (n, t) => n);
            }
            return items;
        }

        public static IQueryable<NegoDraft> FilterNegoDraftByRemitting(this IQueryable<NegoDraft> items, GenericManager<LcEntityDataContext> models, Naming.DraftType draftType)
        {
            items = items.Join(models.GetTable<NegoDraftExtension>().Where(t => t.DraftType == (int)draftType),
                            n => n.DraftID, t => t.DraftID, (n, t) => n)
                        .Join(models.GetTable<FpgNegoDraft>().Where(f => !f.匯入銀行代碼.StartsWith("009")),
                            n => n.DraftID, f => f.DraftID, (n, f) => n);

            return items;
        }

        public static IQueryable<NegoDraftAcceptance> GetDraftAcceptanceTodoList(this UserProfile userProfile, GenericManager<LcEntityDataContext> mgr)
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
                                .Join(userProfile.GetActionBranch(mgr), a => a.開狀行, b => b.BankCode, (a, b) => a)
                            , l => l.AppID, c => c.AppID, (l, c) => l)
                        , n => n.LcID, l => l.LcID, (n, l) => n)
                , r => r.DraftID, n => n.DraftID, (r, n) => r);
        }

        public static IQueryable<NegoDraftAcceptance> GetDraftAcceptanceToVerifyList(this UserProfile userProfile, GenericManager<LcEntityDataContext> mgr)
        {
            return mgr.GetTable<Documentary>()
                .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待主管審核)
                .Join(mgr.GetTable<NegoDraftAcceptance>()
                , d => d.DocID, r => r.AcceptanceID, (d, r) => r)
                .Join(mgr.GetTable<NegoDraft>()
                        .Where(n => n.NegoDraftExtension.DueDate < DateTime.Today.AddDays(1))
                        .Join(mgr.GetTable<LetterOfCredit>()
                            .Join(mgr.GetTable<CreditApplicationDocumentary>()
                                .Join(userProfile.GetActionBranch(mgr), a => a.開狀行, b => b.BankCode, (a, b) => a)
                            , l => l.AppID, c => c.AppID, (l, c) => l)
                        , n => n.LcID, l => l.LcID, (n, l) => n)
                , r => r.DraftID, n => n.DraftID, (r, n) => r);
        }

        public static IQueryable<NegoDraftAcceptance> GetDraftAcceptanceToMarkList(this UserProfile userProfile, GenericManager<LcEntityDataContext> mgr)
        {
            return mgr.GetTable<Documentary>()
                .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待註記)
                .Join(mgr.GetTable<NegoDraftAcceptance>()
                , d => d.DocID, r => r.AcceptanceID, (d, r) => r)
                .Join(mgr.GetTable<NegoDraft>()
                        .Where(n => n.NegoDraftExtension.DueDate < DateTime.Today.AddDays(1))
                        .Join(mgr.GetTable<LetterOfCredit>()
                            .Join(mgr.GetTable<CreditApplicationDocumentary>()
                                .Join(userProfile.GetActionBranch(mgr), a => a.開狀行, b => b.BankCode, (a, b) => a)
                            , l => l.AppID, c => c.AppID, (l, c) => l)
                        , n => n.LcID, l => l.LcID, (n, l) => n)
                , r => r.DraftID, n => n.DraftID, (r, n) => r);
        }


        public static IQueryable<Reimbursement> GetReimbursementTodoList(this UserProfile userProfile, GenericManager<LcEntityDataContext> mgr)
        {
            return mgr.GetTable<Documentary>()
                .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待經辦審核
                    || d.CurrentLevel == (int)Naming.DocumentLevel.已退回_CRC退回
                    || d.CurrentLevel == (int)Naming.DocumentLevel.已退回_主管退回)
                .Join(mgr.GetTable<Reimbursement>()
                    .Where(r => r.NegoLoan == null)
                , d => d.DocID, r => r.ReimID, (d, r) => r)
                .Join(mgr.GetTable<NegoDraft>()
                        .Join(mgr.GetTable<LetterOfCredit>()
                            .Join(mgr.GetTable<CreditApplicationDocumentary>()
                                .Join(userProfile.GetActionBranch(mgr), a => a.開狀行, b => b.BankCode, (a, b) => a)
                            , l => l.AppID, c => c.AppID, (l, c) => l)
                        , n => n.LcID, l => l.LcID, (n, l) => n)
                , r => r.DraftID, n => n.DraftID, (r, n) => r)
                .Where(r => r.NegoLoan == null);
        }

        public static IQueryable<Reimbursement> GetReimbursementToVerifyList(this UserProfile userProfile, GenericManager<LcEntityDataContext> mgr)
        {
            return mgr.GetTable<Documentary>()
                .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待主管審核)
                .Join(mgr.GetTable<Reimbursement>()
                    .Where(r => r.NegoLoan == null)
                , d => d.DocID, r => r.ReimID, (d, r) => r)
                .Join(mgr.GetTable<NegoDraft>()
                        .Join(mgr.GetTable<LetterOfCredit>()
                            .Join(mgr.GetTable<CreditApplicationDocumentary>()
                                .Join(userProfile.GetActionBranch(mgr), a => a.開狀行, b => b.BankCode, (a, b) => a)
                            , l => l.AppID, c => c.AppID, (l, c) => l)
                        , n => n.LcID, l => l.LcID, (n, l) => n)
                , r => r.DraftID, n => n.DraftID, (r, n) => r)
                .Where(r => r.NegoLoan == null);
        }

        public static IQueryable<Reimbursement> GetReimbursementToMarkList(this UserProfile userProfile, GenericManager<LcEntityDataContext> mgr)
        {
            return mgr.GetTable<Documentary>()
                .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待註記)
                .Join(mgr.GetTable<Reimbursement>()
                    .Where(r => r.NegoLoan == null)
                , d => d.DocID, r => r.ReimID, (d, r) => r)
                .Join(mgr.GetTable<NegoDraft>()
                        .Join(mgr.GetTable<LetterOfCredit>()
                            .Join(mgr.GetTable<CreditApplicationDocumentary>()
                                .Join(userProfile.GetActionBranch(mgr), a => a.開狀行, b => b.BankCode, (a, b) => a)
                            , l => l.AppID, c => c.AppID, (l, c) => l)
                        , n => n.LcID, l => l.LcID, (n, l) => n)
                , r => r.DraftID, n => n.DraftID, (r, n) => r)
                .Where(r => r.NegoLoan == null);
        }

        public static IQueryable<Reimbursement> GetNegoLoanTodoList(this UserProfile userProfile, GenericManager<LcEntityDataContext> mgr)
        {
            return mgr.GetTable<Documentary>()
                .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待經辦審核
                    || d.CurrentLevel == (int)Naming.DocumentLevel.已退回_CRC退回
                    || d.CurrentLevel == (int)Naming.DocumentLevel.已退回_主管退回)
                .Join(mgr.GetTable<Reimbursement>()
                , d => d.DocID, r => r.ReimID, (d, r) => r)
                .Join(mgr.GetTable<NegoDraft>()
                        .Join(mgr.GetTable<LetterOfCredit>()
                            .Join(mgr.GetTable<CreditApplicationDocumentary>()
                                .Join(userProfile.GetActionBranch(mgr), a => a.開狀行, b => b.BankCode, (a, b) => a)
                            , l => l.AppID, c => c.AppID, (l, c) => l)
                        , n => n.LcID, l => l.LcID, (n, l) => n)
                , r => r.DraftID, n => n.DraftID, (r, n) => r)
                .Where(r => r.NegoLoan != null);
        }

        public static IQueryable<Reimbursement> GetNegoLoanToVerifyList(this UserProfile userProfile, GenericManager<LcEntityDataContext> mgr)
        {
            return mgr.GetTable<Documentary>()
                .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待主管審核)
                .Join(mgr.GetTable<Reimbursement>()
                , d => d.DocID, r => r.ReimID, (d, r) => r)
                .Join(mgr.GetTable<NegoDraft>()
                        .Join(mgr.GetTable<LetterOfCredit>()
                            .Join(mgr.GetTable<CreditApplicationDocumentary>()
                                .Join(userProfile.GetActionBranch(mgr), a => a.開狀行, b => b.BankCode, (a, b) => a)
                            , l => l.AppID, c => c.AppID, (l, c) => l)
                        , n => n.LcID, l => l.LcID, (n, l) => n)
                , r => r.DraftID, n => n.DraftID, (r, n) => r)
                .Where(r => r.NegoLoan != null);
        }

        public static IQueryable<Reimbursement> GetNegoLoanToRegisterList(this UserProfile userProfile, GenericManager<LcEntityDataContext> mgr)
        {
            return mgr.GetTable<Documentary>()
                .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待CRC登錄
                    || d.CurrentLevel == (int)Naming.DocumentLevel.已退回_CRC主管退回)
                .Join(mgr.GetTable<Reimbursement>()
                , d => d.DocID, r => r.ReimID, (d, r) => r)
                .Join(mgr.GetTable<NegoDraft>()
                        .Join(mgr.GetTable<LetterOfCredit>()
                            .Join(mgr.GetTable<CreditApplicationDocumentary>()
                                .Join(userProfile.GetActionBranch(mgr), a => a.開狀行, b => b.BankCode, (a, b) => a)
                            , l => l.AppID, c => c.AppID, (l, c) => l)
                        , n => n.LcID, l => l.LcID, (n, l) => n)
                , r => r.DraftID, n => n.DraftID, (r, n) => r)
                .Where(r => r.NegoLoan != null);
        }

        public static IQueryable<Reimbursement> GetNegoLoanToAllowList(this UserProfile userProfile, GenericManager<LcEntityDataContext> mgr)
        {
            return mgr.GetTable<Documentary>()
                .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待放行)
                .Join(mgr.GetTable<Reimbursement>()
                , d => d.DocID, r => r.ReimID, (d, r) => r)
                .Join(mgr.GetTable<NegoDraft>()
                        .Join(mgr.GetTable<LetterOfCredit>()
                            .Join(mgr.GetTable<CreditApplicationDocumentary>()
                                .Join(userProfile.GetActionBranch(mgr), a => a.開狀行, b => b.BankCode, (a, b) => a)
                            , l => l.AppID, c => c.AppID, (l, c) => l)
                        , n => n.LcID, l => l.LcID, (n, l) => n)
                , r => r.DraftID, n => n.DraftID, (r, n) => r)
                .Where(r => r.NegoLoan != null);
        }

        public static IQueryable<Reimbursement> GetNegoLoanToMarkList(this UserProfile userProfile, GenericManager<LcEntityDataContext> mgr)
        {
            return mgr.GetTable<Documentary>()
                .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待註記)
                .Join(mgr.GetTable<Reimbursement>()
                , d => d.DocID, r => r.ReimID, (d, r) => r)
                .Join(mgr.GetTable<NegoDraft>()
                        .Join(mgr.GetTable<LetterOfCredit>()
                            .Join(mgr.GetTable<CreditApplicationDocumentary>()
                                .Join(userProfile.GetActionBranch(mgr), a => a.開狀行, b => b.BankCode, (a, b) => a)
                            , l => l.AppID, c => c.AppID, (l, c) => l)
                        , n => n.LcID, l => l.LcID, (n, l) => n)
                , r => r.DraftID, n => n.DraftID, (r, n) => r)
                .Where(r => r.NegoLoan != null);
        }

        public static IQueryable<NegoLoanRepayment> GetNegoLoanRepaymentToAllowList(this UserProfile userProfile, GenericManager<LcEntityDataContext> mgr)
        {
            var loanItems = mgr.GetTable<Reimbursement>()
                .Join(mgr.GetTable<NegoDraft>()
                        .Join(mgr.GetTable<LetterOfCredit>()
                            .Join(mgr.GetTable<CreditApplicationDocumentary>()
                                .Join(userProfile.GetActionBranch(mgr), a => a.開狀行, b => b.BankCode, (a, b) => a)
                            , l => l.AppID, c => c.AppID, (l, c) => l)
                        , n => n.LcID, l => l.LcID, (n, l) => n)
                , r => r.DraftID, n => n.DraftID, (r, n) => r)
                .Join(mgr.GetTable<NegoLoan>(), r => r.ReimID, n => n.LoanID, (r, n) => n);

            return mgr.GetTable<NegoLoanRepayment>()
                .Join(mgr.GetTable<Documentary>().Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待放行), r => r.RepaymentID, d => d.DocID, (r, d) => r)
                .Join(loanItems, r => r.LoanID, n => n.LoanID, (r, n) => r);
        }

        public static IQueryable<NegoLoanRepayment> GetNegoLoanRepaymentToMarkList(this UserProfile userProfile, GenericManager<LcEntityDataContext> mgr)
        {
            var loanItems = mgr.GetTable<Reimbursement>()
                .Join(mgr.GetTable<NegoDraft>()
                        .Join(mgr.GetTable<LetterOfCredit>()
                            .Join(mgr.GetTable<CreditApplicationDocumentary>()
                                .Join(userProfile.GetActionBranch(mgr), a => a.開狀行, b => b.BankCode, (a, b) => a)
                            , l => l.AppID, c => c.AppID, (l, c) => l)
                        , n => n.LcID, l => l.LcID, (n, l) => n)
                , r => r.DraftID, n => n.DraftID, (r, n) => r)
                .Join(mgr.GetTable<NegoLoan>(), r => r.ReimID, n => n.LoanID, (r, n) => n);

            return mgr.GetTable<NegoLoanRepayment>()
                .Join(mgr.GetTable<Documentary>().Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待註記), r => r.RepaymentID, d => d.DocID, (r, d) => r)
                .Join(loanItems, r => r.LoanID, n => n.LoanID, (r, n) => r);
        }

        public static bool IsReimbursementReadyToCheck(this Reimbursement item)
        {
            return item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.見票即付 ? item.L8600ID.HasValue
                : item.L4600ID.HasValue;
        }

        public static String AccountNo(this NegoDraft draft)
        {
            return String.Format("{0}89{1}", draft.LcID.HasValue
                    ? draft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.開狀行
                    : draft.NegoLC.IssuingBank,
                 draft.PaymentNotification.CustomerID).CheckNumber();
        }

        public static String BranchNo(this NegoDraft draft)
        {
            return draft.NegoDraftRegistry.支號;
        }

        public static String ACTNO(this NegoDraft draft)
        {
            return (draft.AccountNo() + draft.BranchNo()).Replace("-", "");
        }

        public static String AccountNo(this CreditApplicationDocumentary item)
        {
            return String.Format("{0}89{1}", item.開狀行, item.PaymentNotification.CustomerID).CheckNumber();
        }

        public static IQueryable<NegoDraft> GetDefaultQueryByUser(this UserProfile userProfile, Table<NegoDraft> table, Expression<Func<NegoDraftExtension, bool>> queryExpr = null)
        {
            var bankCode = userProfile.GetActionBranch(table.Context).Select(b => b.BankCode);
            if (queryExpr != null)
            {
                return table
                    .Join(table.Context.GetTable<NegoDraftExtension>().Where(queryExpr)
                        .Where(l => bankCode.Contains(l.NegoBranch) || bankCode.Contains(l.LcBranch))
                    , n => n.DraftID, l => l.DraftID, (n, l) => n);
            }
            else
            {
                return table
                .Join(table.Context.GetTable<NegoDraftExtension>()
                        .Where(l => bankCode.Contains(l.NegoBranch) || bankCode.Contains(l.LcBranch))
                , n => n.DraftID, l => l.DraftID, (n, l) => n);
            }
        }

        public static IQueryable<NegoDraft> GetDefaultQueryByUser(this UserProfile userProfile, GenericManager<LcEntityDataContext> mgr, IQueryable<NegoDraft> items, IQueryable<NegoDraftExtension> extItems)
        {
            var bankCode = userProfile.GetActionBranch(mgr).Select(b => b.BankCode);
            extItems = extItems
                        .Where(l => bankCode.Contains(l.NegoBranch) || bankCode.Contains(l.LcBranch));

            return items
                .Join(extItems, n => n.DraftID, l => l.DraftID, (n, l) => n);
        }

        public static IQueryable<BankData> GetEffectiveBranches(this GenericManager<LcEntityDataContext> models)
        {
            return models.GetTable<BankData>().Where(b => b.DisabledBranch == null);
        }

        public static NegoDraft UpdateAppSeq(this NegoDraft draftItem,GenericManager<LcEntityDataContext> models)
        {
            models.ExecuteCommand(@"UPDATE          NegoDraft
                                    SET                   AppSeq = (select MAX(n.AppSeq) + 1 from NegoDraft n where n.AppYear = AppYear)
                                    FROM              NegoDraft 
                                    WHERE          (NegoDraft.DraftID = {0})", draftItem.DraftID);
            return draftItem;
        }

    }
}
