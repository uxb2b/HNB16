using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;

using CommonLib.Core.DataWork;
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
                AMT = item.LcItems.開狀金額.Value,
                AMTSpecified = true,
                ASSNPAYCKEDDAT = item.LcItems.PaymentDate.HasValue ? String.Format("{0}{1:MMdd}", item.LcItems.PaymentDate.Value.Year - 1911, item.LcItems.PaymentDate.Value) : null,
                ASSNEPRDAT = item.SpecificNotes.押匯起始日.HasValue ? String.Format("{0:yyyyMMdd}", item.SpecificNotes.押匯起始日) : null,
                BENFADR = item.BeneDetails.Addr,
                BENFCOCNM = item.BeneDetails.CompanyName,
                BENFDIV = item.FpgLcItem.DepartID.Replace(GroupDepartment.DefaultDepartID, ""),
                BENFRFNO = item.Beneficiary.Organization.ReceiptNo,
                BKCHKCOMT = "",
                BKCHKTM = String.Format("{0:yyyyMMddHHmmss}", item.Documentary.DocumentaryAllowance.OrderByDescending(a => a.ApprovalDate).First().ApprovalDate),
                CTMNM = item.FpgLcItem.ContactName,
                CTMTEL = item.FpgLcItem.ContactPhone,
                CUAPLCOMT = item.SpecificNotes.其他,
                CUCOCNM = item.ApplicantDetails.CompanyName,
                CUCY = item.LcItems.CurrencyType.AbbrevName,
                CUNO = item.FpgLcItem.CustomerNo,
                DLEDDAT = String.Format("{0:yyyyMMdd}", item.SpecificNotes.最後交貨日),
                DSRCONT = item.LcItems.GetGoodsDescription(),
                EFFDDAT = String.Format("{0}{1:MMdd}", item.LcItems.有效期限.Value.Year - 1911, item.LcItems.有效期限.Value),
                EIVMK = item.SpecificNotes.接受發票電子訊息 == true ? "Y" : "N",
                EPRDIFRR = item.FpgLcItem.押匯允差比例.HasValue ? item.FpgLcItem.押匯允差比例.Value : 0,
                EPRDIFRRSpecified = true,
                IVADRMK = item.SpecificNotes.接受發票人地址與受益人地址不符 == true ? "Y" : "N",
                IVAMTMK = item.SpecificNotes.接受發票金額大於開狀金額 == true ? "Y" : "N",
                IVDATMK = item.SpecificNotes.接受發票早於開狀日 == true ? "Y" : "N",
                IVFROPNDAT = item.SpecificNotes.接受發票早於開狀日 == true && item.SpecificNotes.押匯發票起始日.HasValue
                            ? String.Format("{0:yyyyMMdd}", item.SpecificNotes.押匯發票起始日)
                            : ""    /*String.Format("{0:yyyyMMdd}", item.OpeningApplicationDocumentary.開狀日期)*/,
                LCAPLTM = String.Format("{0:yyyyMMddHHmmss}", item.ApplicationDate),
                LCBK = "009" + item.IssuingBankCode,
                LCBKACID = "009" + item.AdvisingBankCode,
                LCNO = item.LetterOfCredit.FirstOrDefault()?.LcNo,
                LCPESRFNO = item.CustomerOfBranch.Organization.ReceiptNo,
                LCSQNO = "01",
                PAMK = item.SpecificNotes.分批交貨 == true ? "Y" : "N",
                PAYCKFRSTD = "",
                PAYCKMK = item.SpecificNotes.受益人單獨蓋章 == true ? "Y" : "N",
                PAYDYS = item.UsanceDays > 0 && !item.AtSight ? item.UsanceDays.ToString() : null,
                PAYTY = item.AtSight ? "A" : "B",
                TRNTM = String.Format("{0:yyyyMMddHHmmss}", DateTime.Now)
            };
        }

        public static ModelCore.Schema.FPG.J2SPITEMTXD2SB29 BuildTXD2SB29(this AmendingLcApplication item)
        {
            return new ModelCore.Schema.FPG.J2SPITEMTXD2SB29
            {
                AMT = item.LcItems.開狀金額.Value,
                AMTSpecified = true,
                ASSNPAYCKEDDAT = item.LcItems.PaymentDate.HasValue ? String.Format("{0}{1:MMdd}", item.LcItems.PaymentDate.Value.Year - 1911, item.LcItems.PaymentDate.Value) : null,
                ASSNEPRDAT = item.SpecificNotes.押匯起始日.HasValue ? String.Format("{0:yyyyMMdd}", item.SpecificNotes.押匯起始日) : null,
                BENFADR = item.Source.Lc.Application.BeneDetails.Addr,
                BENFCOCNM = item.Source.Lc.Application.BeneDetails.CompanyName,
                BENFDIV = item.Source.Lc.Application.FpgLcItem.DepartID.Replace(GroupDepartment.DefaultDepartID, ""),
                BENFRFNO = item.Source.Lc.Application.Beneficiary.Organization.ReceiptNo,
                BKCHKCOMT = "",
                BKCHKTM = String.Format("{0:yyyyMMddHHmmss}", item.Documentary.DocumentaryAllowance.OrderByDescending(a => a.ApprovalDate).First().ApprovalDate),
                CTMNM = item.Source.Lc.Application.FpgLcItem.ContactName,
                CTMTEL = item.Source.Lc.Application.FpgLcItem.ContactPhone,
                CUAPLCOMT = item.SpecificNotes.其他,
                CUCOCNM = item.Source.Lc.Application.ApplicantDetails.CompanyName,
                CUCY = item.LcItems.CurrencyType.AbbrevName,
                CUNO = item.Source.Lc.Application.FpgLcItem.CustomerNo,
                DLEDDAT = String.Format("{0:yyyyMMdd}", item.SpecificNotes.最後交貨日),
                DSRCONT = item.LcItems.GetGoodsDescription(),
                EFFDDAT = String.Format("{0}{1:MMdd}", item.LcItems.有效期限.Value.Year - 1911, item.LcItems.有效期限.Value),
                EIVMK = item.SpecificNotes.接受發票電子訊息 == true ? "Y" : "N",
                EPRDIFRR = item.Source.Lc.Application.FpgLcItem.押匯允差比例.HasValue ? item.Source.Lc.Application.FpgLcItem.押匯允差比例.Value : 0,
                EPRDIFRRSpecified = true,
                IVADRMK = item.SpecificNotes.接受發票人地址與受益人地址不符 == true ? "Y" : "N",
                IVAMTMK = item.SpecificNotes.接受發票金額大於開狀金額 == true ? "Y" : "N",
                IVDATMK = item.SpecificNotes.接受發票早於開狀日 == true ? "Y" : "N",
                IVFROPNDAT = item.SpecificNotes.接受發票早於開狀日 == true && item.SpecificNotes.押匯發票起始日.HasValue
                            ? String.Format("{0:yyyyMMdd}", item.SpecificNotes.押匯發票起始日)
                            : ""    /*String.Format("{0:yyyyMMdd}", item.NegoLcVersion.Application.OpeningApplicationDocumentary.開狀日期)*/,
                LCAPLTM = String.Format("{0:yyyyMMddHHmmss}", item.Source.Lc.Application.ApplicationDate),
                LCBK = "009" + item.Source.Lc.Application.IssuingBankCode,
                LCBKACID = "009" + item.Source.Lc.Application.AdvisingBankCode,
                LCNO = item.Source.Lc.LcNo,
                LCPESRFNO = item.Source.Lc.Application.CustomerOfBranch.Organization.ReceiptNo,
                LCSQNO = String.Format("{0:00}", item.AmendingLcInformation.CurrentLc.VersionNo + 1),
                PAMK = item.SpecificNotes.分批交貨 == true ? "Y" : "N",
                PAYCKFRSTD = "",
                PAYCKMK = item.SpecificNotes.受益人單獨蓋章 == true ? "Y" : "N",
                PAYDYS = item.Source.Lc.Application.UsanceDays > 0 && !item.Source.Lc.Application.AtSight ? item.Source.Lc.Application.UsanceDays.ToString() : null,
                PAYTY = item.Source.Lc.Application.AtSight ? "A" : "B",
                TRNTM = String.Format("{0:yyyyMMddHHmmss}", DateTime.Now)
            };
        }

        public static ModelCore.Schema.FPG.J2SPITEMTXD2SB31 BuildTXD2SB31(this NegoDraft item, String statusCode)
        {
            var txdItem = new ModelCore.Schema.FPG.J2SPITEMTXD2SB31
            {
                SQNO = item.DraftNo,
                STS = statusCode,
                BENFRFNO = item.NegoLcVersion.Lc.Application.Beneficiary.Organization.ReceiptNo,
                BKCHKCOMT = "",
                LCBKACID = "009" + item.NegoLcVersion.Lc.Application.AdvisingBankCode,
                LCNO = item.NegoLcVersion.Lc.LcNo,
                LCSQNO = String.Format("{0:00}", item.NegoLcVersion.VersionNo + 1),
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
                LCBK = "009" + item.Source.Lc.Application.IssuingBankCode,
                LCNO = item.Source.Lc.LcNo,
                LCPESRFNO = item.Source.Lc.Application.CustomerOfBranch.Organization.ReceiptNo,
                LCSQNO = String.Format("{0:00}", item.AmendingLcInformation.CurrentLc.VersionNo + 1),
                TRNTM = String.Format("{0:yyyyMMddHHmmss}", DateTime.Now)
            };
        }

        public static ModelCore.Schema.FPG.J2SPITEMTXD2SB30 BuildTXD2SB30(this NegoDraft item)
        {
            return new ModelCore.Schema.FPG.J2SPITEMTXD2SB30
            {
                LCBKACID = "009" + item.NegoLcVersion.Lc.Application.AdvisingBankCode,
                LCNO = item.NegoLcVersion.Lc.LcNo,
                LCSQNO = String.Format("{0:00}", item.NegoLcVersion.VersionNo + 1),
                SQNO = item.DraftNo
            };
        }

        public static void AcceptLcAmendment(this GenericManager<LcEntityDbContext> mgr, ModelCore.Schema.FPG.J2SP j2sp, DataPortLog logItem)
        {
            var processLog = mgr.GetTable<DataProcessLog>();
            List<ModelCore.Schema.FPG.J2SPITEMTXD2SB38> checkItems = new List<ModelCore.Schema.FPG.J2SPITEMTXD2SB38>();

            foreach (var item in j2sp.ITEM.TXD2SB38)
            {
                try
                {
                    LetterOfCreditVersion lcItem;
                    AmendingLcInformation amendmentInfo;
                    if (mgr.FindLc(item.LCNO, item.LCSQNO, out lcItem, out amendmentInfo))
                    {
                        if (amendmentInfo == null)
                        {
                            lcItem.Lc.Application.IsAccepted = true;
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


        public static void ConfirmSentLc(this GenericManager<LcEntityDbContext> mgr, ModelCore.Schema.FPG.J2SP j2sp)
        {
            foreach (var item in j2sp.ITEM.TXD2SB29)
            {
                LetterOfCreditVersion lcItem;
                AmendingLcInformation amendmentInfo;
                if (mgr.FindLc(item.LCNO, item.LCSQNO, out lcItem, out amendmentInfo))
                {
                    DocumentDispatch dispatch = amendmentInfo != null ? amendmentInfo.AmendingLcApplication.Documentary.DocumentDispatch
                        : lcItem.Lc.Application.Documentary.DocumentDispatch;

                    if (dispatch != null)
                    {
                        mgr.GetTable<DocumentDispatch>().Remove(dispatch);
                        mgr.SubmitChanges();
                    }
                }
            }
        }

        public static void ConfirmSentNegoDraftStatus(this GenericManager<LcEntityDbContext> mgr, ModelCore.Schema.FPG.J2SP j2sp)
        {
            foreach (var item in j2sp.ITEM.TXD2SB31)
            {
                LetterOfCreditVersion lcItem;
                AmendingLcInformation amendmentInfo;
                NegoDraft draft;
                if (mgr.FindDraft(item.LCNO, item.LCSQNO,item.SQNO, out lcItem, out amendmentInfo,out draft))
                {
                    DocumentDispatch dispatch = draft.Documentary.DocumentDispatch;

                    if (dispatch != null)
                    {
                        mgr.GetTable<DocumentDispatch>().Remove(dispatch);
                        mgr.SubmitChanges();
                    }
                }
            }
        }

        public static void ConfirmSentNegoPromptRequest(this GenericManager<LcEntityDbContext> mgr, ModelCore.Schema.FPG.J2SP j2sp)
        {
            var queue = mgr.GetTable<NegoPromptRequestQueue>();
            var item = queue.Where(q => q.DataPortLog.FpgFileName == j2sp.INITIALFILE).FirstOrDefault();
            if (item != null)
            {
                queue.Remove(item);
                mgr.SubmitChanges();
            }
        }

        public static void PromptDraftNegotiation(this GenericManager<LcEntityDbContext> mgr, ModelCore.Schema.FPG.J2SP j2sp, DataPortLog logItem, BeneficiaryServiceGroup.ServiceDefinition? serviceID = null)
        {
            // TODO: 1. save Prompt
            DateTime importDate = DateTime.Now;

            NegoPrompt prompt = new NegoPrompt 
            {
                LogID = logItem.LogID,
                ImportDate = importDate
            };
            mgr.GetTable<NegoPrompt>().Add(prompt);
            mgr.SubmitChanges();

            List<ModelCore.Schema.FPG.J2SPITEMTXD2SB30> checkItems = new List<ModelCore.Schema.FPG.J2SPITEMTXD2SB30>();

            // TODO: 2. save NegoDraft
            var draftTable = mgr.GetTable<NegoDraft>();
            for (int idx = 0; idx < j2sp.ITEM.TXD2SB30.Length;idx++)
            {
                var draftItem = j2sp.ITEM.TXD2SB30[idx];
                try
                {
                    LetterOfCreditVersion lcItem;
                    AmendingLcInformation amendmentInfo;

                    if (!mgr.FindLc(draftItem.LCNO, draftItem.LCSQNO, out lcItem, out amendmentInfo))
                    {
                        mgr.LogMessage(String.Format("第{0}筆匯票資料信用狀不存在,信用狀號碼:{1}", idx + 1, draftItem.LCNO), logItem.LogID);
                        continue;
                    }

                    if (draftItem.LCBKACID != "009" + lcItem.Lc.Application.AdvisingBankCode)
                    {
                        mgr.LogMessage(String.Format("第{0}筆匯票資料押匯銀行與信用狀不符,押匯銀行:{1}", idx + 1, draftItem.LCBKACID), logItem.LogID);
                        continue;
                    }

                    DateTime draftDate = draftItem.EPRDAT.FromChineseDate();
                    int appYear = draftDate.Year - 1911;

                    NegoDraft draft = null;
                    draft = lcItem.NegoDraft.Where(d => d.DraftNo == draftItem.SQNO).FirstOrDefault();

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
                                                    : Naming.DraftType.FPG), //  lcItem.Application.Beneficiary.DraftType,
                                    NegoBranch = lcItem.Lc.Application.AdvisingBankCode,
                                    DueDate = importDate,
                                    LcBranch = lcItem.Lc.Application.IssuingBankCode
                                },
                                Prompt = prompt,
                                Amount = draftItem.EPRAMT,
                                AppSeq = 0,
                                AppYear = appYear,
                                DraftNo = draftItem.SQNO,
                                NegoDate = importDate,
                                NegoLcVersionID = lcItem.LcID,
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


                        if ((amendmentInfo == null && lcItem.Lc.Application.IsAccepted != true)
                            || (amendmentInfo != null && amendmentInfo.AmendingLcApplication.IsAccepted != true))
                        {
                            draft.Documentary.DoDeny(Naming.DocumentLevel.拒絕押匯_自動退回, null, "信用狀受益人未接受");
                            draft.IntentToDispatch();
                        }
                        else if (draftItem.OUTMDAT.FromChineseDate() > lcItem.SpecificNotes.最後交貨日)
                        {
                            draft.Documentary.DoDeny(Naming.DocumentLevel.拒絕押匯_自動退回, null, "出貨日晚於信用狀最後交貨日");
                            draft.IntentToDispatch();
                        }
                        else if (lcItem.Lc.可用餘額 == 0)
                        {
                            draft.Documentary.DoDeny(Naming.DocumentLevel.瑕疵押匯, Settings.Default.SystemID, "信用狀餘額為零,請於優利主機再次確認。");
                        }
                        else if (lcItem.Lc.可用餘額 < draft.Amount)
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

                        draftTable.Add(draft);
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

        public static List<NegoDraft> SupplementNegoDraft(this GenericManager<LcEntityDbContext> mgr, ModelCore.Schema.FPG.J2SP sb30, ModelCore.Schema.FPG.J2SP sb32, ModelCore.Schema.FPG.J2SP sb33, BeneficiaryServiceGroup.ServiceDefinition? service = null)
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

        private static List<NegoDraft> processSupplement(this GenericManager<LcEntityDbContext> mgr, List<_Supplement> items, BeneficiaryServiceGroup.ServiceDefinition? service = null)
        {
            // TODO: 1. save Prompt
            DateTime importDate = DateTime.Now;
            List<NegoDraft> results = new List<NegoDraft>();
            // TODO: 2. save NegoDraft
            var draftTable = mgr.GetTable<NegoDraft>();
            int appSeq = 1;
            int appYear = DateTime.Today.Year - 1911;
            var singleDraft = draftTable.Where(d => d.AppYear == appYear).OrderByDescending(d => d.DocumentaryID).FirstOrDefault();
            if (singleDraft != null)
                appSeq = singleDraft.AppSeq.Value + 1;

            foreach (var item in items)
            {

                LetterOfCreditVersion lcItem;
                AmendingLcInformation amendmentInfo;

                if (!mgr.FindLc(item.LcNo, item.LcSqNo, out lcItem, out amendmentInfo))
                {
                    continue;
                }

                if (service.HasValue)
                {
                    if (lcItem.Lc.Application.FpgLcItem?.GroupDepartment.Group.ServiceID != (int?)service)
                    {
                        continue;
                    }
                }

                foreach (var suppDraft in item.Draft)
                {
                    var draftItem = suppDraft.SB30;
                    try
                    {
                        if (draftItem.LCBKACID != "009" + lcItem.Lc.Application.AdvisingBankCode)
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
                                                    : Naming.DraftType.FPG),    //lcItem.Application.Beneficiary.DraftType,
                                NegoBranch = lcItem.Lc.Application.AdvisingBankCode,
                                DueDate = importDate,
                                LcBranch = lcItem.Lc.Application.IssuingBankCode
                            },
                            Amount = draftItem.EPRAMT,
                            AppSeq = appSeq,
                            AppYear = appYear,
                            DraftNo = draftItem.SQNO,
                            NegoDate = importDate,
                            NegoLcVersionID = lcItem.VersionID,
                            NegoLcVersion = lcItem,
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

        private static void processSupplementInvoice(this GenericManager<LcEntityDbContext> mgr, NegoDraft draft, _SupplementDraft supplement)
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

        private static void processSupplementInvoiceDetail(this GenericManager<LcEntityDbContext> mgr, NegoDraft draft,_SupplementInvoice supplement)
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



        public static void ReceiveNegoInvoice(this GenericManager<LcEntityDbContext> mgr, ModelCore.Schema.FPG.J2SP j2sp, DataPortLog logItem)
        {
            DateTime importDate = DateTime.Now;
            List<ModelCore.Schema.FPG.J2SPITEMTXD2SB32> checkItems = new List<ModelCore.Schema.FPG.J2SPITEMTXD2SB32>();

            for (int idx = 0; idx < j2sp.ITEM.TXD2SB32.Length; idx++)
            {
                var invItem = j2sp.ITEM.TXD2SB32[idx];
                try
                {
                    LetterOfCreditVersion lcItem;
                    AmendingLcInformation amendmentInfo;
                    NegoDraft draft;

                    if (!mgr.FindDraft(invItem.LCNO, invItem.LCSQNO, invItem.SQNO, out lcItem, out amendmentInfo, out draft))
                    {
                        mgr.LogMessage(String.Format("第{0}筆發票資料沒有對應的匯票資料,發票號碼:{1}", idx + 1, invItem.IVNO), logItem.LogID);
                        continue;
                    }

                    if (invItem.LCBKACID != "009" + lcItem.Lc.Application.AdvisingBankCode)
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

                    if (mgr.GetTable<NegoAffair>().Where(a => a.NegoInvoiceID == negoInv.InvoiceID && a.NegoDraft.Documentary.CurrentLevel != (int)Naming.DocumentLevel.銀行已拒絕)
                        .Sum(a => (decimal?)a.NegoAmount) > negoInv.InvoiceAmount)
                    {
                        mgr.LogMessage(String.Format("第{0}筆發票資料押匯金額總額已超過發票金額,發票號碼:{1}", idx + 1, invItem.IVNO), logItem.LogID);
                        draft.Documentary.DoDeny(Naming.DocumentLevel.拒絕押匯_自動退回, null, "押匯金額大於發票金額");
                        draft.IntentToDispatch();
                        mgr.SubmitChanges();
                    }
                    else if (draft.Documentary.CurrentLevel == (int)Naming.DocumentLevel.押匯文件準備中 || draft.Documentary.CurrentLevel == (int)Naming.DocumentLevel.瑕疵押匯)
                    {
                        if (lcItem.SpecificNotes.押匯發票起始日.HasValue && lcItem.SpecificNotes.押匯發票起始日.Value > negoInv.InvoiceDate.Value)
                        {
                            mgr.LogMessage(String.Format("第{0}筆發票資料發票日早於發票起始日,發票號碼:{1}", idx + 1, invItem.IVNO), logItem.LogID);
                            draft.Documentary.DoDeny(Naming.DocumentLevel.瑕疵押匯, null, "押匯發票日早於發票起始日");
                            mgr.SubmitChanges();
                        }
                        if (lcItem.SpecificNotes.接受發票早於開狀日 != true && negoInv.InvoiceDate < lcItem.Lc.LcDate.Date)
                        {
                            mgr.LogMessage(String.Format("第{0}筆發票資料發票日早於開狀日,發票號碼:{1}", idx + 1, invItem.IVNO), logItem.LogID);
                            draft.Documentary.DoDeny(Naming.DocumentLevel.瑕疵押匯, null, "押匯發票日早於開狀日");
                            mgr.SubmitChanges();
                        }
                        if (lcItem.Lc.Application.Beneficiary.Organization.ReceiptNo != invItem.IVPESRFNO)
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

        public static void ReceiveInvoiceDetail(this GenericManager<LcEntityDbContext> mgr, ModelCore.Schema.FPG.J2SP j2sp, DataPortLog logItem)
        {
            DateTime importDate = DateTime.Now;
            List<ModelCore.Schema.FPG.J2SPITEMTXD2SB33> checkItems = new List<ModelCore.Schema.FPG.J2SPITEMTXD2SB33>();

            for (int idx = 0; idx < j2sp.ITEM.TXD2SB33.Length; idx++)
            {
                var invItem = j2sp.ITEM.TXD2SB33[idx];
                try
                {
                    LetterOfCreditVersion lcItem;
                    AmendingLcInformation amendmentInfo;
                    NegoDraft draft;

                    if (!mgr.FindDraft(invItem.LCNO, invItem.LCSQNO,invItem.SQNO, out lcItem, out amendmentInfo,out draft))
                    {
                        mgr.LogMessage(String.Format("第{0}筆發票資料沒有對應的匯票資料,發票號碼:{1}", idx + 1, invItem.IVNO), logItem.LogID);
                        continue;
                    }

                    if (invItem.LCBKACID != "009" + lcItem.Lc.Application.AdvisingBankCode)
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

        public static void CheckDraftInvoice(this GenericManager<LcEntityDbContext> mgr)
        {
            var items = mgr.GetTable<Documentary>().Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.押匯文件準備中)
                .Select(d => d.NegoDraft).Where(d => d.NegoAffair.Count(a => a.NegoInvoice.DownloadFlag == 1) == d.InvoiceCount);

            foreach (var item in items)
            {
                item.DownloadFlag = 1;
                if (item.NegoLcVersion.SpecificNotes.押匯起始日.HasValue && item.NegoLcVersion.SpecificNotes.押匯起始日.Value > item.NegoDate)
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

                MessageNotification.CreateInboxMessage(item.DocumentaryID, Naming.MessageTypeDefinition.MSG_NEGO_APP_READY, Naming.MessageReceipent.ForBank);
                MessageNotification.CreateMailMessage(mgr, item.DocumentaryID, Naming.MessageTypeDefinition.MSG_NEGO_APP_READY, Naming.MessageReceipent.ForBank);

            }

            items = mgr.GetTable<Documentary>().Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.瑕疵押匯)
                            .Select(d => d.NegoDraft).Where(d => d.NegoAffair.Count(a => a.NegoInvoice.DownloadFlag == 1) == d.InvoiceCount
                                && d.DownloadFlag != 1);

            foreach (var item in items)
            {
                item.DownloadFlag = 1;
                if (item.NegoLcVersion.SpecificNotes.押匯起始日.HasValue && item.NegoLcVersion.SpecificNotes.押匯起始日.Value > item.NegoDate)
                {
                    item.Documentary.DoDeny(Naming.DocumentLevel.瑕疵押匯, null, "押匯日早於押匯起始日");
                }
                mgr.SubmitChanges();

                MessageNotification.CreateInboxMessage(item.DocumentaryID, Naming.MessageTypeDefinition.MSG_NEGO_APP_READY, Naming.MessageReceipent.ForBank);
                MessageNotification.CreateMailMessage(mgr, item.DocumentaryID, Naming.MessageTypeDefinition.MSG_NEGO_APP_READY, Naming.MessageReceipent.ForBank);

            }

        }

        public static List<ModelCore.Schema.FPG.J2SPITEMTXD2SB39> ReviewFpgDraftNegotiation(this GenericManager<LcEntityDbContext> mgr, ModelCore.Schema.FPG.J2SP j2sp, DataPortLog logItem)
        {
            List<ModelCore.Schema.FPG.J2SPITEMTXD2SB39> items = new List<ModelCore.Schema.FPG.J2SPITEMTXD2SB39>();
            foreach (var item in j2sp.ITEM.TXD2SB39)
            {
                try
                {
                    LetterOfCreditVersion lcItem;
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

        public static void SendNegotiationPromptRequest(this GenericManager<LcEntityDbContext> mgr, List<ModelCore.Schema.FPG.J2SPITEMTXD2SB39> items, BeneficiaryServiceGroup.ServiceDefinition? serviceID)
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


        public static void CreateBatchRemittance(this GenericManager<LcEntityDbContext> mgr, FpgNegoRemittance item)
        {
            bool toCreate = item.FpgNegoRemittanceLog.Count == 0;

            item.Status = (int)Naming.RemittanceStatusDefinition.匯款資料已送出;
            mgr.DeleteAnyOnSubmit<DocumentDispatch>(d => d.DocumentaryID == item.FpgNegoDraftID);
            if (toCreate)
            {
                decimal totalAmt = item.FpgNegoDraft.NegoDraft.Amount;
                while (totalAmt > 0)
                {
                    var amount = Math.Min(totalAmt, 50000000M);
                    item.FpgNegoRemittanceLog.Add(new FpgNegoRemittanceLog
                        {
                            Amount = amount,
                            FpgNegoRemittanceID = item.FpgNegoDraftID,
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


        private static bool createP1002(FpgNegoRemittanceLog log,out Txn_P1002 p1002)
        {
            FpgNegoRemittance item = log.FpgNegoRemittance;
            Organization beneficiary = item.FpgNegoDraft.NegoDraft.NegoLcVersion.Lc.Application.Beneficiary.Organization;

            p1002 = new Txn_P1002();
            p1002.Rq.EAIBody.MsgRq.SvcRq.KINBR = item.FpgNegoDraft.NegoDraft.NegoLcVersion.Lc.Application.AdvisingBankCode;
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

        private static bool doR1000(GenericManager<LcEntityDbContext> models, FpgNegoRemittanceLog item)
        {
            if (createR1000(item, out Txn_R1000 r1000))
            {
                item.DPMTID = r1000.PMTID;
                item.Status = (int)Naming.RemittanceStatusDefinition.匯款已完成;
                models.SubmitChanges();

                models.ExecuteCommand("delete FpgNegoRemittanceDispatch where FpgNegoRemittanceLogID = {0}", item.RemittanceID);

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
            Organization beneficiary = item.FpgNegoDraft.NegoDraft.NegoLcVersion.Lc.Application.Beneficiary.Organization;

            eai = new Txn_A1000();
            eai.Rq.EAIBody.MsgRq.SvcRq.KINBR = item.FpgNegoDraft.NegoDraft.NegoLcVersion.Lc.Application.AdvisingBankCode;
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
            eai.Rq.EAIBody.MsgRq.SvcRq.REMK1 = item.FpgNegoDraft.NegoDraft.NegoLcVersion.Lc.LcNo;
            eai.Rq.EAIBody.MsgRq.SvcRq.NOTE2 = "000";
            eai.Rq.EAIBody.MsgRq.SvcRq.REMK2 = "";
            eai.Rq.EAIBody.MsgRq.SvcRq.MISDEP = "";
            eai.Rq.EAIBody.MsgRq.SvcRq.PDUT = "0000";
            eai.Rq.EAIBody.MsgRq.SvcRq.MISDEP = "";
            eai.Rq.EAIBody.MsgRq.SvcRq.MACTNO = "";
            eai.Rq.EAIBody.MsgRq.SvcRq.TBRNO = item.FpgNegoDraft.NegoDraft.NegoLcVersion.Lc.Application.AdvisingBankCode;
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
            Organization beneficiary = item.FpgNegoDraft.NegoDraft.NegoLcVersion.Lc.Application.Beneficiary.Organization;
            OrganizationBranchSettings settings = beneficiary.OrganizationBranchSettings
                .Where(s => s.Status == (int)Naming.BeneficiaryStatus.已核准)
                .Where(s => s.BankCode == item.FpgNegoDraft.NegoDraft.NegoLcVersion.Lc.Application.AdvisingBankCode)
                .OrderByDescending(s => s.SettingID)
                .FirstOrDefault();

            return (long)(settings?.HandlingCharge ?? beneficiary.OrganizationStatus.HandlingCharge ?? 30)
                + (Math.Max((long)(log.Amount ?? 0) - Settings.Default.HandlingChargeBaseAmount, 0) + Settings.Default.HandlingChargeStepAmount - 1) / Settings.Default.HandlingChargeStepAmount * (long)(settings?.StepCharge ?? beneficiary.OrganizationStatus.StepCharge ?? 5);
        }

        private static bool createR1000(FpgNegoRemittanceLog log, out Txn_R1000 r1000)
        {
            FpgNegoRemittance item = log.FpgNegoRemittance;
            Organization beneficiary = item.FpgNegoDraft.NegoDraft.NegoLcVersion.Lc.Application.Beneficiary.Organization;

            r1000 = new Txn_R1000();

            r1000.Rq.EAIBody.MsgRq.SvcRq.KINBR = item.FpgNegoDraft.NegoDraft.NegoLcVersion.Lc.Application.AdvisingBankCode;
            r1000.Rq.EAIBody.MsgRq.SvcRq.DSCPT = "";    // "005";
            r1000.Rq.EAIBody.MsgRq.SvcRq.ACBRNO = item.FpgNegoDraft.NegoDraft.NegoLcVersion.Lc.Application.AdvisingBankCode;

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
            r1000.Rq.EAIBody.MsgRq.SvcRq.RBRNO = item.FpgNegoDraft.NegoDraft.NegoLcVersion.Lc.Application.AdvisingBankCode;
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
            
            r1000.Rq.EAIBody.MsgRq.SvcRq.FBRNO = item.FpgNegoDraft.NegoDraft.NegoLcVersion.Lc.Application.AdvisingBankCode;
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
