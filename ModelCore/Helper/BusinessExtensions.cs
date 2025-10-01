using System;
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
using ModelCore.Models.ViewModel;

namespace ModelCore.Helper
{
    public static class BusinessExtensions
    {
        public static String CheckCHBAccountNo(this String accountNo, out String[] segment)
        {
            var result = accountNo.CheckCHBAccountNo();
            segment = result?.Split('-');

            return result;
        }

        public static String CheckCHBAccountNo(this String accountNo)
        {
            accountNo = accountNo.GetEfficientString();
            if (accountNo == null || accountNo.Length < 14)
            {
                return null;
            }

            return String.Join("-",
                accountNo.Substring(0, 4),
                accountNo.Substring(4, 2),
                accountNo.Substring(6, 5),
                accountNo.Substring(11, 1),
                accountNo.Substring(12));
        }


        public static IQueryable<BeneficiaryData> PromptBeneficiary(this UserProfile profile,GenericManager<LcEntityDataContext> models,Naming.DraftType? serviceType = null)
        {
            var items = models.GetTable<BeneficiaryData>()
                            .Where(b => b.Status == (int)Naming.BeneficiaryStatus.已核准)
                            .Where(c => c.BankData.DisabledBranch == null);
                            //.Where(b => b.Organization.OrganizationStatus == null
                            //        || b.Organization.OrganizationStatus.SelectedAsBeneficiary == true
                            //        || b.Organization.OrganizationStatus.FpgNegoBeneficiary == true);

            switch(serviceType)
            {
                case Naming.DraftType.CDS_CSC:
                    items = items.Where(b => b.Organization.OrganizationStatus.BeneficiaryGroup.ServiceID == (int)BeneficiaryServiceGroup.ServiceDefinition.UXCDS);
                    break;
                case Naming.DraftType.FPG:
                    items = items.Where(b => b.Organization.OrganizationStatus.BeneficiaryGroup.ServiceID == (int)BeneficiaryServiceGroup.ServiceDefinition.Fpg);
                    break;
                case Naming.DraftType.CHIMEI:
                    items = items.Where(b => b.Organization.OrganizationStatus.BeneficiaryGroup.ServiceID == (int)BeneficiaryServiceGroup.ServiceDefinition.Chimei);
                    break;
                default:
                    //items = items.Where(c => c.DraftType == (int?)serviceType);
                    break;
            }

            //if (serviceType.HasValue)
            //{
            //    if(serviceType == Naming.DraftType.CDS_CSC)
            //    {
            //        items = items.Where(b => b.Organization.OrganizationStatus.BeneficiaryGroup.ServiceID == (int)BeneficiaryServiceGroup.ServiceDefinition.UXCDS);
            //    }
            //    else
            //    {
            //        items = items.Where(c => c.DraftType == (int)serviceType);
            //    }
            //}

            
            return items;

        }

        public static String GetDocumentDescription(this CreditApplicationDocumentary item)
        {
            return item != null ? new StringBuilder()
                .Append("ApplicationNo：").Append(item.ApplicationNo).Append("<br/>")
                .Append("日期：").Append(String.Format("{0:yyyy/MM/dd}", item.ApplicationDate)).Append("<br/>")
                .Append("申請人：").Append(item.ApplicantDetails.CompanyName).Append("<br/>")
                .Append("金額：").Append(String.Format("{0:##,###,###,###}", item.LcItem.開狀金額)).Append("<br/>")
                .Append("有效期限：").Append(String.Format("{0:yyyy/MM/dd}", item.LcItem.有效期限))
                .ToString() : null;
        }

        public static String GetDocumentDescription(this AmendingLcApplication item)
        {
            return item != null ? new StringBuilder()
            .Append("修狀申請書號碼：").Append(item.AmendmentNo).Append("<br/>")
            .Append("日期：").Append(String.Format("{0:yyyy/MM/dd}", item.ApplicationDate)).Append("<br/>")
            .Append("申請人：").Append(item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.ApplicantDetails.CompanyName).Append("<br/>")
            .Append("信用狀號碼：").Append(item.LetterOfCreditVersion.LetterOfCredit.LcNo)
            .ToString() : null;
        }

        public static String GetDocumentDescription(this CreditCancellation item)
        {
            var lc = item.LetterOfCredit;

            return item != null ? new StringBuilder()
            .Append("註銷申請號碼：").Append(item.註銷申請號碼).Append("<br/>")
            .Append("日期：").Append(String.Format("{0:yyyy/MM/dd}", item.申請日期)).Append("<br/>")
            .Append("申請人：").Append(lc.CreditApplicationDocumentary.ApplicantDetails.CompanyName).Append("<br/>")
            .Append("信用狀號碼：").Append(lc.LcNo)
            .ToString() : null;
        }

        public static String GetDocumentDescription(this NegoDraft item)
        {
            return item != null 
                ? String.Concat(
                    "匯票申請書號碼：", item.AppNo(), "<br/>",
                    "匯票號碼：", item.DraftNo, "<br/>",
                    "押匯日期：", String.Format("{0:yyyy/MM/dd}", item.ImportDate), "<br/>",
                    "匯票申請人：", item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.BeneDetails.CompanyName, "<br/>",
                    "押匯金額：", String.Format("{0:##,###,###,###}", item.Amount), "<br/>",
                    "信用狀號碼：", item.LetterOfCreditVersion.LetterOfCredit.LcNo) 
                : null;
        }

        public static String GetDocumentDescription(this Reimbursement item)
        {
            return item != null
                ? item.NegoLoan == null
                    ? String.Concat(
                        "匯票申請書號碼：", item.NegoDraft.AppNo(), "<br/>",
                        "匯票號碼：", item.NegoDraft.DraftNo, "<br/>",
                        "還款日期：", String.Format("{0:yyyy/MM/dd}", item.ApplicationDate), "<br/>",
                        "還款申請人：", item.NegoDraft.LetterOfCreditVersion.LetterOfCredit?.CreditApplicationDocumentary.ApplicantDetails.CompanyName ?? item.NegoDraft.NegoLC?.ApplicantDetails.CompanyName, "<br/>",
                        "還款金額：", String.Format("{0:##,###,###,###}", item.Amount), "<br/>",
                        "約定授權扣款帳號：", item.PayableAccount)
                    : String.Concat(
                        "匯票申請書號碼：", item.NegoDraft.AppNo(), "<br/>",
                        "匯票號碼：", item.NegoDraft.DraftNo, "<br/>",
                        "改貸日期：", String.Format("{0:yyyy/MM/dd}", item.ApplicationDate), "<br/>",
                        "還款申請人：", item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.ApplicantDetails.CompanyName, "<br/>",
                        "改貸金額：", String.Format("{0:##,###,###,###}", item.Amount), "<br/>")
                : null;
        }

        public static String GetDocumentDescription(this NegoLoanRepayment item)
        {
            var loan = item.NegoLoan;
            var reimItem = loan.Reimbursement;
            var draft = reimItem.NegoDraft;
            return item != null
                ? String.Concat(
                        "匯票申請書號碼：", draft.AppNo(), "<br/>",
                        "匯票號碼：", draft.DraftNo, "<br/>",
                        "改貸申請書號碼：", reimItem.ReimbursementNo, "<br/>",
                        "還款日期：", String.Format("{0:yyyy/MM/dd}", item.RepaymentDate), "<br/>",
                        "還款人：", draft.LetterOfCreditVersion?.LetterOfCredit?.CreditApplicationDocumentary.ApplicantDetails.CompanyName ?? draft.NegoLC?.ApplicantDetails.CompanyName, "<br/>",
                        "還款金額：", String.Format("{0:##,###,###,###}", item.RepaymentAmount), "<br/>",
                        "繳付利息金額：", String.Format("{0:##,###,###,###}", item.InterestAmount), "<br/>",
                        "約定授權扣款帳號：", reimItem.PayableAccount)
                : null;
        }

        public static List<LcAmendatory> GetLcAmendmentDetails(this LcAmendmentQueryViewModel viewModel, GenericManager<LcEntityDataContext> models)
        {
            List<LcAmendatory> items = new List<LcAmendatory>();

            LcItem oldItem;
            AttachableDocument oldAttach;
            SpecificNote oldSN;

            var lc = models.GetTable<LetterOfCredit>().Where(l => l.LcID == viewModel.LcID).FirstOrDefault();
            if (lc == null)
                return items;

            var lcVersion = lc.LetterOfCreditVersion.OrderByDescending(v => v.VersionID).First();

            oldItem = lcVersion.LcItem;
            oldAttach = lcVersion.AttachableDocument;
            oldSN = lcVersion.SpecificNote;

            if (viewModel.LcAmt != oldItem.開狀金額)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = "金額：新台幣 " + ValidityAgent.MoneyShow(viewModel.LcAmt),
                    Original = "金額：新台幣 " + ValidityAgent.MoneyShow(oldItem.開狀金額)
                });
            }
            if (viewModel.LcExpiry_D != oldItem.有效期限)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = "有效期限：" + ValidityAgent.ConvertChineseDate(viewModel.LcExpiry_D),
                    Original = "有效期限：" + ValidityAgent.ConvertChineseDate(oldItem.有效期限)
                });
            }

            if (viewModel.UsanceDay != oldItem.定日付款)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = "定日付款天數：" + viewModel.UsanceDay.ToString(),
                    Original = "定日付款天數：" + oldItem.定日付款.ToString()
                });
            }
            if (viewModel.PaymentDate_D != oldItem.PaymentDate)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = "指定付款日：" + ValidityAgent.ConvertChineseDate(viewModel.PaymentDate_D),
                    Original = "指定付款日：" + ValidityAgent.ConvertChineseDate(oldItem.PaymentDate)
                });
            }

            StringBuilder srcGoods = new StringBuilder("貨物名稱：");
            int idx = 1;
            viewModel.Goods = viewModel.Goods.GetEfficientString();
            if (viewModel.Goods != null)
            {
                srcGoods.Append(String.Format("{0}. 品名:{1} 規格:{2} 單價:{3} 數量:{4} 金額:{5} 備註:{6}"
                                , idx++, viewModel.Goods, null, null, null, null, null))
                        .Append("<br/>");
            }
            for (int i = 0; i < viewModel.ProductName.Length; i++)
            {
                srcGoods.Append(String.Format("{0}. 品名:{1} 規格:{2} 單價:{3} 數量:{4} 金額:{5} 備註:{6}"
                            , idx++, viewModel.ProductName[i], viewModel.ProductSize[i], viewModel.UnitPrice[i], viewModel.Quantity[i], viewModel.Amount[i], viewModel.Remark[i]))
                        .Append("<br/>");
            }

            StringBuilder destGoods = new StringBuilder("貨物名稱：");
            idx = 1;
            if (!String.IsNullOrEmpty(oldItem.Goods))
            {
                destGoods.Append(String.Format("{0}. 品名:{1} 規格:{2} 單價:{3} 數量:{4} 金額:{5} 備註:{6}"
                                , idx++, oldItem.Goods, null, null, null, null, null))
                        .Append("<br/>");
            }
            for (int i = 0; i < oldItem.GoodsDetails.Count; i++)
            {
                var g = oldItem.GoodsDetails[i];
                destGoods.Append(String.Format("{0}. 品名:{1} 規格:{2} 單價:{3} 數量:{4} 金額:{5} 備註:{6}"
                            , idx++, g.ProductName, g.ProductSize, g.UnitPrice, g.Quantity, g.Amount, g.Remark))
                        .Append("<br/>");
            }

            String srcGoodsStr = srcGoods.ToString(), destGoodsStr = destGoods.ToString();

            if (srcGoodsStr != destGoodsStr)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = srcGoodsStr,
                    Original = destGoodsStr
                });
            }

            if (viewModel.AttachPayingAcceptance != oldAttach.匯票付款申請書)
            {
                String terms = viewModel.AtSight == true ? "付款" : "承兌";
                items.Add(new LcAmendatory
                {
                    Amendatory = terms + "付款申請書乙份：" + ((true == viewModel.AttachPayingAcceptance) ? "檢附" : "免附"),
                    Original = terms + "付款申請書乙份：" + ((true == oldAttach.匯票付款申請書) ? "檢附" : "免附")
                });
            }
            if (viewModel.AttachInv != oldAttach.統一發票)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = "統一發票：" + (viewModel.AttachInv == true ? "檢附" : "免附"),
                    Original = "統一發票：" + (oldAttach.統一發票 == true ? "檢附" : "免附")
                });
            }
            if (viewModel.AttachEInv != oldAttach.電子發票證明聯)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = "電子發票證明聯：" + (viewModel.AttachEInv == true ? "接受" : "不接受"),
                    Original = "電子發票證明聯：" + (oldAttach.電子發票證明聯 == true ? "接受" : "不接受")
                });
            }

            if (viewModel.AttachAdditional != oldAttach.其他)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = String.Format("檢附其他單據：{0}", viewModel.AttachAdditional),
                    Original = String.Format("檢附其他單據：{0}", oldAttach.其他)
                });
            }
            if (viewModel.Seal != oldSN.原留印鑑相符)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = " 匯票付款申請書上申請人所蓋印鑑應與原留印鑑相符：" + (true == viewModel.Seal ? "是" : "否"),
                    Original = " 匯票付款申請書上申請人所蓋印鑑應與原留印鑑相符：" + (true == oldSN.原留印鑑相符 ? "是" : "否")
                });
            }
            if (viewModel.BeneSeal != oldSN.受益人單獨蓋章)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = " 匯票付款申請書由信用狀受益人單獨蓋章：" + (true == viewModel.BeneSeal ? "是" : "否"),
                    Original = " 匯票付款申請書由信用狀受益人單獨蓋章：" + (true == oldSN.受益人單獨蓋章 ? "是" : "否")
                });
            }
            if (viewModel.Partial != oldSN.分批交貨)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = " 分批交貨：" + (true == viewModel.Partial ? "可" : "不可"),
                    Original = " 分批交貨：" + (true == oldSN.分批交貨 ? "可" : "不可")
                });
            }
            if (viewModel.NoAfterThan_D != oldSN.最後交貨日)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = " 最後交貨日期：" + ValidityAgent.ConvertChineseDate(viewModel.NoAfterThan_D),
                    Original = " 最後交貨日期：" + ValidityAgent.ConvertChineseDate(oldSN.最後交貨日)
                });
            }
            ///////
            if (viewModel.InvoiceDateStart_D != oldSN.押匯發票起始日)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = "押匯發票起始日：" + ValidityAgent.ConvertChineseDate(viewModel.InvoiceDateStart_D),
                    Original = "押匯發票起始日：" + ValidityAgent.ConvertChineseDate(oldSN.押匯發票起始日)
                });

            }

            if (viewModel.DraftDateStart_D != oldSN.押匯起始日)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = "押匯起始日：" + ValidityAgent.ConvertChineseDate(viewModel.DraftDateStart_D),
                    Original = "押匯起始日：" + ValidityAgent.ConvertChineseDate(oldSN.押匯起始日)
                });
            }
            ///////
            if (viewModel.LargerInvAmt != oldSN.接受發票金額大於開狀金額)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = " 統一發票金額大於開狀金額可接受：" + (true == viewModel.LargerInvAmt ? "是" : "否"),
                    Original = " 統一發票金額大於開狀金額可接受。：" + (true == oldSN.接受發票金額大於開狀金額 ? "是" : "否")
                });
            }

            if (viewModel.EarlyInvDate != oldSN.接受發票早於開狀日)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = " 統一發票早於開狀日可接受：" + (true == viewModel.EarlyInvDate ? "是" : "否"),
                    Original = " 統一發票早於開狀日可接受：" + (true == oldSN.接受發票早於開狀日 ? "是" : "否")
                });
            }

            if (viewModel.InvoiceAddr != oldSN.接受發票人地址與受益人地址不符)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = " 統一發票人地址與受益人地址不符可接受：" + (true == viewModel.InvoiceAddr ? "是" : "否"),
                    Original = " 統一發票人地址與受益人地址不符可接受：" + (true == oldSN.接受發票人地址與受益人地址不符 ? "是" : "否")
                });
            }

            if (viewModel.InvoiceProductDetail != oldSN.貨品明細以發票為準)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = " 貨品明細以發票為準：" + (true == viewModel.InvoiceProductDetail ? "是" : "否"),
                    Original = " 貨品明細以發票為準：" + (true == oldSN.貨品明細以發票為準 ? "是" : "否")
                });
            }

            if (viewModel.AcceptEInvoice != oldSN.接受發票電子訊息)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = " 統一發票電子訊息可接受：" + (true == viewModel.AcceptEInvoice ? "是" : "否"),
                    Original = " 統一發票電子訊息可接受：" + (true == oldSN.接受發票電子訊息 ? "是" : "否")
                });
            }

            if (viewModel.LargerInvDraft != oldSN.接受發票金額大於匯票金額)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = " 統一發票金額大於匯票金額可接受：" + (true == viewModel.LargerInvDraft ? "是" : "否"),
                    Original = " 統一發票金額大於匯票金額可接受：" + (true == oldSN.接受發票金額大於匯票金額 ? "是" : "否")
                });
            }

            //if (viewModel.以發票收執聯或扣抵聯正本押匯 != oldSN.以發票收執聯或扣抵聯正本押匯 && viewModel.以發票收執聯或扣抵聯正本押匯.HasValue && oldSN.以發票收執聯或扣抵聯正本押匯.HasValue)
            //{
            //    items.Add(new LcAmendatory
            //    {
            //        Amendatory = " 得以發票收執聯(或扣抵聯)任一聯正本押匯：" + (true == viewModel.以發票收執聯或扣抵聯正本押匯 ? "是" : "否"),
            //        Original = " 得以發票收執聯(或扣抵聯)任一聯正本押匯：" + (true == oldSN.以發票收執聯或扣抵聯正本押匯 ? "是" : "否")
            //    });
            //}

            //if (viewModel.發票影本可接受 != oldSN.發票影本可接受 && viewModel.發票影本可接受.HasValue && oldSN.發票影本可接受.HasValue)
            //{
            //    items.Add(new LcAmendatory
            //    {
            //        Amendatory = " 發票影本可接受(限扣抵聯或收執聯)：" + (true == viewModel.發票影本可接受 ? "是" : "否"),
            //        Original = " 發票影本可接受(限扣抵聯或收執聯)：" + (true == oldSN.發票影本可接受 ? "是" : "否")
            //    });
            //}

            if (viewModel.SNAdditional != oldSN.其他)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = "其他特別指示條款：" + viewModel.SNAdditional,
                    Original = "其他特別指示條款：" + oldSN.其他
                });
            }

            return items;
        }

    }
}
