using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;

using CommonLib.DataAccess;
using EAI.Service;
using EAI.Service.Transaction;
using ModelCore.DataModel;
using ModelCore.EventMessageApp;
using ModelCore.Helper;
using ModelCore.Locale;
using ModelCore.Properties;
using ModelCore.UserManagement;
using CommonLib.Utility;
using CommonLib.Security.UseCrypto;

namespace ModelCore.Service.FPG
{
    public static partial class FpgHelper
    {
        public static ModelCore.Schema.FPG.J2SPITEMTXD2SB29 BuildTXD2SB29(this CreditApplicationDocumentary item)
        {
            return new ModelCore.Schema.FPG.J2SPITEMTXD2SB29
            {
                AMT = item.LcItem.開狀金額.Value,
                AMTSpecified = true,
                ASSNPAYCKEDDAT = item.LcItem.PaymentDate.HasValue ? String.Format("{0}{1:MMdd}", item.LcItem.PaymentDate.Value.Year - 1911, item.LcItem.PaymentDate.Value) : null,
                ASSNEPRDAT = item.SpecificNote.押匯起始日.HasValue ? String.Format("{0:yyyyMMdd}", item.SpecificNote.押匯起始日) : null,
                BENFADR = item.BeneDetails.Addr,
                BENFCOCNM = item.BeneDetails.CompanyName,
                BENFDIV = item.FpgLcItem.DepartID.Replace(GroupDepartment.DefaultDepartID, ""),
                BENFRFNO = item.BeneficiaryData.Organization.ReceiptNo,
                BKCHKCOMT = "",
                BKCHKTM = String.Format("{0:yyyyMMddHHmmss}", item.Documentary.DocumentaryAllowance.OrderByDescending(a => a.ApprovalDate).First().ApprovalDate),
                CTMNM = item.FpgLcItem.ContactName,
                CTMTEL = item.FpgLcItem.ContactPhone,
                CUAPLCOMT = item.SpecificNote.其他,
                CUCOCNM = item.ApplicantDetails.CompanyName,
                CUCY = item.LcItem.currency_type.apprev_name,
                CUNO = item.FpgLcItem.CustomerNo,
                DLEDDAT = String.Format("{0:yyyyMMdd}", item.SpecificNote.最後交貨日),
                DSRCONT = item.LcItem.GetGoodsDescription(),
                EFFDDAT = String.Format("{0}{1:MMdd}", item.LcItem.有效期限.Value.Year - 1911, item.LcItem.有效期限.Value),
                EIVMK = item.SpecificNote.接受發票電子訊息 == true ? "Y" : "N",
                EPRDIFRR = item.FpgLcItem.押匯允差比例.HasValue ? item.FpgLcItem.押匯允差比例.Value : 0,
                EPRDIFRRSpecified = true,
                IVADRMK = item.SpecificNote.接受發票人地址與受益人地址不符 == true ? "Y" : "N",
                IVAMTMK = item.SpecificNote.接受發票金額大於開狀金額 == true ? "Y" : "N",
                IVDATMK = item.SpecificNote.接受發票早於開狀日 == true ? "Y" : "N",
                IVFROPNDAT = item.SpecificNote.接受發票早於開狀日 == true && item.SpecificNote.押匯發票起始日.HasValue
                            ? String.Format("{0:yyyyMMdd}", item.SpecificNote.押匯發票起始日)
                            : ""    /*String.Format("{0:yyyyMMdd}", item.OpeningApplicationDocumentary.開狀日期)*/,
                LCAPLTM = String.Format("{0:yyyyMMddHHmmss}", item.開狀申請日期),
                LCBK = "009" + item.開狀行,
                LCBKACID = "009" + item.通知行,
                LCNO = item.LetterOfCredit[0].LcNo,
                LCPESRFNO = item.BeneficiaryData.CustomerOfBranch.Organization.ReceiptNo,
                LCSQNO = "01",
                PAMK = item.SpecificNote.分批交貨 == true ? "Y" : "N",
                PAYCKFRSTD = "",
                PAYCKMK = item.SpecificNote.受益人單獨蓋章 == true ? "Y" : "N",
                PAYDYS = item.定日付款 > 0 && !item.見票即付 ? item.定日付款.ToString() : null,
                PAYTY = item.見票即付 ? "A" : "B",
                TRNTM = String.Format("{0:yyyyMMddHHmmss}", DateTime.Now)
            };
        }

        public static ModelCore.Schema.FPG.J2SPITEMTXD2SB29 BuildTXD2SB29(this AmendingLcApplication item)
        {
            return new ModelCore.Schema.FPG.J2SPITEMTXD2SB29
            {
                AMT = item.LcItem.開狀金額.Value,
                AMTSpecified = true,
                ASSNPAYCKEDDAT = item.LcItem.PaymentDate.HasValue ? String.Format("{0}{1:MMdd}", item.LcItem.PaymentDate.Value.Year - 1911, item.LcItem.PaymentDate.Value) : null,
                ASSNEPRDAT = item.SpecificNote.押匯起始日.HasValue ? String.Format("{0:yyyyMMdd}", item.SpecificNote.押匯起始日) : null,
                BENFADR = item.LetterOfCredit.CreditApplicationDocumentary.BeneDetails.Addr,
                BENFCOCNM = item.LetterOfCredit.CreditApplicationDocumentary.BeneDetails.CompanyName,
                BENFDIV = item.LetterOfCredit.CreditApplicationDocumentary.FpgLcItem.DepartID.Replace(GroupDepartment.DefaultDepartID, ""),
                BENFRFNO = item.LetterOfCredit.CreditApplicationDocumentary.BeneficiaryData.Organization.ReceiptNo,
                BKCHKCOMT = "",
                BKCHKTM = String.Format("{0:yyyyMMddHHmmss}", item.Documentary.DocumentaryAllowance.OrderByDescending(a => a.ApprovalDate).First().ApprovalDate),
                CTMNM = item.LetterOfCredit.CreditApplicationDocumentary.FpgLcItem.ContactName,
                CTMTEL = item.LetterOfCredit.CreditApplicationDocumentary.FpgLcItem.ContactPhone,
                CUAPLCOMT = item.SpecificNote.其他,
                CUCOCNM = item.LetterOfCredit.CreditApplicationDocumentary.ApplicantDetails.CompanyName,
                CUCY = item.LcItem.currency_type.apprev_name,
                CUNO = item.LetterOfCredit.CreditApplicationDocumentary.FpgLcItem.CustomerNo,
                DLEDDAT = String.Format("{0:yyyyMMdd}", item.SpecificNote.最後交貨日),
                DSRCONT = item.LcItem.GetGoodsDescription(),
                EFFDDAT = String.Format("{0}{1:MMdd}", item.LcItem.有效期限.Value.Year - 1911, item.LcItem.有效期限.Value),
                EIVMK = item.SpecificNote.接受發票電子訊息 == true ? "Y" : "N",
                EPRDIFRR = item.LetterOfCredit.CreditApplicationDocumentary.FpgLcItem.押匯允差比例.HasValue ? item.LetterOfCredit.CreditApplicationDocumentary.FpgLcItem.押匯允差比例.Value : 0,
                EPRDIFRRSpecified = true,
                IVADRMK = item.SpecificNote.接受發票人地址與受益人地址不符 == true ? "Y" : "N",
                IVAMTMK = item.SpecificNote.接受發票金額大於開狀金額 == true ? "Y" : "N",
                IVDATMK = item.SpecificNote.接受發票早於開狀日 == true ? "Y" : "N",
                IVFROPNDAT = item.SpecificNote.接受發票早於開狀日 == true && item.SpecificNote.押匯發票起始日.HasValue
                            ? String.Format("{0:yyyyMMdd}", item.SpecificNote.押匯發票起始日)
                            : ""    /*String.Format("{0:yyyyMMdd}", item.LetterOfCredit.CreditApplicationDocumentary.OpeningApplicationDocumentary.開狀日期)*/,
                LCAPLTM = String.Format("{0:yyyyMMddHHmmss}", item.LetterOfCredit.CreditApplicationDocumentary.開狀申請日期),
                LCBK = "009" + item.LetterOfCredit.CreditApplicationDocumentary.開狀行,
                LCBKACID = "009" + item.LetterOfCredit.CreditApplicationDocumentary.通知行,
                LCNO = item.LetterOfCredit.LcNo,
                LCPESRFNO = item.LetterOfCredit.CreditApplicationDocumentary.BeneficiaryData.CustomerOfBranch.Organization.ReceiptNo,
                LCSQNO = String.Format("{0:00}", item.AmendingLcInformation.LcVersionNo + 1),
                PAMK = item.SpecificNote.分批交貨 == true ? "Y" : "N",
                PAYCKFRSTD = "",
                PAYCKMK = item.SpecificNote.受益人單獨蓋章 == true ? "Y" : "N",
                PAYDYS = item.LetterOfCredit.CreditApplicationDocumentary.定日付款 > 0 && !item.LetterOfCredit.CreditApplicationDocumentary.見票即付 ? item.LetterOfCredit.CreditApplicationDocumentary.定日付款.ToString() : null,
                PAYTY = item.LetterOfCredit.CreditApplicationDocumentary.見票即付 ? "A" : "B",
                TRNTM = String.Format("{0:yyyyMMddHHmmss}", DateTime.Now)
            };
        }

        public static ModelCore.Schema.FPG.J2SPITEMTXD2SB31 BuildTXD2SB31(this NegoDraft item, String statusCode)
        {
            var txdItem = new ModelCore.Schema.FPG.J2SPITEMTXD2SB31
            {
                SQNO = item.DraftNo,
                STS = statusCode,
                BENFRFNO = item.LetterOfCredit.CreditApplicationDocumentary.BeneficiaryData.Organization.ReceiptNo,
                BKCHKCOMT = "",
                LCBKACID = "009" + item.LetterOfCredit.CreditApplicationDocumentary.通知行,
                LCNO = item.LetterOfCredit.LcNo,
                LCSQNO = String.Format("{0:00}", item.AmendingID.HasValue ? item.AmendingLcInformation.LcVersionNo + 1 : 1),
                TRNTM = String.Format("{0:yyyyMMddHHmmss}", DateTime.Now)
            };

            if (statusCode == "E" && item.NegoDraftExtension.DraftType == (int)Naming.DraftType.CHIMEI)
            {
                txdItem.RENIFEESpecified = true;
                txdItem.RENIFEE = item.FpgNegoDraft.FpgNegoRemittance.FpgNegoRemittanceLog
                                    .Sum(l => l.CountHandlingCharge());
            }

            return txdItem;
        }

        public static ModelCore.Schema.FPG.J2SPITEMTXD2SB38 BuildTXD2SB38(this AmendingLcApplication item)
        {
            return new ModelCore.Schema.FPG.J2SPITEMTXD2SB38
            {
                BKID = "009",
                LCBK = "009" + item.LetterOfCredit.CreditApplicationDocumentary.開狀行,
                LCNO = item.LetterOfCredit.LcNo,
                LCPESRFNO = item.LetterOfCredit.CreditApplicationDocumentary.BeneficiaryData.CustomerOfBranch.Organization.ReceiptNo,
                LCSQNO = String.Format("{0:00}", item.AmendingLcInformation.LcVersionNo.Value + 1),
                TRNTM = String.Format("{0:yyyyMMddHHmmss}", DateTime.Now)
            };
        }

        public static ModelCore.Schema.FPG.J2SPITEMTXD2SB30 BuildTXD2SB30(this NegoDraft item)
        {
            return new ModelCore.Schema.FPG.J2SPITEMTXD2SB30
            {
                LCBKACID = "009" + item.LetterOfCredit.CreditApplicationDocumentary.通知行,
                LCNO = item.LetterOfCredit.LcNo,
                LCSQNO = String.Format("{0:00}", item.AmendingID.HasValue ? item.AmendingLcInformation.LcVersionNo.Value + 1 : 1),
                SQNO = item.DraftNo
            };
        }

        public static void AcceptLcAmendment(this GenericManager<LcEntityDataContext> mgr, ModelCore.Schema.FPG.J2SP j2sp, DataPortLog logItem)
        {
            var processLog = mgr.GetTable<DataProcessLog>();
            List<ModelCore.Schema.FPG.J2SPITEMTXD2SB38> checkItems = new List<ModelCore.Schema.FPG.J2SPITEMTXD2SB38>();

            foreach (var item in j2sp.ITEM.TXD2SB38)
            {
                try
                {
                    LetterOfCredit lcItem;
                    AmendingLcInformation amendmentInfo;
                    if (mgr.FindLc(item.LCNO, item.LCSQNO, out lcItem, out amendmentInfo))
                    {
                        if (amendmentInfo == null)
                        {
                            lcItem.CreditApplicationDocumentary.IsAccepted = true;
                            mgr.SubmitChanges();
                        }
                        else
                        {
                            amendmentInfo.AmendingLcApplication.IsAccepted = true;
                            mgr.SubmitChanges();
                        }
                        checkItems.Add(item);
                    }

                }
                catch (Exception ex)
                {
                    ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                    mgr.LogException(ex, logItem.LogID);
                }
            }

            j2sp.MakeCommitment(logItem, new ModelCore.Schema.FPG.J2SPITEM
            {
                TXD2SB38 = checkItems.ToArray()
            });
        }

        public static ModelCore.Schema.FPG.J2SP MakeCommitment(this ModelCore.Schema.FPG.J2SP j2sp,DataPortLog logItem,ModelCore.Schema.FPG.J2SPITEM commitmentItem)
        {
            ModelCore.Schema.FPG.J2SP commitment = new ModelCore.Schema.FPG.J2SP
            {
                BkId = j2sp.BkId,
                FLAGSpecified = false,
                TABLENAME = j2sp.TABLENAME,
                INITIALFILE = j2sp.FILENAME,
                ITEM = commitmentItem
            };

            commitment.ConvertToXml().Save(Path.Combine(Path.GetDirectoryName(logItem.ContentPath), "R_" + logItem.FpgFileName));
            return commitment;
        }

        public static ModelCore.Schema.FPG.J2SP LoadCommitment(this DataPortLog logItem)
        {
            XmlDocument docMsg = new XmlDocument();
            String path = Path.Combine(Path.GetDirectoryName(logItem.ContentPath), "R_" + logItem.FpgFileName);
            if (File.Exists(path))
            {
                docMsg.Load(path);
                return docMsg.ConvertTo<ModelCore.Schema.FPG.J2SP>();
            }
            return null;
        }


        public static void ConfirmSentLc(this GenericManager<LcEntityDataContext> mgr, ModelCore.Schema.FPG.J2SP j2sp)
        {
            foreach (var item in j2sp.ITEM.TXD2SB29)
            {
                LetterOfCredit lcItem;
                AmendingLcInformation amendmentInfo;
                if (mgr.FindLc(item.LCNO, item.LCSQNO, out lcItem, out amendmentInfo))
                {
                    DocumentDispatch dispatch = amendmentInfo != null ? amendmentInfo.AmendingLcApplication.Documentary.DocumentDispatch
                        : lcItem.CreditApplicationDocumentary.Documentary.DocumentDispatch;

                    if (dispatch != null)
                    {
                        mgr.GetTable<DocumentDispatch>().DeleteOnSubmit(dispatch);
                        mgr.SubmitChanges();
                    }
                }
            }
        }

        public static void ConfirmSentNegoDraftStatus(this GenericManager<LcEntityDataContext> mgr, ModelCore.Schema.FPG.J2SP j2sp)
        {
            foreach (var item in j2sp.ITEM.TXD2SB31)
            {
                LetterOfCredit lcItem;
                AmendingLcInformation amendmentInfo;
                NegoDraft draft;
                if (mgr.FindDraft(item.LCNO, item.LCSQNO,item.SQNO, out lcItem, out amendmentInfo,out draft))
                {
                    DocumentDispatch dispatch = draft.Documentary.DocumentDispatch;

                    if (dispatch != null)
                    {
                        mgr.GetTable<DocumentDispatch>().DeleteOnSubmit(dispatch);
                        mgr.SubmitChanges();
                    }
                }
            }
        }

        public static void ConfirmSentNegoPromptRequest(this GenericManager<LcEntityDataContext> mgr, ModelCore.Schema.FPG.J2SP j2sp)
        {
            var queue = mgr.GetTable<NegoPromptRequestQueue>();
            var item = queue.Where(q => q.DataPortLog.FpgFileName == j2sp.INITIALFILE).FirstOrDefault();
            if (item != null)
            {
                queue.DeleteOnSubmit(item);
                mgr.SubmitChanges();
            }
        }

        public static void PromptDraftNegotiation(this GenericManager<LcEntityDataContext> mgr, ModelCore.Schema.FPG.J2SP j2sp, DataPortLog logItem, BeneficiaryServiceGroup.ServiceDefinition? serviceID = null)
        {
            // TODO: 1. save NegoPrompt
            DateTime importDate = DateTime.Now;

            NegoPrompt prompt = new NegoPrompt 
            {
                LogID = logItem.LogID,
                ImportDate = importDate
            };
            mgr.GetTable<NegoPrompt>().InsertOnSubmit(prompt);
            mgr.SubmitChanges();

            List<ModelCore.Schema.FPG.J2SPITEMTXD2SB30> checkItems = new List<ModelCore.Schema.FPG.J2SPITEMTXD2SB30>();

            // TODO: 2. save NegoDraft
            var draftTable = mgr.GetTable<NegoDraft>();
            for (int idx = 0; idx < j2sp.ITEM.TXD2SB30.Length;idx++)
            {
                var draftItem = j2sp.ITEM.TXD2SB30[idx];
                try
                {
                    LetterOfCredit lcItem;
                    AmendingLcInformation amendmentInfo;

                    if (!mgr.FindLc(draftItem.LCNO, draftItem.LCSQNO, out lcItem, out amendmentInfo))
                    {
                        mgr.LogMessage(String.Format("第{0}筆匯票資料信用狀不存在,信用狀號碼:{1}", idx + 1, draftItem.LCNO), logItem.LogID);
                        continue;
                    }

                    if (draftItem.LCBKACID != "009" + lcItem.CreditApplicationDocumentary.通知行)
                    {
                        mgr.LogMessage(String.Format("第{0}筆匯票資料押匯銀行與信用狀不符,押匯銀行:{1}", idx + 1, draftItem.LCBKACID), logItem.LogID);
                        continue;
                    }

                    DateTime draftDate = draftItem.EPRDAT.FromChineseDate();
                    int appYear = draftDate.Year - 1911;

                    NegoDraft draft = null;
                    if (amendmentInfo != null)
                    {
                        draft = amendmentInfo.NegoDraft.Where(d => d.DraftNo == draftItem.SQNO).FirstOrDefault();
                    }
                    else
                    {
                        draft = lcItem.NegoDraft.Where(d => d.DraftNo == draftItem.SQNO).FirstOrDefault();
                    }

                    if (draft != null)
                    {
                        mgr.LogMessage(String.Format("第{0}筆匯票資料重複押匯,LCNO:{1},LCSQNO:{2},SQNO:{3}", idx + 1, draftItem.LCNO, draftItem.LCSQNO, draftItem.SQNO), logItem.LogID);
                        continue;
                    }
                    else
                    {
                        draft = new NegoDraft
                            {
                                Documentary = new Documentary
                                {
                                    DocType = (int)Naming.DocumentTypeDefinition.押匯申請書,
                                    DocDate = DateTime.ParseExact(draftItem.TRNTM, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture),
                                },
                                NegoDraftExtension = new NegoDraftExtension
                                {
                                    DraftType = (int)(serviceID == BeneficiaryServiceGroup.ServiceDefinition.Chimei
                                                    ? Naming.DraftType.CHIMEI
                                                    : Naming.DraftType.FPG), //  lcItem.CreditApplicationDocumentary.BeneficiaryData.DraftType,
                                    NegoBranch = lcItem.CreditApplicationDocumentary.通知行,
                                    DueDate = importDate,
                                    NegotiateDate = importDate,
                                    LcBranch = lcItem.CreditApplicationDocumentary.開狀行
                                },
                                NegoPrompt = prompt,
                                Amount = draftItem.EPRAMT,
                                AppSeq = 0,
                                AppYear = appYear,
                                DraftNo = draftItem.SQNO,
                                ImportDate = importDate,
                                LcID = lcItem.LcID,
                                ShipmentDate = draftItem.OUTMDAT.FromChineseDate(),
                                DraftContent = draftItem.FILE1,
                                ItemName = draftItem.DSRCONT,
                                InvoiceCount = (int)draftItem.IVNUM,
                                DownloadFlag = 0
                            };

                        draft.FpgNegoDraft = new FpgNegoDraft
                        {
                            匯入銀行代碼 = draftItem.BROWBK,
                            Remark = String.Format("{0}{1}{2}", draftItem.COMT1, draftItem.COMT2, draftItem.COMT3),
                            押匯提示地點 = draftItem.HNTSITE,
                            匯入帳號 = draftItem.REMIACNO,
                            匯入戶名 = draftItem.REMIACNONM,
                            營業員姓名 = draftItem.CTM,
                            營業員連絡電話 = draftItem.TEL,
                            匯入銀行名稱 = draftItem.BKNM
                        };

                        if (amendmentInfo != null)
                            draft.AmendingID = amendmentInfo.AmendingID;

                        if ((amendmentInfo == null && lcItem.CreditApplicationDocumentary.IsAccepted != true)
                            || (amendmentInfo != null && amendmentInfo.AmendingLcApplication.IsAccepted != true))
                        {
                            draft.Documentary.DoDeny(Naming.DocumentLevel.拒絕押匯_自動退回, null, "信用狀受益人未接受");
                            draft.IntentToDispatch();
                        }
                        else if (draftItem.OUTMDAT.FromChineseDate() > lcItem.SpecificNote.最後交貨日)
                        {
                            draft.Documentary.DoDeny(Naming.DocumentLevel.拒絕押匯_自動退回, null, "出貨日晚於信用狀最後交貨日");
                            draft.IntentToDispatch();
                        }
                        else if (lcItem.可用餘額 == 0)
                        {
                            draft.Documentary.DoDeny(Naming.DocumentLevel.瑕疵押匯, Settings.Default.SystemID, "信用狀餘額為零,請於優利主機再次確認。");
                        }
                        else if (lcItem.可用餘額 < draft.Amount)
                        {
                            draft.Documentary.DoDeny(Naming.DocumentLevel.瑕疵押匯, Settings.Default.SystemID, "信用狀餘額不足,請於優利主機再次確認。");
                        }
                        else
                        {
                            draft.Documentary.CurrentLevel = (int)Naming.DocumentLevel.押匯文件準備中;
                            new DocumentaryLevel
                            {
                                Documentary = draft.Documentary,
                                LevelDate = importDate,
                                DocLevel = (int)Naming.DocumentLevel.押匯文件準備中
                            };
                        }

                        draftTable.InsertOnSubmit(draft);
                    }

                    mgr.SubmitChanges();
                    draft.UpdateAppSeq(mgr);
                    checkItems.Add(draftItem);

                }
                catch (Exception ex)
                {
                    ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                    mgr.LogException(ex, logItem.LogID);
                }
            }

            j2sp.MakeCommitment(logItem, new ModelCore.Schema.FPG.J2SPITEM
            {
                TXD2SB30 = checkItems.ToArray()
            });
        }

        class _Supplement
        {
            public String LcNo { get; set; }
            public String LcSqNo { get; set; }
            public List<_SupplementDraft> Draft { get; set; }
        }

        class _SupplementDraft
        {
            public ModelCore.Schema.FPG.J2SPITEMTXD2SB30 SB30 { get; set; }
            public List<_SupplementInvoice> Invoice { get; set; }
        }

        class _SupplementInvoice
        {
            public ModelCore.Schema.FPG.J2SPITEMTXD2SB32 SB32 { get; set; }
            public List<ModelCore.Schema.FPG.J2SPITEMTXD2SB33> InvoiceDetail { get; set; }
        }

        public static List<NegoDraft> SupplementNegoDraft(this GenericManager<LcEntityDataContext> mgr, ModelCore.Schema.FPG.J2SP sb30, ModelCore.Schema.FPG.J2SP sb32, ModelCore.Schema.FPG.J2SP sb33, BeneficiaryServiceGroup.ServiceDefinition? service = null)
        {
            List<_Supplement> items = new List<_Supplement>();
            if (sb30.ITEM.TXD2SB30 != null && sb30.ITEM.TXD2SB30.Length > 0)
            {
                foreach (var lc in sb30.ITEM.TXD2SB30.GroupBy(d => new { d.LCNO, d.LCSQNO }))
                {
                    items.Add(new _Supplement
                    {
                        LcNo = lc.Key.LCNO,
                        LcSqNo = lc.Key.LCSQNO,
                        Draft = lc.Select(d => new _SupplementDraft { SB30 = d }).ToList()
                    });
                }

                if (sb32.ITEM.TXD2SB32 != null && sb32.ITEM.TXD2SB32.Length > 0)
                {
                    foreach (var inv in sb32.ITEM.TXD2SB32)
                    {
                        var lc = items.Where(s => s.LcNo == inv.LCNO && s.LcSqNo == inv.LCSQNO).FirstOrDefault();
                        if (lc != null)
                        {
                            var draft = lc.Draft.Where(d => d.SB30.SQNO == inv.SQNO).FirstOrDefault();
                            if (draft != null)
                            {
                                if (draft.Invoice == null)
                                    draft.Invoice = new List<_SupplementInvoice>();
                                draft.Invoice.Add(new _SupplementInvoice { SB32 = inv });
                            }
                        }
                    }
                }

                if (sb33.ITEM.TXD2SB33 != null && sb33.ITEM.TXD2SB33.Length > 0)
                {
                    foreach (var detail in sb33.ITEM.TXD2SB33)
                    {
                        var lc = items.Where(s => s.LcNo == detail.LCNO && s.LcSqNo == detail.LCSQNO).FirstOrDefault();
                        if (lc != null)
                        {
                            var draft = lc.Draft.Where(d => d.SB30.SQNO == detail.SQNO).FirstOrDefault();
                            if (draft != null)
                            {
                                var inv = draft.Invoice.Where(i => i.SB32.IVNO == detail.IVNO).FirstOrDefault();
                                if (inv != null)
                                {
                                    if (inv.InvoiceDetail == null)
                                        inv.InvoiceDetail = new List<ModelCore.Schema.FPG.J2SPITEMTXD2SB33>();
                                    inv.InvoiceDetail.Add(detail);
                                }
                            }
                        }
                    }
                }

                return mgr.processSupplement(items, service);

            }
                                    
            return new List<NegoDraft>();
        }

        private static List<NegoDraft> processSupplement(this GenericManager<LcEntityDataContext> mgr, List<_Supplement> items, BeneficiaryServiceGroup.ServiceDefinition? service = null)
        {
            // TODO: 1. save NegoPrompt
            DateTime importDate = DateTime.Now;
            List<NegoDraft> results = new List<NegoDraft>();
            // TODO: 2. save NegoDraft
            var draftTable = mgr.GetTable<NegoDraft>();
            int appSeq = 1;
            int appYear = DateTime.Today.Year - 1911;
            var singleDraft = draftTable.Where(d => d.AppYear == appYear).OrderByDescending(d => d.DraftID).FirstOrDefault();
            if (singleDraft != null)
                appSeq = singleDraft.AppSeq.Value + 1;

            foreach (var item in items)
            {

                LetterOfCredit lcItem;
                AmendingLcInformation amendmentInfo;

                if (!mgr.FindLc(item.LcNo, item.LcSqNo, out lcItem, out amendmentInfo))
                {
                    continue;
                }

                if (service.HasValue)
                {
                    if (lcItem.CreditApplicationDocumentary.FpgLcItem?.GroupDepartment.BeneficiaryGroup.ServiceID != (int?)service)
                    {
                        continue;
                    }
                }

                foreach (var suppDraft in item.Draft)
                {
                    var draftItem = suppDraft.SB30;
                    try
                    {
                        if (draftItem.LCBKACID != "009" + lcItem.CreditApplicationDocumentary.通知行)
                        {
                            continue;
                        }

                        DateTime draftDate = draftItem.EPRDAT.FromChineseDate();

                        NegoDraft draft = new NegoDraft
                        {
                            Documentary = new Documentary
                            {
                                DocType = (int)Naming.DocumentTypeDefinition.押匯申請書,
                                DocDate = importDate
                            },
                            NegoDraftExtension = new NegoDraftExtension
                            {
                                DraftType = (int)(service == BeneficiaryServiceGroup.ServiceDefinition.Chimei
                                                    ? Naming.DraftType.CHIMEI
                                                    : Naming.DraftType.FPG),    //lcItem.CreditApplicationDocumentary.BeneficiaryData.DraftType,
                                NegoBranch = lcItem.CreditApplicationDocumentary.通知行,
                                DueDate = importDate,
                                NegotiateDate = importDate,
                                LcBranch = lcItem.CreditApplicationDocumentary.開狀行
                            },
                            Amount = draftItem.EPRAMT,
                            AppSeq = appSeq,
                            AppYear = appYear,
                            DraftNo = draftItem.SQNO,
                            ImportDate = importDate,
                            LcID = lcItem.LcID,
                            LetterOfCredit = lcItem,
                            ShipmentDate = draftItem.OUTMDAT.FromChineseDate(),
                            DraftContent = draftItem.FILE1,
                            ItemName = draftItem.DSRCONT,
                            InvoiceCount = (int)draftItem.IVNUM,
                            DownloadFlag = 0
                        };

                        draft.FpgNegoDraft = new FpgNegoDraft
                        {
                            匯入銀行代碼 = draftItem.BROWBK,
                            Remark = String.Format("{0}{1}{2}", draftItem.COMT1, draftItem.COMT2, draftItem.COMT3),
                            押匯提示地點 = draftItem.HNTSITE,
                            匯入帳號 = draftItem.REMIACNO,
                            匯入戶名 = draftItem.REMIACNONM,
                            營業員姓名 = draftItem.CTM,
                            營業員連絡電話 = draftItem.TEL,
                            匯入銀行名稱 = draftItem.BKNM
                        };

                        if (amendmentInfo != null)
                            draft.AmendingID = amendmentInfo.AmendingID;

                        draft.Documentary.CurrentLevel = (int)Naming.DocumentLevel.押匯資料補登;
                        new DocumentaryLevel
                        {
                            Documentary = draft.Documentary,
                            LevelDate = importDate,
                            DocLevel = (int)Naming.DocumentLevel.押匯資料補登
                        };

                        mgr.processSupplementInvoice(draft, suppDraft);

                        results.Add(draft);

                    }
                    catch (Exception ex)
                    {
                        ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                    }
                }
            }

            return results;
        }

        private static void processSupplementInvoice(this GenericManager<LcEntityDataContext> mgr, NegoDraft draft, _SupplementDraft supplement)
        {
            if (supplement.Invoice == null || supplement.Invoice.Count < 1)
                return;

            DateTime importDate = DateTime.Now;

            foreach (var item in supplement.Invoice)
            {
                var invItem = item.SB32;
                try
                {

                    if (invItem.LCBKACID != supplement.SB30.LCBKACID)
                    {
                        continue;
                    }

                    NegoInvoice negoInv = draft.NegoAffair.Select(a => a.NegoInvoice).Where(i => i.InvoiceNo == invItem.IVNO).FirstOrDefault();
                    if (negoInv == null)
                    {
                        negoInv = new NegoInvoice
                        {
                            InvoiceNo = invItem.IVNO
                        };

                        draft.NegoAffair.Add(new NegoAffair
                        {
                            NegoInvoice = negoInv,
                            NegoAmount = invItem.EPRAMTSpecified ? invItem.EPRAMT : 0
                        });
                    }

                    negoInv.ImportDate = importDate;
                    negoInv.DownloadFlag = 0;
                    negoInv.InvoiceAmount = invItem.IVAMT;
                    negoInv.InvoiceDate = invItem.IVOPNDAT.FromChineseDate();

                    mgr.processSupplementInvoiceDetail(draft, item);

                }
                catch (Exception ex)
                {
                    ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                }
            }
        }

        private static void processSupplementInvoiceDetail(this GenericManager<LcEntityDataContext> mgr, NegoDraft draft,_SupplementInvoice supplement)
        {
            if (supplement.InvoiceDetail == null || supplement.InvoiceDetail.Count < 1)
                return;

            DateTime importDate = DateTime.Now;

            foreach (var invItem in supplement.InvoiceDetail)
            {
                try
                {
                    if (invItem.LCBKACID != supplement.SB32.LCBKACID)
                    {
                        continue;
                    }

                    NegoInvoice negoInv = draft.NegoAffair.Select(a => a.NegoInvoice).Where(i => i.InvoiceNo == invItem.IVNO).FirstOrDefault();
                    if (negoInv == null)
                    {
                        continue;
                    }

                    NegoInvoiceDetail detail = negoInv.NegoInvoiceDetail.Where(i => i.ItemNo == invItem.IT).FirstOrDefault();
                    if (detail == null)
                    {
                        detail = new NegoInvoiceDetail
                        {
                            InvoiceID = negoInv.InvoiceID,
                            ItemNo = invItem.IT
                        };

                        negoInv.NegoInvoiceDetail.Add(detail);
                    }
                    detail.Spec = invItem.DSR;
                    negoInv.DownloadFlag = 1;
                }
                catch (Exception ex)
                {
                    ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                }
            }

        }



        public static void ReceiveNegoInvoice(this GenericManager<LcEntityDataContext> mgr, ModelCore.Schema.FPG.J2SP j2sp, DataPortLog logItem)
        {
            DateTime importDate = DateTime.Now;
            List<ModelCore.Schema.FPG.J2SPITEMTXD2SB32> checkItems = new List<ModelCore.Schema.FPG.J2SPITEMTXD2SB32>();

            for (int idx = 0; idx < j2sp.ITEM.TXD2SB32.Length; idx++)
            {
                var invItem = j2sp.ITEM.TXD2SB32[idx];
                try
                {
                    LetterOfCredit lcItem;
                    AmendingLcInformation amendmentInfo;
                    NegoDraft draft;

                    if (!mgr.FindDraft(invItem.LCNO, invItem.LCSQNO, invItem.SQNO, out lcItem, out amendmentInfo, out draft))
                    {
                        mgr.LogMessage(String.Format("第{0}筆發票資料沒有對應的匯票資料,發票號碼:{1}", idx + 1, invItem.IVNO), logItem.LogID);
                        continue;
                    }

                    if (invItem.LCBKACID != "009" + lcItem.CreditApplicationDocumentary.通知行)
                    {
                        mgr.LogMessage(String.Format("第{0}筆發票資料押匯銀行與信用狀不符,押匯銀行:{1}", idx + 1, invItem.LCBKACID), logItem.LogID);
                        continue;
                    }

                    NegoInvoice negoInv = draft.NegoAffair.Select(a => a.NegoInvoice).Where(i => i.InvoiceNo == invItem.IVNO).FirstOrDefault();
                    if (negoInv == null)
                    {
                        negoInv = new NegoInvoice
                        {
                            InvoiceNo = invItem.IVNO
                        };

                        draft.NegoAffair.Add(new NegoAffair
                        {
                            NegoInvoice = negoInv,
                            NegoAmount = invItem.EPRAMTSpecified ? invItem.EPRAMT : 0
                        });
                    }

                    negoInv.ImportDate = importDate;
                    negoInv.DownloadFlag = 0;
                    negoInv.InvoiceAmount = invItem.IVAMT;
                    negoInv.InvoiceDate = invItem.IVOPNDAT.FromChineseDate();
                    negoInv.ReceiptNo = invItem.IVPESRFNO;
                    mgr.SubmitChanges();

                    if (mgr.GetTable<NegoAffair>().Where(a => a.InvoiceID == negoInv.InvoiceID && a.NegoDraft.Documentary.CurrentLevel != (int)Naming.DocumentLevel.銀行已拒絕)
                        .Sum(a => (decimal?)a.NegoAmount) > negoInv.InvoiceAmount)
                    {
                        mgr.LogMessage(String.Format("第{0}筆發票資料押匯金額總額已超過發票金額,發票號碼:{1}", idx + 1, invItem.IVNO), logItem.LogID);
                        draft.Documentary.DoDeny(Naming.DocumentLevel.拒絕押匯_自動退回, null, "押匯金額大於發票金額");
                        draft.IntentToDispatch();
                        mgr.SubmitChanges();
                    }
                    else if (draft.Documentary.CurrentLevel == (int)Naming.DocumentLevel.押匯文件準備中 || draft.Documentary.CurrentLevel == (int)Naming.DocumentLevel.瑕疵押匯)
                    {
                        if (lcItem.SpecificNote.押匯發票起始日.HasValue && lcItem.SpecificNote.押匯發票起始日.Value > negoInv.InvoiceDate.Value)
                        {
                            mgr.LogMessage(String.Format("第{0}筆發票資料發票日早於發票起始日,發票號碼:{1}", idx + 1, invItem.IVNO), logItem.LogID);
                            draft.Documentary.DoDeny(Naming.DocumentLevel.瑕疵押匯, null, "押匯發票日早於發票起始日");
                            mgr.SubmitChanges();
                        }
                        if (lcItem.SpecificNote.接受發票早於開狀日 != true && negoInv.InvoiceDate < lcItem.LcDate.Date)
                        {
                            mgr.LogMessage(String.Format("第{0}筆發票資料發票日早於開狀日,發票號碼:{1}", idx + 1, invItem.IVNO), logItem.LogID);
                            draft.Documentary.DoDeny(Naming.DocumentLevel.瑕疵押匯, null, "押匯發票日早於開狀日");
                            mgr.SubmitChanges();
                        }
                        if (lcItem.CreditApplicationDocumentary.BeneficiaryData.Organization.ReceiptNo != invItem.IVPESRFNO)
                        {
                            mgr.LogMessage(String.Format("第{0}筆發票資料發票開立人統編與受益人統編不符,發票號碼:{1}", idx + 1, invItem.IVNO), logItem.LogID);
                            draft.Documentary.DoDeny(Naming.DocumentLevel.瑕疵押匯, null, "發票開立人統編與受益人統編不符");
                            mgr.SubmitChanges();
                        }
                    }

                    checkItems.Add(invItem);
                }
                catch (Exception ex)
                {
                    ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                    //CommonLib.Core.Utility.Logger.Error(ex);
                    mgr.LogException(ex, logItem.LogID);
                }
            }

            j2sp.MakeCommitment(logItem, new ModelCore.Schema.FPG.J2SPITEM
            {
                TXD2SB32 = checkItems.ToArray()
            });
        }

        public static void ReceiveInvoiceDetail(this GenericManager<LcEntityDataContext> mgr, ModelCore.Schema.FPG.J2SP j2sp, DataPortLog logItem)
        {
            DateTime importDate = DateTime.Now;
            List<ModelCore.Schema.FPG.J2SPITEMTXD2SB33> checkItems = new List<ModelCore.Schema.FPG.J2SPITEMTXD2SB33>();

            for (int idx = 0; idx < j2sp.ITEM.TXD2SB33.Length; idx++)
            {
                var invItem = j2sp.ITEM.TXD2SB33[idx];
                try
                {
                    LetterOfCredit lcItem;
                    AmendingLcInformation amendmentInfo;
                    NegoDraft draft;

                    if (!mgr.FindDraft(invItem.LCNO, invItem.LCSQNO,invItem.SQNO, out lcItem, out amendmentInfo,out draft))
                    {
                        mgr.LogMessage(String.Format("第{0}筆發票資料沒有對應的匯票資料,發票號碼:{1}", idx + 1, invItem.IVNO), logItem.LogID);
                        continue;
                    }

                    if (invItem.LCBKACID != "009" + lcItem.CreditApplicationDocumentary.通知行)
                    {
                        mgr.LogMessage(String.Format("第{0}筆發票資料押匯銀行與信用狀不符,押匯銀行:{1}", idx + 1, invItem.LCBKACID), logItem.LogID);
                        continue;
                    }

                    NegoInvoice negoInv = draft.NegoAffair.Select(a => a.NegoInvoice).Where(i => i.InvoiceNo == invItem.IVNO).FirstOrDefault();
                    if (negoInv == null)
                    {
                        mgr.LogMessage(String.Format("第{0}筆發票資料未建立,發票號碼:{1}", idx + 1, invItem.IVNO), logItem.LogID);
                        continue;
                    }

                    NegoInvoiceDetail detail = negoInv.NegoInvoiceDetail.Where(i => i.ItemNo == invItem.IT).FirstOrDefault();
                    if (detail == null)
                    {
                        detail = new NegoInvoiceDetail 
                        {
                            InvoiceID= negoInv.InvoiceID,
                            ItemNo = invItem.IT
                        };

                        negoInv.NegoInvoiceDetail.Add(detail);
                    }
                    detail.Spec = invItem.DSR;
                    negoInv.DownloadFlag = 1;
                    mgr.SubmitChanges();
                    checkItems.Add(invItem);
                }
                catch (Exception ex)
                {
                    ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                    mgr.LogException(ex, logItem.LogID);
                }
            }

            j2sp.MakeCommitment(logItem, new ModelCore.Schema.FPG.J2SPITEM
            {
                TXD2SB33 = checkItems.ToArray()
            });

        }

        public static void CheckDraftInvoice(this GenericManager<LcEntityDataContext> mgr)
        {
            var items = mgr.GetTable<Documentary>().Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.押匯文件準備中)
                .Select(d => d.NegoDraft).Where(d => d.NegoAffair.Count(a => a.NegoInvoice.DownloadFlag == 1) == d.InvoiceCount);

            foreach (var item in items)
            {
                item.DownloadFlag = 1;
                if (item.LetterOfCredit.SpecificNote.押匯起始日.HasValue && item.LetterOfCredit.SpecificNote.押匯起始日.Value > item.ImportDate)
                {
                    item.Documentary.DoDeny(Naming.DocumentLevel.瑕疵押匯, null, "押匯日早於押匯起始日");
                }
                else
                {
                    item.Documentary.DocumentaryLevel.Add(new DocumentaryLevel
                    {
                        DocLevel = (int)Naming.DocumentLevel.待經辦審核,
                        LevelDate = DateTime.Now
                    });
                }
                item.Documentary.CurrentLevel = (int)Naming.DocumentLevel.待經辦審核;
                mgr.SubmitChanges();

                MessageNotification.CreateInboxMessage(item.DraftID, Naming.MessageTypeDefinition.MSG_NEGO_APP_READY, Naming.MessageReceipent.ForBank);
                MessageNotification.CreateMailMessage(mgr, item.DraftID, Naming.MessageTypeDefinition.MSG_NEGO_APP_READY, Naming.MessageReceipent.ForBank);
                BankManagement.BankManager.DoDraftR3801(item.DraftID);

            }

            items = mgr.GetTable<Documentary>().Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.瑕疵押匯)
                            .Select(d => d.NegoDraft).Where(d => d.NegoAffair.Count(a => a.NegoInvoice.DownloadFlag == 1) == d.InvoiceCount
                                && d.DownloadFlag != 1);

            foreach (var item in items)
            {
                item.DownloadFlag = 1;
                if (item.LetterOfCredit.SpecificNote.押匯起始日.HasValue && item.LetterOfCredit.SpecificNote.押匯起始日.Value > item.ImportDate)
                {
                    item.Documentary.DoDeny(Naming.DocumentLevel.瑕疵押匯, null, "押匯日早於押匯起始日");
                }
                mgr.SubmitChanges();

                MessageNotification.CreateInboxMessage(item.DraftID, Naming.MessageTypeDefinition.MSG_NEGO_APP_READY, Naming.MessageReceipent.ForBank);
                MessageNotification.CreateMailMessage(mgr, item.DraftID, Naming.MessageTypeDefinition.MSG_NEGO_APP_READY, Naming.MessageReceipent.ForBank);
                BankManagement.BankManager.DoDraftR3801(item.DraftID);

            }

        }

        public static List<ModelCore.Schema.FPG.J2SPITEMTXD2SB39> ReviewFpgDraftNegotiation(this GenericManager<LcEntityDataContext> mgr, ModelCore.Schema.FPG.J2SP j2sp, DataPortLog logItem)
        {
            List<ModelCore.Schema.FPG.J2SPITEMTXD2SB39> items = new List<ModelCore.Schema.FPG.J2SPITEMTXD2SB39>();
            foreach (var item in j2sp.ITEM.TXD2SB39)
            {
                try
                {
                    LetterOfCredit lcItem;
                    AmendingLcInformation amendmentInfo;
                    NegoDraft draft;

                    if (!mgr.FindDraft(item.LCNO, item.LCSQNO, item.SQNO, out lcItem, out amendmentInfo, out draft)
                        || draft.Documentary.CurrentLevel == (int)Naming.DocumentLevel.押匯文件準備中)
                    {
                        items.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                    mgr.LogException(ex, logItem.LogID);
                }
            }
            j2sp.MakeCommitment(logItem, new ModelCore.Schema.FPG.J2SPITEM
            {
                TXD2SB39 = j2sp.ITEM.TXD2SB39
            });
            return items;
        }

        public static void SendNegotiationPromptRequest(this GenericManager<LcEntityDataContext> mgr, List<ModelCore.Schema.FPG.J2SPITEMTXD2SB39> items, BeneficiaryServiceGroup.ServiceDefinition? serviceID)
        {
            DateTime now = DateTime.Now;

            ModelCore.Schema.FPG.J2SP j2sp = new ModelCore.Schema.FPG.J2SP
            {
                BkId = "009",
                FILENAME = String.Format("{0:yyyyMMddHHmmssfff}009_J2SP_TXD2SB40.xml", now),
                FLAG = 1,
                FLAGSpecified = true,
                TABLENAME = "TXD2SB40",
                ITEM = new ModelCore.Schema.FPG.J2SPITEM
                {
                    id = "sign"
                }
            };

            if (items.Count > 0)
            {
                j2sp.ITEM.TXD2SB40 = items.Select(i => new ModelCore.Schema.FPG.J2SPITEMTXD2SB40
                {
                    LCBKACID = i.LCBKACID,
                    LCNO = i.LCNO,
                    LCSQNO = i.LCSQNO,
                    SQNO = i.SQNO,
                    TRNTM = String.Format("{0:yyyyMMddHHmmss}", DateTime.Now)
                }).ToArray();

                var logItem = mgr.SendToFPG(j2sp, serviceID);
                logItem.NegoPromptRequestQueue = new NegoPromptRequestQueue { };
                mgr.SubmitChanges();
            }
        }

        public static String CreateRemittanceDataFile(this GenericManager<LcEntityDataContext> mgr, Organization beneficiary, IEnumerable<FpgNegoRemittance> items)
        {
            String fileName = Path.Combine(CommonLib.Core.Utility.Logger.LogDailyPath, String.Format("PCCUTN_{0:yyyyMMddHHmmssf}.txt", DateTime.Now));
            Encoding enc = Encoding.GetEncoding(950);

            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                var item = items.First();

                fs.WriteByte((byte)'1');
                fs.Write(enc.GetBytes("009"), 0, 3);
                fs.Write(enc.GetBytes(item.FpgNegoDraft.NegoDraft.LetterOfCredit.CreditApplicationDocumentary.通知行), 0, 4);
                fs.Write(enc.GetBytes(String.Format("{0}", item.FpgNegoDraft.NegoDraft.PaymentNotification.入戶帳號).PadLeft(16, '0')), 0, 16);
                fs.Write(enc.GetBytes(String.Format("{0:yyyyMMdd}", item.RemittanceDate)), 0, 8);
                fs.Write(enc.GetBytes(item.FpgNegoDraft.NegoDraft.LetterOfCredit.CreditApplicationDocumentary.通知行), 0, 4);
                fs.Write(enc.GetBytes(item.FpgNegoDraft.NegoDraft.LetterOfCredit.CreditApplicationDocumentary.BeneficiaryData.Organization.ReceiptNo.PadRight(10)), 0, 10);
                fs.Write(enc.GetBytes(String.Format("{0}", item.FpgNegoDraft.NegoDraft.PaymentNotification.入戶帳號名稱).PadLeft(60, ' ')), 0, 60);
                fs.Write(enc.GetBytes("  "), 0, 2);
                fs.Write(enc.GetBytes("    "), 0, 4);
                fs.Write(enc.GetBytes("  "), 0, 2);
                fs.Write(enc.GetBytes("499"), 0, 3);
                fs.Write(enc.GetBytes("AMS01"), 0, 5);
                fs.Write(enc.GetBytes("1"), 0, 1);
                fs.Write(enc.GetBytes("01"), 0, 2);
                fs.Write(enc.GetBytes("00"), 0, 2);
                fs.Write(enc.GetBytes("".PadLeft(23, ' ')), 0, 23);
                fs.Write(enc.GetBytes("".PadLeft(200, ' ')), 0, 200);

                int idx = 0;
                foreach (var remittance in items)
                {
                    fs.WriteByte((byte)'2');
                    fs.Write(enc.GetBytes(String.Format("{0:00000}", ++idx)), 0, 5);
                    fs.Write(enc.GetBytes(String.Format("{0}", remittance.FpgNegoDraft.匯入銀行代碼).PadLeft(7, '0')), 0, 7);
                    fs.Write(enc.GetBytes(String.Format("{0}", remittance.FpgNegoDraft.匯入帳號).PadLeft(16, '0')), 0, 16);
                    fs.WriteByte((byte)'0');
                    fs.Write(enc.GetBytes("".PadLeft(10, ' ')), 0, 10);
                    fs.Write(enc.GetBytes(String.Format("{0}", remittance.FpgNegoDraft.匯入戶名).PadRight(60, ' ')), 0, 60);
                    fs.Write(enc.GetBytes(String.Format("{0:yyyyMMdd}", item.RemittanceDate)), 0, 8);
                    fs.WriteByte((byte)'2');
                    fs.WriteByte((byte)'0');
                    fs.Write(enc.GetBytes("00"), 0, 2);
                    fs.Write(enc.GetBytes(String.Format("{0:000000000000.00}", remittance.FpgNegoDraft.NegoDraft.Amount).Replace(".", "")), 0, 14);
                    fs.Write(enc.GetBytes("99"), 0, 2);
                    fs.Write(enc.GetBytes("".PadLeft(10, ' ')), 0, 10);
                    fs.Write(enc.GetBytes("".PadLeft(10, ' ')), 0, 10);
                    fs.WriteByte((byte)'2');
                    fs.WriteByte((byte)'1');
                    fs.Write(enc.GetBytes("".PadLeft(50, ' ')), 0, 50);
                    fs.Write(enc.GetBytes("".PadLeft(3, ' ')), 0, 3);
                    fs.Write(enc.GetBytes("".PadLeft(8, ' ')), 0, 8);
                    fs.Write(enc.GetBytes("".PadLeft(20, ' ')), 0, 20);
                    fs.Write(enc.GetBytes("電子押匯匯款".PadRight(60, ' ')), 0, 60);
                    fs.Write(enc.GetBytes(String.Format("{0:yyyyMMdd}15{1:000000}", item.RemittanceDate, item.DraftID).PadRight(30, ' ')), 0, 30);
                    fs.Write(enc.GetBytes("".PadLeft(11, ' ')), 0, 11);
                    fs.Write(enc.GetBytes("".PadLeft(18, ' ')), 0, 18);
                }

                fs.WriteByte((byte)'3');
                fs.Write(enc.GetBytes(String.Format("{0:00000000000000.00}", items.Select(f => f.FpgNegoDraft.NegoDraft).Sum(d => d.Amount)).Replace(".", "")), 0, 16);
                fs.Write(enc.GetBytes(String.Format("{0:0000000000}", items.Count()).Replace(".", "")), 0, 10);
                fs.Write(enc.GetBytes("".PadRight(16, '0')), 0, 16);
                fs.Write(enc.GetBytes("".PadRight(10, '0')), 0, 10);
                fs.Write(enc.GetBytes("".PadRight(16, ' ')), 0, 16);
                fs.Write(enc.GetBytes("".PadRight(10, ' ')), 0, 10);
                fs.Write(enc.GetBytes("".PadRight(271, ' ')), 0, 271);

            }
            return fileName;
        }

        public static void SendB8500(this GenericManager<LcEntityDataContext> mgr, Organization beneficiary, IEnumerable<FpgNegoRemittance> items, DataPortLog log)
        {
            var item = items.First();
            Txn_B8500 txn = new Txn_B8500();
            txn.Rq.EAIBody.MsgRq.SvcRq.CIFKEY = beneficiary.ReceiptNo;
            txn.Rq.EAIBody.MsgRq.SvcRq.TYPE = "499";
            txn.Rq.EAIBody.MsgRq.SvcRq.KIND = "PC01";
            txn.Rq.EAIBody.MsgRq.SvcRq.NAME = Path.GetFileName(log.ContentPath);
            txn.Rq.EAIBody.MsgRq.SvcRq.WCNT = String.Format("{0:0000000000}", items.Count());
            txn.Rq.EAIBody.MsgRq.SvcRq.WAMT = String.Format("{0:000000000000.00}", items.Select(f => f.FpgNegoDraft.NegoDraft).Sum(d => d.Amount)).Replace(".", "");
            txn.Rq.EAIBody.MsgRq.SvcRq.DCNT = "0000000000";
            txn.Rq.EAIBody.MsgRq.SvcRq.DAMT = "00000000000000";
            txn.Rq.EAIBody.MsgRq.SvcRq.CHKNO = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.KINBR = item.FpgNegoDraft.NegoDraft.LetterOfCredit.CreditApplicationDocumentary.通知行;
            txn.Rq.EAIBody.MsgRq.SvcRq.EMPNOT = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.EMPNOS = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.TLRNO = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.INDEXKEY = String.Format("{0:yyyyMMdd}20{1:000000}", item.RemittanceDate, log.LogID);

            using (WebClient wc = new WebClient())
            {
                wc.Credentials = new NetworkCredential(Settings.Default.FpgRemittanceFtpUserName, Settings.Default.FpgRemittanceFtpPWD);
                wc.UploadFile(String.Format(Settings.Default.FpgRemittanceFtp, DateTime.Today), log.ContentPath);
            }

            log.FpgFileName = txn.Rq.EAIBody.MsgRq.SvcRq.INDEXKEY;

            if (txn.Commit())
            {
                log.TransportTime = DateTime.Now;
            }
            else
            {
                log.ExceptionLog = new ExceptionLog
                {
                    LogTime = DateTime.Now,
                    Message = String.Format("{0}:{1}", txn.Rs.EAIBody.MsgRs.Header.RspCode
                        , mgr.ReadErrCodeDis(txn))
                };
                log.AlertDataQueue = new AlertDataQueue { };
            }
            mgr.SubmitChanges();

        }

        public static void CreateBatchRemittance(this GenericManager<LcEntityDataContext> mgr, FpgNegoRemittance item)
        {
            bool toCreate = item.FpgNegoRemittanceLog.Count == 0;

            item.Status = (int)Naming.RemittanceStatusDefinition.匯款資料已送出;
            mgr.DeleteAnyOnSubmit<DocumentDispatch>(d => d.DocID == item.DraftID);
            if (toCreate)
            {
                decimal totalAmt = item.FpgNegoDraft.NegoDraft.Amount;
                while (totalAmt > 0)
                {
                    var amount = Math.Min(totalAmt, 50000000M);
                    item.FpgNegoRemittanceLog.Add(new FpgNegoRemittanceLog
                        {
                            Amount = amount,
                            DraftID = item.DraftID,
                            SeqNo = 0,
                            Status = (int)Naming.RemittanceStatusDefinition.匯款資料已送出,
                            DataPortLog = new DataPortLog
                            {
                                Catalog = (int)Naming.TransportCatalogDefinition.BatchFpgNegoRemittance,
                                Direction = (int)Naming.TransportDirection.Outbound
                            },
                            FpgNegoRemittanceDispatch = new FpgNegoRemittanceDispatch { },
                        });

                    totalAmt -= amount;
                }
            }
            else
            {
                foreach(var log in item.FpgNegoRemittanceLog)
                {
                    log.Status = (int)Naming.RemittanceStatusDefinition.匯款資料已送出;
                    if (log.FpgNegoRemittanceDispatch == null)
                    {
                        log.FpgNegoRemittanceDispatch = new FpgNegoRemittanceDispatch { };
                    }
                }
            }
            mgr.SubmitChanges();

            if (toCreate)
            {
                foreach (var log in item.FpgNegoRemittanceLog)
                {
                    log.BatchNo = String.Format("{0:yyyyMMdd}20{1:000000}", item.RemittanceDate, log.RemittanceID % 1000000);
                    log.DPMTID = String.Format("{0:yyyyMMdd}15{1:000000}", DateTime.Today, log.RemittanceID % 1000000);

                    log.DataPortLog.ContentPath = Path.Combine(CommonLib.Core.Utility.Logger.LogDailyPath, String.Format("RT-{0:0000000000}.xml", log.RemittanceID));
                    log.DataPortLog.FpgFileName = log.BatchNo;

                    mgr.SubmitChanges();
                }
            }
        }


        public static void SendP1002(this GenericManager<LcEntityDataContext> models, FpgNegoRemittanceLog item)
        {
            bool toCreate = item.Status == (int)Naming.RemittanceStatusDefinition.匯款資料已送出;

            if (toCreate)
            {
                if(createP1002(item,out Txn_P1002 p1002))
                {
                    bool hasEC = false;

                    try
                    {
                        hasEC = !doR1000(models, item);
                    }
                    catch (Exception ex)
                    {
                        ModelCore.Helper.Logger.Error(ex);
                        item.Status = (int)Naming.RemittanceStatusDefinition.匯款失敗;
                        item.FpgNegoRemittance.Status = (int)Naming.RemittanceStatusDefinition.匯款失敗;
                        item.Description = ex.Message;
                        models.SubmitChanges();

                        hasEC = true;
                    }

                    if(hasEC)
                    {
                        //P1002 EC
                        p1002.Rq.EAIBody.MsgRq.SvcRq.HCODE = "1";
                        p1002.Rq.EAIBody.MsgRq.SvcRq.ECKIN = p1002.Rs.EAIBody.MsgRs.SvcRs.BRNO;
                        p1002.Rq.EAIBody.MsgRq.SvcRq.ECTRM = p1002.Rs.EAIBody.MsgRs.SvcRs.TRMSEQ;
                        p1002.Rq.EAIBody.MsgRq.SvcRq.ECTNO = p1002.Rs.EAIBody.MsgRs.SvcRs.TXTNO;
                        p1002.DoTransaction();
                        //
                    }
                }
                else
                {
                    item.Status = (int)Naming.RemittanceStatusDefinition.匯款失敗;
                    item.FpgNegoRemittance.Status = (int)Naming.RemittanceStatusDefinition.匯款失敗;
                    item.Description = p1002.GetResponseDescription();
                    item.DPMTID = null;
                    models.SubmitChanges();
                }
            }
        }

        private static bool createP1002(FpgNegoRemittanceLog log,out Txn_P1002 p1002)
        {
            FpgNegoRemittance item = log.FpgNegoRemittance;
            Organization beneficiary = item.FpgNegoDraft.NegoDraft.LetterOfCredit.CreditApplicationDocumentary.BeneficiaryData.Organization;

            p1002 = new Txn_P1002();
            p1002.Rq.EAIBody.MsgRq.SvcRq.KINBR = item.FpgNegoDraft.NegoDraft.LetterOfCredit.CreditApplicationDocumentary.通知行;
            p1002.Rq.EAIBody.MsgRq.SvcRq.DSCPT = "005";
            p1002.Rq.EAIBody.MsgRq.SvcRq.ACTNO = beneficiary.OrganizationStatus.ReserveAccount?.Replace("-", "");
            p1002.Rq.EAIBody.MsgRq.SvcRq.TXAMT = log.Amount.ToString();
            p1002.Rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";
            p1002.Rq.EAIBody.MsgRq.SvcRq.CIFKEY = beneficiary.OrganizationStatus.ReserveAccountReceiptNo;
            p1002.Rq.EAIBody.MsgRq.SvcRq.TXDAY = String.Format("{0:yyyy-MM-dd}", item.RemittanceDate);
            p1002.Rq.EAIBody.MsgRq.SvcRq.CDTIME = String.Format("{0:HHmmss}", DateTime.Now);
            p1002.Rq.EAIBody.MsgRq.SvcRq.BANKID = item.FpgNegoDraft.匯入銀行代碼.Substring(0, 3);
            p1002.Rq.EAIBody.MsgRq.SvcRq.TRNACTNO = item.FpgNegoDraft.匯入帳號;
            p1002.Rq.EAIBody.MsgRq.SvcRq.TXTYPE = "2";
            p1002.Rq.EAIBody.MsgRq.SvcRq.TXMEMO = "電子押匯匯款";
            p1002.Rq.EAIBody.MsgRq.SvcRq.CNAME = beneficiary.OrganizationStatus.ReserveAccountName;
            p1002.Rq.EAIBody.MsgRq.SvcRq.FEEBYWHO = "2";
            p1002.Rq.EAIBody.MsgRq.SvcRq.RCVNAME = item.FpgNegoDraft.匯入戶名;
            p1002.Rq.EAIBody.MsgRq.SvcRq.RCVMEMO = "電子押匯匯款";
            p1002.Rq.EAIBody.MsgRq.SvcRq.REMARK = "電子押匯匯款";

            return p1002.Commit();

        }

        public static void SendA1000(this GenericManager<LcEntityDataContext> models, FpgNegoRemittanceLog item)
        {
            bool toCreate = item.Status == (int)Naming.RemittanceStatusDefinition.匯款資料已送出;

            if (toCreate)
            {
                if (createA1000(item, out Txn_A1000 a1000))
                {
                    bool hasEC = false;

                    try
                    {
                        hasEC = !doR1000(models, item);
                    }
                    catch (Exception ex)
                    {
                        ModelCore.Helper.Logger.Error(ex);
                        item.Status = (int)Naming.RemittanceStatusDefinition.匯款失敗;
                        item.FpgNegoRemittance.Status = (int)Naming.RemittanceStatusDefinition.匯款失敗;
                        item.Description = ex.Message;
                        models.SubmitChanges();

                        hasEC = true;
                    }

                    if (hasEC)
                    {
                        // A1000 反向沖帳
                        //<IDSCPT>060</IDSCPT>  ：此處改為 060。(連動轉)
                        //<CRDB>1</CRDB>        ：維持不變為1。
                        //<TACNO>BBBB</TACNO>   ：改為2144。
                        //<TSBNO>001</TSBNO>    ：改為160。
                        //<TDTLNO>098</TDTLNO>  ：改為000。
                        //<REFNO>12144160000098</REFNO>  ：改為1BBBB004098098。
                        a1000.Rq.EAIBody.MsgRq.SvcRq.IDSCPT = "060";
                        a1000.Rq.EAIBody.MsgRq.SvcRq.TACNO = "2144";
                        a1000.Rq.EAIBody.MsgRq.SvcRq.TSBNO = "160";
                        a1000.Rq.EAIBody.MsgRq.SvcRq.TDTLNO = "000";
                        a1000.Rq.EAIBody.MsgRq.SvcRq.REFNO = "1BBBB004098098";
                        a1000.DoTransaction();                        
                    }
                }
                else
                {
                    item.Status = (int)Naming.RemittanceStatusDefinition.匯款失敗;
                    item.FpgNegoRemittance.Status = (int)Naming.RemittanceStatusDefinition.匯款失敗;
                    item.Description = a1000.GetResponseDescription();
                    item.DPMTID = null;
                    models.SubmitChanges();
                }
            }
        }

        private static bool doR1000(GenericManager<LcEntityDataContext> models, FpgNegoRemittanceLog item)
        {
            if (createR1000(item, out Txn_R1000 r1000))
            {
                item.DPMTID = r1000.PMTID;
                item.Status = (int)Naming.RemittanceStatusDefinition.匯款已完成;
                models.SubmitChanges();

                models.ExecuteCommand("delete FpgNegoRemittanceDispatch where RemittanceID = {0}", item.RemittanceID);

                var remittance = item.FpgNegoRemittance;
                if (remittance.Status == (int)Naming.RemittanceStatusDefinition.匯款資料已送出
                    && remittance.FpgNegoRemittanceLog.Count(l => l.DataPortLog.AlertDataQueue != null) == 0)
                {
                    remittance.FpgNegoDraft.NegoDraft.Documentary.DoApprove(Naming.DocumentLevel.押匯申請已轉帳, Settings.Default.SystemID, null);
                    remittance.Status = (int)Naming.RemittanceStatusDefinition.匯款已完成;
                    remittance.FpgNegoDraft.NegoDraft.IntentToDispatch();
                    models.SubmitChanges();
                }

                return true;
            }
            else
            {
                item.Status = (int)Naming.RemittanceStatusDefinition.匯款失敗;
                item.FpgNegoRemittance.Status = (int)Naming.RemittanceStatusDefinition.匯款失敗;
                item.Description = r1000.GetResponseDescription();
                models.SubmitChanges();
            }

            return false;
        }

        private static bool createA1000(FpgNegoRemittanceLog log, out Txn_A1000 eai)
        {
            FpgNegoRemittance item = log.FpgNegoRemittance;
            Organization beneficiary = item.FpgNegoDraft.NegoDraft.LetterOfCredit.CreditApplicationDocumentary.BeneficiaryData.Organization;

            eai = new Txn_A1000();
            eai.Rq.EAIBody.MsgRq.SvcRq.KINBR = item.FpgNegoDraft.NegoDraft.LetterOfCredit.CreditApplicationDocumentary.通知行;
            eai.Rq.EAIBody.MsgRq.SvcRq.IDSCPT = "004";
            eai.Rq.EAIBody.MsgRq.SvcRq.CRDB = "1";
            eai.Rq.EAIBody.MsgRq.SvcRq.SECNO = "98";
            eai.Rq.EAIBody.MsgRq.SvcRq.ZERO2 = "00";
            eai.Rq.EAIBody.MsgRq.SvcRq.SUPNO = "00000";
            eai.Rq.EAIBody.MsgRq.SvcRq.CIFKEY = "9999";
            eai.Rq.EAIBody.MsgRq.SvcRq.BRELCD = "2";
            eai.Rq.EAIBody.MsgRq.SvcRq.NBCD = "0";
            eai.Rq.EAIBody.MsgRq.SvcRq.FORCE = "0";
            eai.Rq.EAIBody.MsgRq.SvcRq.PSUEDO = "0";
            eai.Rq.EAIBody.MsgRq.SvcRq.HCODE = "0";
            eai.Rq.EAIBody.MsgRq.SvcRq.ECKIN = "0000";
            eai.Rq.EAIBody.MsgRq.SvcRq.RELNO3 = "00000000";
            eai.Rq.EAIBody.MsgRq.SvcRq.DRVSNO = "0000000000";
            eai.Rq.EAIBody.MsgRq.SvcRq.NOTE1 = "000";
            eai.Rq.EAIBody.MsgRq.SvcRq.REMK1 = item.FpgNegoDraft.NegoDraft.LetterOfCredit.LcNo;
            eai.Rq.EAIBody.MsgRq.SvcRq.NOTE2 = "000";
            eai.Rq.EAIBody.MsgRq.SvcRq.REMK2 = "";
            eai.Rq.EAIBody.MsgRq.SvcRq.MISDEP = "";
            eai.Rq.EAIBody.MsgRq.SvcRq.PDUT = "0000";
            eai.Rq.EAIBody.MsgRq.SvcRq.MISDEP = "";
            eai.Rq.EAIBody.MsgRq.SvcRq.MACTNO = "";
            eai.Rq.EAIBody.MsgRq.SvcRq.TBRNO = item.FpgNegoDraft.NegoDraft.LetterOfCredit.CreditApplicationDocumentary.通知行;
            eai.Rq.EAIBody.MsgRq.SvcRq.TDEPT = "1";
            eai.Rq.EAIBody.MsgRq.SvcRq.TACNO = "BBBB";
            eai.Rq.EAIBody.MsgRq.SvcRq.TSBNO = "001";
            eai.Rq.EAIBody.MsgRq.SvcRq.TDTLNO = "098";
            eai.Rq.EAIBody.MsgRq.SvcRq.TRVSTYP = "0";
            eai.Rq.EAIBody.MsgRq.SvcRq.TSECNO = "98";
            eai.Rq.EAIBody.MsgRq.SvcRq.CRVSNO = "";
            eai.Rq.EAIBody.MsgRq.SvcRq.CNAMNO = "";
            eai.Rq.EAIBody.MsgRq.SvcRq.DNAMNO = "";
            eai.Rq.EAIBody.MsgRq.SvcRq.FLAG = "0";
            eai.Rq.EAIBody.MsgRq.SvcRq.EDATE = "00000000";
            eai.Rq.EAIBody.MsgRq.SvcRq.KIND = "0";
            eai.Rq.EAIBody.MsgRq.SvcRq.TRACTNO = "";
            eai.Rq.EAIBody.MsgRq.SvcRq.APPSRNO = "000000";
            eai.Rq.EAIBody.MsgRq.SvcRq.SDATE = "00000000";
            eai.Rq.EAIBody.MsgRq.SvcRq.REFNO = "12144160000098";
            eai.Rq.EAIBody.MsgRq.SvcRq.TXAMT = $"{log.Amount:.00}";

            return eai.Commit();

        }

        public static long CountHandlingCharge(this FpgNegoRemittanceLog log)
        {
            FpgNegoRemittance item = log.FpgNegoRemittance;
            Organization beneficiary = item.FpgNegoDraft.NegoDraft.LetterOfCredit.CreditApplicationDocumentary.BeneficiaryData.Organization;
            OrganizationBranchSettings settings = beneficiary.OrganizationBranchSettings
                .Where(s => s.Status == (int)Naming.BeneficiaryStatus.已核准)
                .Where(s => s.BankCode == item.FpgNegoDraft.NegoDraft.LetterOfCredit.CreditApplicationDocumentary.通知行)
                .OrderByDescending(s => s.SettingID)
                .FirstOrDefault();

            return (long)(settings?.HandlingCharge ?? beneficiary.OrganizationStatus.HandlingCharge ?? 30)
                + (Math.Max((long)(log.Amount ?? 0) - Settings.Default.HandlingChargeBaseAmount, 0) + Settings.Default.HandlingChargeStepAmount - 1) / Settings.Default.HandlingChargeStepAmount * (long)(settings?.StepCharge ?? beneficiary.OrganizationStatus.StepCharge ?? 5);
        }

        private static bool createR1000(FpgNegoRemittanceLog log, out Txn_R1000 r1000)
        {
            FpgNegoRemittance item = log.FpgNegoRemittance;
            Organization beneficiary = item.FpgNegoDraft.NegoDraft.LetterOfCredit.CreditApplicationDocumentary.BeneficiaryData.Organization;

            r1000 = new Txn_R1000();

            r1000.Rq.EAIBody.MsgRq.SvcRq.KINBR = item.FpgNegoDraft.NegoDraft.LetterOfCredit.CreditApplicationDocumentary.通知行;
            r1000.Rq.EAIBody.MsgRq.SvcRq.DSCPT = "";    // "005";
            r1000.Rq.EAIBody.MsgRq.SvcRq.ACBRNO = item.FpgNegoDraft.NegoDraft.LetterOfCredit.CreditApplicationDocumentary.通知行;

            if (item.FpgNegoDraft.NegoDraft.NegoDraftExtension.DraftType == (int)Naming.DraftType.CHIMEI)
            {
                r1000.Rq.EAIBody.MsgRq.SvcRq.ACTNO = (beneficiary.OrganizationStatus.ReserveAccount == null) ?
               "0000000000000000" : beneficiary.OrganizationStatus.ReserveAccount.Replace("-", "");
            }
            else 
            {
                r1000.Rq.EAIBody.MsgRq.SvcRq.ACTNO = beneficiary.OrganizationStatus.ReserveAccount.Replace("-", "");
            }
          
            r1000.Rq.EAIBody.MsgRq.SvcRq.HCODE = "0";
            r1000.Rq.EAIBody.MsgRq.SvcRq.CRDB = "2";
            var chargeFee = log.CountHandlingCharge();
            r1000.Rq.EAIBody.MsgRq.SvcRq.TXAMT = $"{Math.Max((long)(log.Amount ?? 0) - chargeFee, 0)}";
            r1000.Rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";
            r1000.Rq.EAIBody.MsgRq.SvcRq.RBRNO = item.FpgNegoDraft.NegoDraft.LetterOfCredit.CreditApplicationDocumentary.通知行;
            r1000.Rq.EAIBody.MsgRq.SvcRq.SECNO = "25";

            if (item.FpgNegoDraft.NegoDraft.NegoDraftExtension.DraftType == (int)Naming.DraftType.CHIMEI)
            {
                r1000.Rq.EAIBody.MsgRq.SvcRq.CIFKEY = (beneficiary.OrganizationStatus.ReserveAccountReceiptNo == null) ?
                 beneficiary.ReceiptNo : beneficiary.OrganizationStatus.ReserveAccountReceiptNo;
            }
            else
            {
                r1000.Rq.EAIBody.MsgRq.SvcRq.CIFKEY = beneficiary.OrganizationStatus.ReserveAccountReceiptNo;
            }
            
            r1000.Rq.EAIBody.MsgRq.SvcRq.FBRNO = item.FpgNegoDraft.NegoDraft.LetterOfCredit.CreditApplicationDocumentary.通知行;
            r1000.Rq.EAIBody.MsgRq.SvcRq.REMDAY = String.Format("{0:yyyy-MM-dd}", item.RemittanceDate);
            r1000.Rq.EAIBody.MsgRq.SvcRq.RCVBK = item.FpgNegoDraft.匯入銀行代碼;
            r1000.Rq.EAIBody.MsgRq.SvcRq.REMTYPE = "11";
            r1000.Rq.EAIBody.MsgRq.SvcRq.ORGDAY = String.Format("{0:yyyy-MM-dd}", item.RemittanceDate);
            if (item.FpgNegoDraft.NegoDraft.NegoDraftExtension.DraftType == (int)Naming.DraftType.CHIMEI)
            {
                r1000.Rq.EAIBody.MsgRq.SvcRq.REMCIF = (beneficiary.OrganizationStatus.ReserveAccountReceiptNo == null) ?
                 beneficiary.ReceiptNo : beneficiary.OrganizationStatus.ReserveAccountReceiptNo;
            }
            else
            {
                r1000.Rq.EAIBody.MsgRq.SvcRq.REMCIF = beneficiary.OrganizationStatus.ReserveAccountReceiptNo;
            }

            if (item.FpgNegoDraft.NegoDraft.NegoDraftExtension.DraftType == (int)Naming.DraftType.CHIMEI)
            {
                r1000.Rq.EAIBody.MsgRq.SvcRq.MACTNO = (beneficiary.OrganizationStatus.ReserveAccount == null) ?
               "00000000000000" : beneficiary.OrganizationStatus.ReserveAccount.Replace("-", "");
            }
            else
            {
                r1000.Rq.EAIBody.MsgRq.SvcRq.MACTNO = beneficiary.OrganizationStatus.ReserveAccount.Replace("-", "");
            }

            r1000.Rq.EAIBody.MsgRq.SvcRq.TXTYPE = "002";
            r1000.Rq.EAIBody.MsgRq.SvcRq.REMHC = $"{chargeFee}";
            r1000.Rq.EAIBody.MsgRq.SvcRq.HCTXTYPE = "002";

            if (item.FpgNegoDraft.NegoDraft.NegoDraftExtension.DraftType == (int)Naming.DraftType.CHIMEI)
            {
                r1000.Rq.EAIBody.MsgRq.SvcRq.REMNAM = (beneficiary.OrganizationStatus.ReserveAccountName==null) ?
                  beneficiary.CompanyName : beneficiary.OrganizationStatus.ReserveAccountName;
            }
            else
            {
                r1000.Rq.EAIBody.MsgRq.SvcRq.REMNAM = beneficiary.OrganizationStatus.ReserveAccountName;
            }
           
            r1000.Rq.EAIBody.MsgRq.SvcRq.RCVACT = item.FpgNegoDraft.匯入帳號;
            r1000.Rq.EAIBody.MsgRq.SvcRq.RCVNAM = item.FpgNegoDraft.匯入戶名;
            r1000.Rq.EAIBody.MsgRq.SvcRq.REMARK = "電子押匯匯款";
            r1000.Rq.EAIBody.MsgRq.SvcRq.FEDICD = "4";
            r1000.Rq.EAIBody.MsgRq.SvcRq.CHRGTYP = "";  //"1";
            r1000.Rq.EAIBody.MsgRq.SvcRq.DONATE = "0";

            return r1000.Commit();
        }
                

        private static int __DPMTID_IDX = 1;
        private static DateTime __DPMTID_Date = DateTime.Today;

    }
}
