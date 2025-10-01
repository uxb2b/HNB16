﻿using CommonLib.DataAccess;
using CommonLib.Utility;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ModelCore.DataModel;
using ModelCore.Locale;
using ModelCore.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ModelCore.Schema.ENID
{
    public static class ENIDExtensions
    {
        public static CreditApplicationDocumentary CommitCreditApplication(this CDSLcdApplication lcApp, ModelStateDictionary modelState, bool overTheCounter = false)
        {
            var appNo = lcApp.lcAppInfo?.appNo;
            DateTime appDate;
            if (!DateTime.TryParse(lcApp.lcAppInfo?.appDate, out appDate))
            {
                appDate = DateTime.Now;
            }

            if (string.IsNullOrEmpty(appNo))
            {
                modelState.AddModelError("AppNo", "開狀申請書號碼不可為空白");
                return null;
            }

            var lcDoc = lcApp.lcDoc;
            var lcData = lcApp.lcData;

            return CheckCreditApplication(appNo, appDate, lcDoc, lcData, overTheCounter, modelState);
        }

        private static CreditApplicationDocumentary CheckCreditApplication(string appNo, DateTime appDate, LcdType lcDoc, LcdDataType lcData, bool overTheCounter, ModelStateDictionary modelState)
        {
            using GenericManager<LcEntityDataContext> models = new GenericManager<LcEntityDataContext>();

            // 受益人
            var beneID = CheckBeneficiary(lcDoc?.beneficiary, modelState);
            BeneficiaryData bene = models.GetTable<BeneficiaryData>()
                .Where(b => b.BeneID == beneID)
                .FirstOrDefault();
            if (bene == null)
            {
                modelState.AddModelError("company", "信用狀受益人資料錯誤");
                return null;
            }
            // 申請人
            var issuingBank = lcDoc?.issuingBank?.branchCode;
            var applicantID = CheckApplicant(lcDoc?.applicant, issuingBank, modelState);
            CustomerOfBranch cust = models.GetTable<CustomerOfBranch>()
                            .Where(b => b.BankCode == issuingBank
                                && b.CompanyID == applicantID)
                            .FirstOrDefault();

            CreditApplicationDocumentary item = models.GetTable<CreditApplicationDocumentary>()
                .Where(d => d.ApplicationNo == appNo).FirstOrDefault();

            if (item == null)
            {
                DateTime now = DateTime.Now;
                item = new CreditApplicationDocumentary
                {
                    Documentary = new Documentary
                    {
                        CurrentLevel = (int)Naming.DocumentLevel.文件預覽,
                        DocDate = now,
                        DocType = (int)Naming.DocumentTypeDefinition.開狀申請書,
                        SysDocID = "LLC",
                    },
                    LcItem = new LcItem
                    {
                    },
                    SpecificNote = new SpecificNote
                    {
                    },
                    AttachableDocument = new AttachableDocument { },
                    // 開狀申請日期 = now, // 移除不存在屬性
                };

                // 其他初始化依需求補充
                models.GetTable<CreditApplicationDocumentary>().InsertOnSubmit(item);
            }

            BankData advisingBank = null;

            void checkInput()
            {
                if (cust == null)
                {
                    modelState.AddModelError("Applicant", "信用狀開立申請人資料錯誤");
                }

                if (bene == null)
                {
                    modelState.AddModelError("Beneficiary", "信用狀受益人資料錯誤");
                }

                advisingBank = models.GetTable<BankData>()
                        .Where(b => b.BankCode == lcDoc.advisingBank.branchCode)
                        .FirstOrDefault();

                if (advisingBank == null)
                {
                    modelState.AddModelError("AdvisingBank", "通知銀行不存在");
                }
                else
                {
                    var DisabledBranch = models.GetTable<DisabledBranch>()
                            .Where(b => b.BankCode == lcDoc.advisingBank.branchCode)
                            .FirstOrDefault();
                    if (DisabledBranch != null)
                    {
                        modelState.AddModelError("AdvisingBank", "通知行已裁撤，請再重新選取!");
                    }
                }

                DateTime dateValue;
                // 付款方式判斷
                item.LcItem.PaymentTerms = $"{lcData?.payment?.term}";
                switch (lcData?.payment?.term)
                {
                    case LcdDataTypePaymentTerm.atSight:
                        item.見票即付 = true;
                        item.定日付款 = 0;
                        break;

                    case LcdDataTypePaymentTerm.atDaysAfterDate:
                    case LcdDataTypePaymentTerm.atDaysAfterSight:
                    case LcdDataTypePaymentTerm.atDaysAfterBLDate:
                        int usanceDay = 0;
                        item.見票即付 = false;
                        item.定日付款 = int.TryParse(lcData?.payment?.days, out usanceDay) ? usanceDay : 0;
                        break;

                    case LcdDataTypePaymentTerm.onDate:
                        item.定日付款 = 0;
                        item.見票即付 = false;
                        if (DateTime.TryParse(lcData.payment.date, out dateValue))
                        {
                            item.LcItem.PaymentDate = dateValue;
                        }
                        else
                        {
                            modelState.AddModelError("PaymentTerm", "請選擇有效定期付款");
                        }
                        break;
                    default:
                        modelState.AddModelError("PaymentTerm", "請選擇有效的付款方式");
                        break;
                }

                item.ApplicationDate = appDate;

                // 其他檢查...
            }

            checkInput();

            if (!modelState.IsValid)
            {
                return null;
            }

            // 設定主要欄位
            item.ApplicationNo = appNo;
            item.付款行 = lcDoc?.payingBank?.branchCode;
            item.開狀行 = cust.BankCode;
            item.申請人 = cust.CompanyID;
            item.受益人 = bene.BeneID;
            item.通知行 = advisingBank.BankCode;
            item.沖銷保證金方式 = null; // 依需求補充
            item.Instrunction = "非本行制式特別指示之申請原因及依據：";
            item.OverTheCounter = overTheCounter;
            item.VersionID = cust.Organization.OrganizationExtension?.CurrentVersion;
            item.BeneDetailID = bene.Organization.OrganizationExtension?.CurrentVersion;

                // 這裡可根據實際 XML 結構取得貨物名稱
            FromConditions(item, null, null, lcDoc, modelState);
            // 分批交貨
            item.SpecificNote.分批交貨 = lcData?.isPartialShipment == LcdDataTypeIsPartialShipment.@true; ;
            item.SpecificNote.IsCSCTerms = lcData?.isCSCConditions == LcdDataTypeIsCSCConditions.@true;
            item.SpecificNote.CSCSalesDept = lcData?.CSCSalesDept;

            if (!modelState.IsValid)
            {
                return null;
            }

            models.SubmitChanges();
            return item;
        }

        private static int CheckBeneficiary(CompanyType company, ModelStateDictionary modelState)
        {
            using GenericManager<LcEntityDataContext> models = new GenericManager<LcEntityDataContext>();
            if (company == null)
            {
                modelState.AddModelError("Beneficiary", "受益人資料不可為空白");
                return -1;
            }

            string beneficiaryNo = company?.regNo;
            if (string.IsNullOrEmpty(beneficiaryNo))
            {
                modelState.AddModelError("Beneficiary", "受益人統一編號不可為空白");
                return -1;
            }
            // 檢查受益人是否已存在
            var companyID = CheckOrganization(company);
            Organization org = models.GetTable<Organization>()
                        .Where(o => o.CompanyID == companyID)
                        .FirstOrDefault();
            if (org == null)
            {
                modelState.AddModelError("Beneficiary", "受益人資料不存在");
                return -1;
            }
            // 檢查受益人是否有 BeneficiaryData
            var bene = org.BeneficiaryData;

            if (bene == null)
            {
                bene = org.BeneficiaryData = new BeneficiaryData
                {
                };
                models.GetTable<BeneficiaryData>().InsertOnSubmit(bene);
                models.SubmitChanges();
            }

            return bene.BeneID;
        }

        private static int CheckApplicant(CompanyType company,String bankCode, ModelStateDictionary modelState)
        {
            using GenericManager<LcEntityDataContext> models = new GenericManager<LcEntityDataContext>();
            if (company == null)
            {
                modelState.AddModelError("Applicant", "開狀申請人資料不可為空白");
                return -1;
            }
            string applicantNo = company?.regNo;
            if (string.IsNullOrEmpty(applicantNo))
            {
                modelState.AddModelError("Applicant", "開狀申請人統一編號不可為空白");
                return -1;
            }
            // 檢查開狀申請人是否已存在
            var companyID = CheckOrganization(company);
            Organization org = models.GetTable<Organization>()
                        .Where(o => o.CompanyID == companyID)
                        .FirstOrDefault();
            if (org == null)
            {
                modelState.AddModelError("Applicant", "開狀申請人資料不存在");
                return -1;
            }
            // 檢查開狀申請人是否有 CustomerOfBranch
            var cust = org.CustomerOfBranch.Where(c => c.BankCode == bankCode).FirstOrDefault();
            if (cust == null)
            {
                cust = new CustomerOfBranch
                {
                    BankCode = bankCode,
                    CompanyID = companyID,
                };
                models.GetTable<CustomerOfBranch>().InsertOnSubmit(cust);
                models.SubmitChanges();
            }
            return cust.CompanyID;
        }

        private static int CheckOrganization(CompanyType company)
        {
            using GenericManager<LcEntityDataContext> models = new GenericManager<LcEntityDataContext>();
            var receiptNo = company?.regNo;
            var org = models.GetTable<Organization>()
                        .Where(o => o.ReceiptNo == receiptNo)
                        .FirstOrDefault();

            if (org == null)
            {
                org = new Organization
                {
                    CompanyID = models.GetTable<Organization>().Select(o => o.CompanyID).Max() + 1,
                    ReceiptNo = receiptNo,
                    CompanyName = company.name,
                    Addr = company.address,
                    Phone = company.phone,
                    ContactEmail = company.chiefEmail,
                    UndertakerName = company.chiefName,
                    OrganizationExtension = new OrganizationExtension
                    {
                    },
                    OrganizationStatus = new OrganizationStatus
                    {
                    },
                };
                models.GetTable<Organization>().InsertOnSubmit(org);
                org.OrganizationExtension.CustomerOfBranchVersion = new CustomerOfBranchVersion
                {
                    CompanyName = org.CompanyName,
                    Addr = org.Addr,
                    Phone = org.Phone,
                    ContactEmail = org.ContactEmail,
                    UndertakerName = org.UndertakerName,
                };
                models.SubmitChanges();
            }

            return org.CompanyID;
        }

        private static void FromConditions(CreditApplicationDocumentary appItem, AmendingLcApplication amending, LetterOfCreditVersion versionItem, LcdType lcDoc, ModelStateDictionary modelState)
        {
            SpecificNote noteItem = appItem?.SpecificNote ?? amending?.SpecificNote ?? versionItem?.SpecificNote;
            LcItem lcItem = appItem?.LcItem ?? amending?.LcItem ?? versionItem?.LcItem;
            AttachableDocument attachableDocument = appItem?.AttachableDocument ?? amending?.AttachableDocument ?? versionItem?.AttachableDocument;
            DateTime dateValue;

            if (DateTime.TryParse(lcDoc?.expiryDate?.Value, out dateValue))
            {
                lcItem.有效期限 = dateValue;
            }
            else
            {
                modelState.AddModelError("LcExpiry", "請選擇有效期限");
            }

            if (decimal.TryParse(lcDoc?.amount?.value, out decimal lcAmt) && lcAmt > 0)
            {
                lcItem.開狀金額 = lcAmt;
            }
            else
            {
                modelState.AddModelError("LcAmt", "請輸入開狀金額");
            }

            if (int.TryParse(lcDoc?.currency?.code, out int currency))
            {
                lcItem.幣別 = currency;
            }
            else
            {
                modelState.AddModelError("Currency", "請選擇開狀幣別");
            }


            IEnumerable<conditionsCondition> conditions = lcDoc?.conditions?.Where(c => c.used == conditionsConditionUsed.@true);
            if (conditions == null || !conditions.Any())
            {
                modelState.AddModelError("Conditions", "信用狀條件不可為空白");
                return;
            }

            foreach (var condition in conditions)
            {
                switch (condition.name)
                {
                    case "PaymentTerm":
                        if(appItem!=null)
                        {
                            appItem.見票即付 = condition.singleChoice.item.Any(i => i.name == "Sight" && i.@checked == ItemTypeChecked.@true);
                        }
                        break;

                    case "AtDaysAfterDate":
                        var blank = condition.textBlank?.blank.Where(b => b.name == "AtDays").FirstOrDefault();
                        if (blank != null)
                        {
                            int days = 0;
                            lcItem.定日付款 = days;
                            if (appItem != null)
                            {
                                appItem.定日付款 = days;
                                appItem.見票即付 = days == 0; // 如果有定日付款則見票即付為 false
                            }
                        }
                        break;

                    case "DueDateSpecified":
                        blank = condition.textBlank?.blank.Where(b => b.name == "DueDateSpecified").FirstOrDefault();
                        if (blank != null)
                        {
                            if (DateTime.TryParse(blank.Value, out dateValue))
                            {
                                lcItem.PaymentDate = dateValue;
                                lcItem.定日付款 = 0;
                                if (appItem != null)
                                {
                                    appItem.定日付款 = 0;
                                    appItem.見票即付 = false;
                                }
                            }
                            else if (string.IsNullOrEmpty(blank.Value))
                            {
                                modelState.AddModelError("DueDateSpecified", "請輸入有效的日期");
                            }

                        }
                        break;

                    case "NonCSNegotiationSpecialClause":
                        noteItem.NonCSCTerms = condition.textArea?.area?.FirstOrDefault()?.Value;
                        break;

                    case "GoodsName":
                        lcItem.Goods = condition.textArea?.area?.FirstOrDefault()?.Value;
                        if (string.IsNullOrEmpty(lcItem.Goods))
                        {
                            modelState.AddModelError("GoodsName", "請輸入貨物名稱");
                        }
                        break;
                    case "LastDateOfDelivery":
                        DateTime noAfterThan = DateTime.Today.AddMonths(3); // 預設為 3 個月後
                        if (DateTime.TryParse(condition.textBlank?.blank?.FirstOrDefault()?.Value, out dateValue))
                        {
                            noAfterThan = dateValue;
                        }
                        noteItem.最後交貨日 = noAfterThan;
                        break;
                    case "NegotiateDate":
                        if (DateTime.TryParse(condition.textBlank?.blank?.FirstOrDefault()?.Value, out dateValue))
                        {
                            noteItem.押匯起始日 = dateValue;
                        }
                        break;
                    case "DraftPaymentOrAcceptanceApplication":
                        if (condition.singleChoice?.item != null)
                        {
                            attachableDocument.匯票付款申請書 = condition.singleChoice.item.Any(i => i.name == "Payment" && i.@checked == ItemTypeChecked.@true);
                            attachableDocument.匯票承兌申請書 = condition.singleChoice.item.Any(i => i.name == "Acceptance" && i.@checked == ItemTypeChecked.@true);
                        }
                        break;
                    case "Invoice":
                        attachableDocument.統一發票 = condition.textOnly != null;
                        break;
                    case "OtherAppendix":
                        attachableDocument.其他 = condition.textArea?.area?.FirstOrDefault()?.Value.GetEfficientString();
                        break;
                    case "IsUsanceLcInterestPayByBuyer":
                        noteItem.IsUsanceLcInterestPayByBuyer = condition.singleChoice?.item?.Any(i => i.name == "Yes" && i.@checked == ItemTypeChecked.@true) == true;
                        break;
                    case "IsAcceptanceChargePayByBuyer":
                        noteItem.IsAcceptanceChargePayByBuyer = condition.singleChoice?.item?.Any(i => i.name == "Yes" && i.@checked == ItemTypeChecked.@true) == true;
                        break;
                    case "IsBatchDeliverGoods":
                        noteItem.分批交貨 = condition.singleChoice?.item?.Any(i => i.name == "Yes" && i.@checked == ItemTypeChecked.@true) == true;
                        break;
                    case "OtherSpecialClause":
                        noteItem.其他 = condition.textArea?.area?.FirstOrDefault()?.Value;
                        break;
                    case "SpecialMessageForCS":
                        noteItem.SpecialMessageForCS = condition.singleChoice?.item?.FirstOrDefault(i => i.@checked == ItemTypeChecked.@true)?.Value;
                        break;
                }
            }
        }

        public static LetterOfCredit CommitLC(this CDSLcd lcInstance, ModelStateDictionary modelState, int version = 0)
        {
            using GenericManager<LcEntityDataContext> models = new GenericManager<LcEntityDataContext>();

            // 1. 取得對應的 CreditApplicationDocumentary
            var appNo = lcInstance.lcInfo?.appNo;
            if (string.IsNullOrEmpty(appNo))
            {
                modelState.AddModelError("AppNo", "LC Application No 不可為空白");
                return null;
            }

            if (string.IsNullOrEmpty(lcInstance.lcDoc?.lcNo))
            {
                modelState.AddModelError("LcNo", "LC No 不可為空白");
                return null;
            }

            DateTime issuingDate;
            if (!DateTime.TryParse(lcInstance.lcDoc?.issuingDate, out issuingDate))
            {
                modelState.AddModelError("issuingDate", "請選擇有效開狀日期");
                return null;
            }

            var lcDoc = lcInstance.lcDoc;
            var lcData = lcInstance.lcData;

            var creditApp = models.GetTable<CreditApplicationDocumentary>()
                .Where(d => d.ApplicationNo == appNo)
                .FirstOrDefault();

            if (creditApp == null)
            {
                creditApp = CheckCreditApplication(appNo, issuingDate, lcDoc, lcData, false, modelState);
                if (creditApp != null)
                {
                    creditApp = models.GetTable<CreditApplicationDocumentary>()
                        .Where(d => d.AppID == creditApp.AppID)
                        .First();
                    creditApp.Documentary.CurrentLevel = (int)Naming.DocumentLevel.已開立;
                    models.SubmitChanges();
                }
            }

            if (creditApp == null)
            {
                modelState.AddModelError("CreditApplicationDocumentary", $"找不到對應的開狀申請書: {appNo}");
                return null;
            }


            // 2. 檢查是否已存在 LetterOfCredit
            var letterOfCredit = models.GetTable<LetterOfCredit>()
                .Where(lc => lc.CreditApplicationDocumentary.ApplicationNo == appNo)
                .FirstOrDefault();
            if (letterOfCredit == null)
            {
                letterOfCredit = new LetterOfCredit();
                // 關聯 CreditApplicationDocumentary
                letterOfCredit.CreditApplicationDocumentary = creditApp;
                models.GetTable<LetterOfCredit>().InsertOnSubmit(letterOfCredit);
            }

            var versionItem = letterOfCredit.LetterOfCreditVersion
                .Where(v => v.VersionNo == version)
                .FirstOrDefault();

            if (versionItem == null)
            {
                versionItem = new LetterOfCreditVersion
                {
                    VersionNo = version,
                };
                letterOfCredit.LetterOfCreditVersion.Add(versionItem);
            }

            // 3. 將 CDSLcd 相關資料填入 LetterOfCredit
            letterOfCredit.NotifyingBank = versionItem.NotifyingBank = creditApp.通知行;
            letterOfCredit.LcDate = issuingDate;
            // LC號碼
            letterOfCredit.LcNo = lcInstance.lcDoc?.lcNo;
            letterOfCredit.可用餘額 = creditApp.LcItem?.開狀金額;
            letterOfCredit.AppCountersign = creditApp.BeneficiaryData.AppCountersign == true;
            if (version == 0)
            {
                versionItem.ItemID = creditApp.ItemID;
                versionItem.AttachmentID = creditApp.AttachmentID;
                versionItem.NoteID = creditApp.NoteID;
            }
            else
            {
                versionItem.LcItem = new LcItem
                {
                };
                versionItem.SpecificNote = new SpecificNote
                {
                };
                versionItem.AttachableDocument = new AttachableDocument
                {
                };

                // 這裡可根據實際 XML 結構取得貨物名稱
                FromConditions(null, null, versionItem, lcDoc, modelState);
            }


            // 其他欄位依需求補充

            // 4. 儲存
            if (!modelState.IsValid)
                return null;

            models.SubmitChanges();
            return letterOfCredit;
        }

        public static AmendingLcApplication CommitLcAmendment(this CDSLcdAmendment lcAmendment, ModelStateDictionary modelState, Naming.DocumentLevel? level = null)
        {
            using GenericManager<LcEntityDataContext> models = new GenericManager<LcEntityDataContext>();
            var appNo = lcAmendment.lcAmendInfo?.appNo;
            if (string.IsNullOrEmpty(appNo))
            {
                modelState.AddModelError("AppNo", "開狀申請書號碼不可為空白");
                return null;
            }

            var lcNo = lcAmendment.lcDocNew?.lcNo;
            if (string.IsNullOrEmpty(lcNo))
            {
                modelState.AddModelError("LcNo", "信用狀號碼不可為空白");
                return null;
            }

            LetterOfCredit lc = models.GetTable<LetterOfCredit>()
                .Where(lc => lc.CreditApplicationDocumentary.ApplicationNo == appNo)
                .Where(lc => lc.LcNo == lcNo)
                .FirstOrDefault();
            if (lc == null)
            {
                modelState.AddModelError("LetterOfCredit", $"找不到對應的信用狀: {lcNo} for {appNo}");
                return null;
            }

            var lcVersion = new LetterOfCreditVersion
            {
                VersionNo = lc.LetterOfCreditVersion.Min(v => v.VersionNo) - 1,
                LetterOfCredit = lc,
                LcItem = new LcItem
                {
                },
                SpecificNote = new SpecificNote
                {
                },
                AttachableDocument = new AttachableDocument
                {
                },
            };
            lc.LetterOfCreditVersion.Add(lcVersion);
            // 這裡可根據實際 XML 結構取得貨物名稱
            FromConditions(null, null, lcVersion, lcAmendment.lcDocOld, modelState);

            //var lcVersion = models.GetTable<LetterOfCreditVersion>()
            //        .Where(v => v.LetterOfCredit.LcNo == lcNo)
            //        .Where(v => v.LetterOfCredit.CreditApplicationDocumentary.ApplicationNo == appNo)
            //    .FirstOrDefault();

            //if (lcVersion == null)
            //{
            //    modelState.AddModelError("LcVersion", $"找不到對應的信用狀版本: {lcNo} for {appNo}");
            //    return null;
            //}
            //else if (lcVersion.IsAmending)
            //{
            //    modelState.AddModelError("LcVersion", $"信用狀 {lcNo} 已經在修狀中，請勿重複提交");
            //    return null;
            //}

            var amendmentNo = lcAmendment.lcAmendInfo?.amendNo;
            if (string.IsNullOrEmpty(amendmentNo))
            {
                modelState.AddModelError("amendNo", "修狀申請書號碼不可為空白");
                return null;
            }

            var amendingApp = models.GetTable<AmendingLcApplication>()
                .Where(a => a.AmendmentNo == amendmentNo)
                .FirstOrDefault();
            if (amendingApp == null)
            {
                amendingApp = new AmendingLcApplication
                {
                    AmendmentNo = amendmentNo,
                    Documentary = new Documentary
                    {
                        DocType = (int)Naming.DocumentTypeDefinition.修狀申請書,
                        CurrentLevel = (int)(level ?? Naming.DocumentLevel.文件預覽),
                        DocDate = DateTime.Now,
                    },
                    LetterOfCreditVersion = lcVersion,
                    LcItem = new LcItem { },
                    SpecificNote = new SpecificNote { },
                    AttachableDocument = new AttachableDocument { },
                };
                models.GetTable<AmendingLcApplication>().InsertOnSubmit(amendingApp);
            }
            else if (amendingApp.IsAmending)
            {
                modelState.AddModelError("AmendingLcApplication", $"信用狀 {lcNo} 已經在修狀中，請勿重複提交");
                return null;
            }

            amendingApp.ApplicationDate = DateTime.Now;
            // 填入修正資訊
            FromConditions(null, amendingApp, null, lcAmendment.lcDocNew, modelState);
            amendingApp.SpecificNote.分批交貨 = lcAmendment.lcData?.isPartialShipment == LcdDataTypeIsPartialShipment.@true; ;
            amendingApp.SpecificNote.IsCSCTerms = lcAmendment.lcData?.isCSCConditions == LcdDataTypeIsCSCConditions.@true;
            amendingApp.SpecificNote.CSCSalesDept = lcAmendment.lcData?.CSCSalesDept;
            // 其他欄位依需求補充
            // 儲存
            if (!modelState.IsValid)
                return null;
            models.SubmitChanges();
            // 更新信用狀版本為修狀中
            return amendingApp;
        }

        public static AmendingLcInformation CommitAmendmentNotification(this CDSLcdAdvice lcAdvice, ModelStateDictionary modelState)
        {
            using GenericManager<LcEntityDataContext> models = new GenericManager<LcEntityDataContext>();
            var lcNo = lcAdvice.lcDocOld?.lcNo;
            if (string.IsNullOrEmpty(lcNo))
            {
                modelState.AddModelError("LcNo", "信用狀號碼不可為空白");
                return null;
            }
            var amendmentNo = lcAdvice.lcAdviceInfo?.amendNo;
            var amendingApp = models.GetTable<AmendingLcApplication>()
                .Where(a => a.AmendmentNo == amendmentNo)
                .Where(a => a.LetterOfCreditVersion.LetterOfCredit.LcNo == lcNo)
                .FirstOrDefault();
            if (amendingApp == null)
            {
                modelState.AddModelError("AmendingLcApplication", $"找不到對應的修狀申請書: {amendmentNo} for {lcNo}");
                return null;
            }
            var currentLc = amendingApp.LetterOfCreditVersion;
            LetterOfCreditVersion lcVersion = null;
            var noticeItem = amendingApp.AmendingLcInformation;
            if (noticeItem != null)
            {
                modelState.AddModelError("AmendingLcInformation", "修狀通知書已存在，請勿重複提交");
                return null;
            }
            else
            {
                // 如果不存在，則建立新的修狀通知書
                noticeItem = new AmendingLcInformation
                {
                    InformationNo = lcAdvice.lcAdviceInfo?.advNo,
                };

                amendingApp.AmendingLcInformation = noticeItem;
                lcVersion = currentLc.LetterOfCredit.LetterOfCreditVersion
                    .Where(v => v.VersionNo > 0)
                    .Where(v => !v.AmendingID.HasValue)
                    .OrderBy(v => v.VersionID)
                    .FirstOrDefault();

                if (lcVersion == null)
                {
                    lcVersion = new LetterOfCreditVersion
                    {
                        VersionNo = currentLc.LetterOfCredit.LetterOfCreditVersion.Max(v => v.VersionNo) + 1,
                        LetterOfCredit = currentLc.LetterOfCredit,
                        AmendingLcInformation = noticeItem,
                        NotifyingBank = currentLc.LetterOfCredit.NotifyingBank,
                    };
                }
                else
                {
                    lcVersion.AmendingLcInformation = noticeItem;
                }

            }

            if (DateTime.TryParse(lcAdvice.lcAdviceInfo?.advDate, out DateTime dateValue))
            {
                noticeItem.AmendingDate = dateValue;
            }
            else
            {
                noticeItem.AmendingDate = DateTime.Now;
            }
            // 關聯到修狀申請書
            lcVersion.AttachmentID = amendingApp.AttachmentID;
            lcVersion.ItemID = amendingApp.ItemID;
            lcVersion.NoteID = amendingApp.NoteID;

            // 其他欄位依需求補充
            // 儲存
            if (!modelState.IsValid)
                return null;
            models.SubmitChanges();
            return noticeItem;
        }

        public static CreditCancellation CommitLcCancellation(this CDSLcdCancellation lcCancel, ModelStateDictionary modelState)
        {
            using GenericManager<LcEntityDataContext> models = new GenericManager<LcEntityDataContext>();

            var lcNo = lcCancel.lcDoc?.lcNo;
            if (string.IsNullOrEmpty(lcNo))
            {
                modelState.AddModelError("LcNo", "信用狀號碼不可為空白");
                return null;
            }

            var appNo = lcCancel.lcCancInfo?.appNo;
            var lc = models.GetTable<LetterOfCredit>()
                .Where(lc => lc.CreditApplicationDocumentary.ApplicationNo == appNo)
                .Where(lc => lc.LcNo == lcNo)
                .FirstOrDefault();

            if (lc == null)
            {
                modelState.AddModelError("LetterOfCredit", $"找不到對應的信用狀: {lcNo} for {appNo}");
                return null;
            }
            // 1. 檢查是否已存在撤銷申請書
            if (lc.CreditCancellation.Where(c => c.CreditCancellationInfo != null).Any())
            {
                modelState.AddModelError("CreditCancellation", $"信用狀 {lcNo} 已經撤銷，請勿重複提交");
                return null;
            }

            var cancellationNo = lcCancel.lcCancInfo?.cancNo;
            if (string.IsNullOrEmpty(cancellationNo))
            {
                modelState.AddModelError("cancellationNo", "撤銷申請書號碼不可為空白");
                return null;
            }

            var cancellingApp = lc.CreditCancellation.Where(c => c.註銷申請號碼 == cancellationNo)
                .FirstOrDefault();

            if (cancellingApp != null)
            {
                modelState.AddModelError("cancellationNo", $"撤銷申請書已存在: {cancellationNo} for {lcNo}");
                return null;
            }

            cancellingApp = new CreditCancellation
            {
                Documentary = new Documentary
                {
                    CurrentLevel = (int)Naming.DocumentLevel.文件預覽,
                    DocDate = DateTime.Now,
                    DocType = (int)Naming.DocumentTypeDefinition.信用狀註銷申請書,
                },
                LcID = lc.LcID,
            };
            models.GetTable<CreditCancellation>().InsertOnSubmit(cancellingApp);

            cancellingApp.註銷申請號碼 = cancellationNo;
            if (DateTime.TryParse(lcCancel.lcCancInfo?.cancDate, out DateTime dateValue))
            {
                cancellingApp.申請日期 = dateValue;
            }
            else
            {
                cancellingApp.申請日期 = DateTime.Now;
            }

            // 其他欄位依需求補充
            // 儲存
            if (!modelState.IsValid)
                return null;
            models.SubmitChanges();
            return cancellingApp;
        }

        public static CreditCancellationInfo CancelLc(this String cancellationNo,DateTime cancellationDate, ModelStateDictionary modelState)
        {
            using GenericManager<LcEntityDataContext> models = new GenericManager<LcEntityDataContext>();

            var cancellingApp = models.GetTable<CreditCancellation>()
                .Where(c => c.註銷申請號碼 == cancellationNo)
                .FirstOrDefault();

            if (cancellingApp == null)
            {
                modelState.AddModelError("cancellationNo", $"撤銷申請書不存在: {cancellationNo}");
                return null;
            }

            if (cancellingApp.CreditCancellationInfo != null)
            {
                modelState.AddModelError("cancellationNo", $"信用狀已經註銷: {cancellationNo} for {cancellingApp.LetterOfCredit.LcNo}");
                return null;
            }

            cancellingApp.Documentary.CurrentLevel = (int)Naming.DocumentLevel.已註銷;

            cancellingApp.CreditCancellationInfo = new CreditCancellationInfo
            {
                CancellationDate = cancellationDate,
            };

            // 其他欄位依需求補充
            // 儲存
            if (!modelState.IsValid)
                return null;
            models.SubmitChanges();
            return cancellingApp.CreditCancellationInfo;
        }

    }
}
