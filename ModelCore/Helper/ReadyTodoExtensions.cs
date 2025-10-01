using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CommonLib.DataAccess;
using EAI.Service.Transaction;
using ModelCore.DataModel;
using ModelCore.Locale;
using ModelCore.Properties;
using ModelCore.UserManagement;

namespace ModelCore.Helper
{
    public static class ReadyTodoExtensions
    {
        internal static int?[] __IDELC0101_DataScope = new int?[]
        {
            (int)Naming.DocumentLevel.待經辦審核,
            (int)Naming.DocumentLevel.已退回_主管退回,
            (int)Naming.DocumentLevel.已退回_CRC退回,
        };

        internal static int?[] __Check_To_Accept_Document = new int?[]
        {
            (int)Naming.DocumentTypeDefinition.開狀申請書,
            (int)Naming.DocumentTypeDefinition.修狀申請書,
            (int)Naming.DocumentTypeDefinition.押匯申請書,
            (int)Naming.DocumentTypeDefinition.還款改貸申請書,
        };

        public static IQueryable<Documentary> PromptToAccept(this GenericManager<LcEntityDataContext> models)
        {
            return models.GetTable<Documentary>()
                .Where(d => __IDELC0101_DataScope.Contains(d.CurrentLevel))
                .Where(d => __Check_To_Accept_Document.Contains(d.DocType));
        }


        public static IQueryable<Documentary> PromptToAcceptALC(this GenericManager<LcEntityDataContext> models, UserProfile profile)
        {
            return models.GetTable<Documentary>().Where(d =>
                __IDELC0101_DataScope.Contains(d.CurrentLevel))
                    .Join(models.GetTable<CreditApplicationDocumentary>()
                        .Join(profile.GetActionBranch(models), a => a.開狀行, b => b.BankCode, (a, b) => a)
                    , d => d.DocID, a => a.AppID, (d, a) => d);
        }

        public static IQueryable<Documentary> PromptToVerifyALC(this GenericManager<LcEntityDataContext> models, UserProfile profile)
        {
            return models.GetTable<Documentary>().Where(d =>
                d.CurrentLevel == (int)Naming.DocumentLevel.待主管審核)
                    .Join(models.GetTable<CreditApplicationDocumentary>()
                        .Join(profile.GetActionBranch(models), a => a.開狀行, b => b.BankCode, (a, b) => a)
                    , d => d.DocID, a => a.AppID, (d, a) => d);
        }

        public static IQueryable<Documentary> PromptToRegisterALC(this GenericManager<LcEntityDataContext> models, UserProfile profile)
        {
            return models.GetTable<Documentary>().Where(d =>
                d.CurrentLevel == (int)Naming.DocumentLevel.待CRC登錄
                || d.CurrentLevel == (int)Naming.DocumentLevel.已退回_CRC主管退回)
                    .Join(models.GetTable<CreditApplicationDocumentary>()
                        .Join(profile.GetActionBranch(models), a => a.開狀行, b => b.BankCode, (a, b) => a)
                    , d => d.DocID, a => a.AppID, (d, a) => d);
        }

        public static IQueryable<Documentary> PromptToAllowALC(this GenericManager<LcEntityDataContext> models, UserProfile profile)
        {
            return models.GetTable<Documentary>().Where(d =>
                d.CurrentLevel == (int)Naming.DocumentLevel.待放行)
                    .Join(models.GetTable<CreditApplicationDocumentary>()
                        .Join(profile.GetActionBranch(models), a => a.開狀行, b => b.BankCode, (a, b) => a)
                    , d => d.DocID, a => a.AppID, (d, a) => d);
        }

        public static IQueryable<Documentary> PromptToMarkALC(this GenericManager<LcEntityDataContext> models, UserProfile profile)
        {
            return models.GetTable<Documentary>().Where(d =>
                d.CurrentLevel == (int)Naming.DocumentLevel.待註記)
                    .Join(models.GetTable<CreditApplicationDocumentary>()
                        .Join(profile.GetActionBranch(models), a => a.開狀行, b => b.BankCode, (a, b) => a)
                    , d => d.DocID, a => a.AppID, (d, a) => d);
        }

        public static IQueryable<Documentary> PromptToAcceptALCByCounter(this GenericManager<LcEntityDataContext> models, UserProfile profile)
        {
            return models.GetTable<Documentary>().Where(d =>
                d.CurrentLevel == (int)Naming.DocumentLevel.臨櫃申請CRC主管退回
                || d.CurrentLevel == (int)Naming.DocumentLevel.臨櫃申請待登錄)
                    .Join(models.GetTable<CreditApplicationDocumentary>()
                        //.Where(c => c.FpgLcItem != null)
                        .Join(profile.GetActionBranch(models), a => a.開狀行, b => b.BankCode, (a, b) => a)
                    , d => d.DocID, a => a.AppID, (d, a) => d);
        }

        public static IQueryable<Documentary> PromptToRegisterALCByCounter(this GenericManager<LcEntityDataContext> models, UserProfile profile)
        {
            return models.GetTable<Documentary>().Where(d =>
                d.CurrentLevel == (int)Naming.DocumentLevel.臨櫃申請待登錄)
                    .Join(models.GetTable<CreditApplicationDocumentary>()
                        .Join(profile.GetActionBranch(models), a => a.開狀行, b => b.BankCode, (a, b) => a)
                    , d => d.DocID, a => a.AppID, (d, a) => d);
        }

        public static IQueryable<AmendingLcApplication> PromptToAcceptMLC(this GenericManager<LcEntityDataContext> models, UserProfile profile)
        {
            return models.GetTable<Documentary>().Where(d => __IDELC0101_DataScope.Contains(d.CurrentLevel))
                    .Join(models.PromptAmendingLcApplication(profile),
                        d => d.DocID, a => a.AmendingID, (d, a) => a);
        }

        public static IQueryable<AmendingLcApplication> PromptToVerifyMLC(this GenericManager<LcEntityDataContext> models, UserProfile profile)
        {
            return models.GetTable<Documentary>().Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待主管審核)
                    .Join(models.PromptAmendingLcApplication(profile),
                        d => d.DocID, a => a.AmendingID, (d, a) => a);
        }

        public static IQueryable<AmendingLcApplication> PromptToRegisterMLC(this GenericManager<LcEntityDataContext> models, UserProfile profile)
        {
            return models.GetTable<Documentary>().Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待CRC登錄
                || d.CurrentLevel == (int)Naming.DocumentLevel.已退回_CRC主管退回)
                    .Join(models.PromptAmendingLcApplication(profile),
                        d => d.DocID, a => a.AmendingID, (d, a) => a);
        }

        public static IQueryable<AmendingLcApplication> PromptToAllowMLC(this GenericManager<LcEntityDataContext> models, UserProfile profile)
        {
            return models.GetTable<Documentary>().Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待放行)
                    .Join(models.PromptAmendingLcApplication(profile),
                        d => d.DocID, a => a.AmendingID, (d, a) => a);
        }

        public static IQueryable<AmendingLcApplication> PromptToMarkMLC(this GenericManager<LcEntityDataContext> models, UserProfile profile)
        {
            return models.GetTable<Documentary>().Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待註記)
                    .Join(models.PromptAmendingLcApplication(profile),
                        d => d.DocID, a => a.AmendingID, (d, a) => a);
        }

        public static IQueryable<AmendingLcApplication> PromptToAcceptMLCByCounter(this GenericManager<LcEntityDataContext> models, UserProfile profile)
        {
            return models.GetTable<Documentary>()
                    .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.臨櫃申請CRC主管退回
                        || d.CurrentLevel == (int)Naming.DocumentLevel.臨櫃申請待登錄)
                    .Join(models.PromptAmendingLcApplication(profile),
                        d => d.DocID, a => a.AmendingID, (d, a) => a); 
        }

        public static IQueryable<AmendingLcApplication> PromptToRegisterMLCByCounter(this GenericManager<LcEntityDataContext> models, UserProfile profile)
        {
            return models.GetTable<Documentary>()
                    .Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.臨櫃申請待登錄)
                    .Join(models.PromptAmendingLcApplication(profile),
                        d => d.DocID, a => a.AmendingID, (d, a) => a);
        }

        public static IQueryable<CreditCancellation> PromptToAcceptBCL(this GenericManager<LcEntityDataContext> models, UserProfile profile)
        {
            return models.GetTable<Documentary>().Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待經辦審核
                || d.CurrentLevel == (int)Naming.DocumentLevel.已退回_主管退回)
                    .Join(models.PromptCreditCancellation(profile),
                        d => d.DocID, a => a.CancellationID, (d, a) => a);
        }

        public static IQueryable<CreditCancellation> PromptToVerifyBCL(this GenericManager<LcEntityDataContext> models, UserProfile profile)
        {
            return models.GetTable<Documentary>().Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待主管審核)
                    .Join(models.PromptCreditCancellation(profile),
                        d => d.DocID, a => a.CancellationID, (d, a) => a);
        }

        public static IQueryable<CreditCancellation> PromptToRegisterAutoBCL(this GenericManager<LcEntityDataContext> models, UserProfile profile)
        {
            return models.GetTable<Documentary>().Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.主動餘額註銷_待登錄)
                    .Join(models.PromptCreditCancellation(profile),
                        d => d.DocID, a => a.CancellationID, (d, a) => a);
        }

        public static IQueryable<CreditCancellation> PromptToAllowAutoBCL(this GenericManager<LcEntityDataContext> models, UserProfile profile)
        {
            return models.GetTable<Documentary>().Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.主動餘額註銷_待放行)
                    .Join(models.PromptCreditCancellation(profile),
                        d => d.DocID, a => a.CancellationID, (d, a) => a);
        }

        public static IQueryable<CreditCancellation> PromptToMarkBCL(this GenericManager<LcEntityDataContext> models, UserProfile profile)
        {
            return models.GetTable<Documentary>().Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.待註記)
                    .Join(models.PromptCreditCancellation(profile),
                        d => d.DocID, a => a.CancellationID, (d, a) => a);
        }

        public static IQueryable<AmendingLcApplication> PromptAmendingLcApplication(this GenericManager<LcEntityDataContext> models, UserProfile profile)
        {
            var lcItems = models.GetTable<LetterOfCredit>()
                .Join(models.GetTable<CreditApplicationDocumentary>()
                    .Join(profile.GetActionBranch(models), a => a.開狀行, b => b.BankCode, (a, b) => a),
                    l => l.AppID, d => d.AppID, (l, d) => l);
            var versionItems = models.GetTable<LetterOfCreditVersion>()
                .Join(lcItems, a => a.LcID, l => l.LcID, (a, l) => a);
            // Filter for amending applications
            return models.GetTable<AmendingLcApplication>()
                    .Where(a => versionItems.Any(v => v.VersionID == a.SourceID));
        }

        public static IQueryable<CreditCancellation> PromptCreditCancellation(this GenericManager<LcEntityDataContext> models, UserProfile profile)
        {
            return models.GetTable<CreditCancellation>()
                    .Join(models.GetTable<LetterOfCredit>()
                        .Join(models.GetTable<CreditApplicationDocumentary>()
                            .Join(profile.GetActionBranch(models), a => a.開狀行, b => b.BankCode, (a, b) => a),
                            l => l.AppID, d => d.AppID, (l, d) => l),
                        a => a.LcID, l => l.LcID, (a, l) => a);
        }

        public static IQueryable<BeneficiaryData> PromptToVerifyBeneficiary(this GenericManager<LcEntityDataContext> models, UserProfile profile)
        {
            return models.GetTable<BeneficiaryData>().Where(b => (b.Status == (int)Naming.BeneficiaryStatus.修改待放行 || b.Status == (int)Naming.BeneficiaryStatus.新增待放行 || b.Status == (int)Naming.BeneficiaryStatus.刪除待放行));
        }

        public static IQueryable<CustomerOfBranch> PromptToVerifyCustomer(this GenericManager<LcEntityDataContext> models, UserProfile profile)
        {
            return models.GetTable<CustomerOfBranch>().Where(b => profile.BranchCodeItems.Contains(b.BankCode)
                && (b.CurrentLevel == (int)Naming.BeneficiaryStatus.修改待放行 || b.CurrentLevel == (int)Naming.BeneficiaryStatus.新增待放行 || b.CurrentLevel == (int)Naming.BeneficiaryStatus.刪除待放行));
        }

        public static IQueryable<OrganizationBranchSettings> PromptToVerifyOrganizationSettings(this GenericManager<LcEntityDataContext> models, UserProfile profile)
        {
            return models.GetTable<OrganizationBranchSettings>().Where(b => profile.BranchCodeItems.Contains(b.BankCode)
                && (b.Status == (int)Naming.BeneficiaryStatus.修改待放行 || b.Status == (int)Naming.BeneficiaryStatus.新增待放行 || b.Status == (int)Naming.BeneficiaryStatus.刪除待放行));
        }

    }
}
