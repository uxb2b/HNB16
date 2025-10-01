using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using CommonLib.DataAccess;
using ModelCore.Locale;
using CommonLib.Utility;

namespace ModelCore.DataModel
{
    public static partial class ExtensionMethods
    {
        public static BankData GetIssuingBranch(this CreditApplicationDocumentary item)
        {
            return item.CustomerOfBranch.BankData;
        }

        public static LetterOfCredit GetLetterOfCredit(this CreditApplicationDocumentary item)
        {
            return item.LetterOfCredit.FirstOrDefault();
        }

        public static String AppNo(this NegoDraft draft)
        {
            return String.Format("N{0:000}{1:00000}", draft.AppYear, draft.AppSeq);
        }

        public static DateTime UsanceDate(this NegoDraft draft)
        {
            return draft.LcID.HasValue
                ? draft.LetterOfCreditVersion.LcItem.PaymentDate.HasValue
                    ? draft.LetterOfCreditVersion.LcItem.PaymentDate.Value : draft.ImportDate.AddDays(draft.LetterOfCreditVersion.LcItem.定日付款)
                    : draft.ImportDate.AddDays(draft.NegoLC.DueDays.HasValue ? draft.NegoLC.DueDays.Value : 0);

            //return draft.LcID.HasValue ? draft.ShipmentDate.Value.AddDays(draft.LetterOfCredit.LcItem.定日付款)
            //    : draft.ShipmentDate.Value.AddDays(draft.NegoLC.DueDays.Value);
        }

        public static bool AtSight(this NegoDraft draft)
        {
            return draft.LcID.HasValue
                ? draft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.見票即付
                : draft.NegoLC.DueDays == 0;

        }

        public static void GetLcAmendmentItems(this AmendingLcApplication item, out LcItem newItem, out LcItem oldItem,
            out AttachableDocument newAttach, out AttachableDocument oldAttach,
            out SpecificNote newSN, out SpecificNote oldSN,
            out String newNotifyingBank, out String oldNotifyingBank)
        {

            newItem = item.LcItem;
            newAttach = item.AttachableDocument;
            newSN = item.SpecificNote;
            newNotifyingBank = item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.通知行;

            oldAttach = item.LetterOfCreditVersion.AttachableDocument;
            oldItem = item.LetterOfCreditVersion.LcItem;
            oldSN = item.LetterOfCreditVersion.SpecificNote;
            oldNotifyingBank = item.LetterOfCreditVersion.NotifyingBank;
        }

        public static List<LcAmendatory> GetLcAmendmentDetails(this AmendingLcApplication item)
        {
            List<LcAmendatory> items = new List<LcAmendatory>();
            LcItem newItem, oldItem;
            AttachableDocument newAttach, oldAttach;
            SpecificNote newSN, oldSN;

            item.GetLcAmendmentItems(out newItem, out oldItem, out newAttach, out oldAttach, out newSN, out oldSN,out String newNotifyingBank,out String oldNotifyingBank);

            if (newNotifyingBank != null && newNotifyingBank!=oldNotifyingBank)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = "通知行：" + newNotifyingBank,
                    Original = "通知行：" + oldNotifyingBank
                });
            }

            if (newItem.開狀金額 != oldItem.開狀金額)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = "金額：新台幣 " + ValidityAgent.MoneyShow(newItem.開狀金額),
                    Original = "金額：新台幣 " + ValidityAgent.MoneyShow(oldItem.開狀金額)
                });
            }
            if (newItem.有效期限 != oldItem.有效期限)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = "有效期限：" + ValidityAgent.ConvertChineseDate(newItem.有效期限),
                    Original = "有效期限：" + ValidityAgent.ConvertChineseDate(oldItem.有效期限)
                });
            }

            if (newItem.定日付款 != oldItem.定日付款)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = "定日付款天數：" + newItem.定日付款.ToString(),
                    Original = "定日付款天數：" + oldItem.定日付款.ToString()
                });
            }
            if (newItem.PaymentDate != oldItem.PaymentDate)
            {
                if (newItem.PaymentDate.HasValue && newItem.PaymentDate.HasValue)
                {
                    items.Add(new LcAmendatory
                    {

                        Amendatory = "指定付款日：" + ValidityAgent.ConvertChineseDate(newItem.PaymentDate),

                        Original = "指定付款日：" + ValidityAgent.ConvertChineseDate(oldItem.PaymentDate)
                    });

                }
                else
                {
                    if (!newItem.PaymentDate.HasValue)
                    {
                        items.Add(new LcAmendatory
                        {

                            Amendatory = "指定付款日：" + "",

                            Original = "指定付款日：" + ValidityAgent.ConvertChineseDate(oldItem.PaymentDate)
                        });

                    }
                    else if (!oldItem.PaymentDate.HasValue)
                    {
                        items.Add(new LcAmendatory
                        {

                            Amendatory = "指定付款日：" + ValidityAgent.ConvertChineseDate(newItem.PaymentDate),

                            Original = "指定付款日："
                        });


                    }

                }
            }

            StringBuilder srcGoods = new StringBuilder("貨物名稱：");
            int idx = 1;
            if (!String.IsNullOrEmpty(newItem.Goods))
            {
                srcGoods.Append(String.Format("{0}. 品名:{1} 規格:{2} 單價:{3} 數量:{4} 金額:{5} 備註:{6}"
                                , idx++, newItem.Goods, null, null, null, null, null))
                        .Append("<br/>");
            }
            for (int i = 0; i < newItem.GoodsDetails.Count; i++)
            {
                var g = newItem.GoodsDetails[i];
                srcGoods.Append(String.Format("{0}. 品名:{1} 規格:{2} 單價:{3} 數量:{4} 金額:{5} 備註:{6}"
                            , idx++, g.ProductName, g.ProductSize, g.UnitPrice, g.Quantity, g.Amount, g.Remark))
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

            if (newAttach.匯票付款申請書 != oldAttach.匯票付款申請書)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = "匯票付款申請書乙份：" + ((true == newAttach.匯票付款申請書) ? "檢附" : "免附"),
                    Original = "匯票付款申請書乙份：" + ((true == oldAttach.匯票付款申請書) ? "檢附" : "免附")
                });
            }
            if (newAttach.統一發票 != oldAttach.統一發票)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = "統一發票：" + (newAttach.統一發票 == true ? "檢附" : "免附"),
                    Original = "統一發票：" + (oldAttach.統一發票 == true ? "檢附" : "免附")
                });
            }
            if (newAttach.電子發票證明聯 != oldAttach.電子發票證明聯)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = "電子發票證明聯：" + (newAttach.電子發票證明聯 == true ? "接受" : "不接受"),
                    Original = "電子發票證明聯：" + (oldAttach.電子發票證明聯 == true ? "接受" : "不接受")
                });
            }

            if (newAttach.其他 != oldAttach.其他)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = String.Format("檢附其他單據：{0}", newAttach.其他),
                    Original = String.Format("檢附其他單據：{0}", oldAttach.其他)
                });
            }
            if (newSN.原留印鑑相符 != oldSN.原留印鑑相符)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = " 匯票付款申請書上申請人所蓋印鑑應與原留印鑑相符：" + (true == newSN.原留印鑑相符 ? "是" : "否"),
                    Original = " 匯票付款申請書上申請人所蓋印鑑應與原留印鑑相符：" + (true == oldSN.原留印鑑相符 ? "是" : "否")
                });
            }
            if (newSN.受益人單獨蓋章 != oldSN.受益人單獨蓋章)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = " 匯票付款申請書由信用狀受益人單獨蓋章：" + (true == newSN.受益人單獨蓋章 ? "是" : "否"),
                    Original = " 匯票付款申請書由信用狀受益人單獨蓋章：" + (true == oldSN.受益人單獨蓋章 ? "是" : "否")
                });
            }
            if (newSN.分批交貨 != oldSN.分批交貨)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = " 分批交貨：" + (true == newSN.分批交貨 ? "可" : "不可"),
                    Original = " 分批交貨：" + (true == oldSN.分批交貨 ? "可" : "不可")
                });
            }
            if (newSN.最後交貨日 != oldSN.最後交貨日)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = " 最後交貨日期：" + ValidityAgent.ConvertChineseDate(newSN.最後交貨日),
                    Original = " 最後交貨日期：" + ValidityAgent.ConvertChineseDate(oldSN.最後交貨日)
                });
            }
            ///////
            if (newSN.押匯發票起始日 != oldSN.押匯發票起始日)
            {
                if (!Convert.IsDBNull(newSN.押匯發票起始日) && !Convert.IsDBNull(newSN.押匯發票起始日))
                {
                    items.Add(new LcAmendatory
                    {

                        Amendatory = "押匯發票起始日：" + ValidityAgent.ConvertChineseDate(newSN.押匯發票起始日),

                        Original = "押匯發票起始日：" + ValidityAgent.ConvertChineseDate(oldSN.押匯發票起始日)
                    });

                }
                else
                {
                    if (Convert.IsDBNull(newSN.押匯發票起始日))
                    {
                        items.Add(new LcAmendatory
                        {

                            Amendatory = "押匯發票起始日：" + "",

                            Original = "押匯發票起始日：" + ValidityAgent.ConvertChineseDate(oldSN.押匯發票起始日)
                        });

                    }
                    else if (Convert.IsDBNull(oldSN.押匯發票起始日))
                    {
                        items.Add(new LcAmendatory
                        {

                            Amendatory = "押匯發票起始日：" + ValidityAgent.ConvertChineseDate(newSN.押匯發票起始日),

                            Original = "押匯發票起始日："
                        });


                    }

                }
            }
            if (newSN.押匯起始日 != oldSN.押匯起始日)
            {
                if (newSN.押匯起始日.HasValue && newSN.押匯起始日.HasValue)
                {
                    items.Add(new LcAmendatory
                    {

                        Amendatory = "押匯起始日：" + ValidityAgent.ConvertChineseDate(newSN.押匯起始日),

                        Original = "押匯起始日：" + ValidityAgent.ConvertChineseDate(oldSN.押匯起始日)
                    });

                }
                else
                {
                    if (oldSN.押匯起始日.HasValue)
                    {
                        items.Add(new LcAmendatory
                        {

                            Amendatory = "押匯起始日：" + "",

                            Original = "押匯起始日：" + ValidityAgent.ConvertChineseDate(oldSN.押匯起始日)
                        });

                    }
                    else if (newSN.押匯起始日.HasValue)
                    {
                        items.Add(new LcAmendatory
                        {

                            Amendatory = "押匯起始日：" + ValidityAgent.ConvertChineseDate(newSN.押匯起始日),

                            Original = "押匯起始日："
                        });


                    }

                }
            }
            ///////
            if (newSN.接受發票金額大於開狀金額 != oldSN.接受發票金額大於開狀金額 && newSN.接受發票金額大於開狀金額.HasValue && oldSN.接受發票金額大於開狀金額.HasValue)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = " 統一發票金額大於開狀金額可接受：" + (true == newSN.接受發票金額大於開狀金額 ? "是" : "否"),
                    Original = " 統一發票金額大於開狀金額可接受。：" + (true == oldSN.接受發票金額大於開狀金額 ? "是" : "否")
                });
            }

            if (newSN.接受發票早於開狀日 != oldSN.接受發票早於開狀日 && newSN.接受發票早於開狀日.HasValue && oldSN.接受發票早於開狀日.HasValue)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = " 統一發票早於開狀日可接受：" + (true == newSN.接受發票早於開狀日 ? "是" : "否"),
                    Original = " 統一發票早於開狀日可接受：" + (true == oldSN.接受發票早於開狀日 ? "是" : "否")
                });
            }

            if (newSN.接受發票人地址與受益人地址不符 != oldSN.接受發票人地址與受益人地址不符 && newSN.接受發票人地址與受益人地址不符.HasValue && oldSN.接受發票人地址與受益人地址不符.HasValue)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = " 統一發票人地址與受益人地址不符可接受：" + (true == newSN.接受發票人地址與受益人地址不符 ? "是" : "否"),
                    Original = " 統一發票人地址與受益人地址不符可接受：" + (true == oldSN.接受發票人地址與受益人地址不符 ? "是" : "否")
                });
            }

            if (newSN.貨品明細以發票為準 != oldSN.貨品明細以發票為準 && newSN.貨品明細以發票為準.HasValue && oldSN.貨品明細以發票為準.HasValue)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = " 貨品明細以發票為準：" + (true == newSN.貨品明細以發票為準 ? "是" : "否"),
                    Original = " 貨品明細以發票為準：" + (true == oldSN.貨品明細以發票為準 ? "是" : "否")
                });
            }

            if (newSN.接受發票電子訊息 != oldSN.接受發票電子訊息 && newSN.接受發票電子訊息.HasValue && oldSN.接受發票電子訊息.HasValue)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = " 統一發票電子訊息可接受：" + (true == newSN.接受發票電子訊息 ? "是" : "否"),
                    Original = " 統一發票電子訊息可接受：" + (true == oldSN.接受發票電子訊息 ? "是" : "否")
                });
            }

            if (newSN.接受發票金額大於匯票金額 != oldSN.接受發票金額大於匯票金額 && newSN.接受發票金額大於匯票金額.HasValue && oldSN.接受發票金額大於匯票金額.HasValue)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = " 統一發票金額大於匯票金額可接受：" + (true == newSN.接受發票金額大於匯票金額 ? "是" : "否"),
                    Original = " 統一發票金額大於匯票金額可接受：" + (true == oldSN.接受發票金額大於匯票金額 ? "是" : "否")
                });
            }

            if (newSN.以發票收執聯或扣抵聯正本押匯 != oldSN.以發票收執聯或扣抵聯正本押匯 && newSN.以發票收執聯或扣抵聯正本押匯.HasValue && oldSN.以發票收執聯或扣抵聯正本押匯.HasValue)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = " 得以發票收執聯(或扣抵聯)任一聯正本押匯：" + (true == newSN.以發票收執聯或扣抵聯正本押匯 ? "是" : "否"),
                    Original = " 得以發票收執聯(或扣抵聯)任一聯正本押匯：" + (true == oldSN.以發票收執聯或扣抵聯正本押匯 ? "是" : "否")
                });
            }

            if (newSN.發票影本可接受 != oldSN.發票影本可接受 && newSN.發票影本可接受.HasValue && oldSN.發票影本可接受.HasValue)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = " 發票影本可接受(限扣抵聯或收執聯)：" + (true == newSN.發票影本可接受 ? "是" : "否"),
                    Original = " 發票影本可接受(限扣抵聯或收執聯)：" + (true == oldSN.發票影本可接受 ? "是" : "否")
                });
            }

            if (newSN.其他 != oldSN.其他)
            {
                items.Add(new LcAmendatory
                {
                    Amendatory = "其他特別指示條款：" + newSN.其他,
                    Original = "其他特別指示條款：" + oldSN.其他
                });
            }

            return items;
        }

        public static String BuildGoodsDetails(this LcItem item,String newLine = "\r\n")
        {
            StringBuilder srcGoods = new StringBuilder("貨物名稱：");
            int idx = 1;
            if (!String.IsNullOrEmpty(item.Goods))
            {
                srcGoods.Append(String.Format("{0}. 品名:{1} 規格:{2} 單價:{3} 數量:{4} 金額:{5} 備註:{6}"
                                , idx++, item.Goods, null, null, null, null, null))
                        .Append(newLine);
            }
            for (int i = 0; i < item.GoodsDetails.Count; i++)
            {
                var g = item.GoodsDetails[i];
                srcGoods.Append(String.Format("{0}. 品名:{1} 規格:{2} 單價:{3} 數量:{4} 金額:{5} 備註:{6}"
                            , idx++, g.ProductName, g.ProductSize, g.UnitPrice, g.Quantity, g.Amount, g.Remark))
                        .Append("\r\n");
            }

            return srcGoods.ToString();
        }


        public static bool CheckL3700Items(this AmendingLcApplication item, out LcItem newItem, out LcItem oldItem,
            out AttachableDocument newAttach, out AttachableDocument oldAttach,
            out SpecificNote newSN, out SpecificNote oldSN, out String description, out int itemNo, out String newNB, out String oldNB)
        {
            itemNo = 5;
            description = null;

            item.GetLcAmendmentItems(out newItem, out oldItem,
                out newAttach, out oldAttach,
                out newSN, out oldSN,
                out newNB, out oldNB);

            if (newItem.開狀金額 > oldItem.開狀金額)
                itemNo = 1;

            if (newItem.有效期限 > oldItem.有效期限)
            {
                itemNo = (itemNo == 5) ? 2 : 4;
            }

            //延長匯票期限
            if (newItem.定日付款 != oldItem.定日付款)
            {
                itemNo = (itemNo == 5) ? 3 : 4;
            }

            switch (itemNo)
            {
                case 0:
                    return false;
                case 1:
                    description = "01 增加信用狀金額";
                    return true;
                case 2:
                    description = "02 延長信用狀有效期限";
                    return true;
                case 3:
                    description = "03 延長匯票期限";
                    return true;
                case 4:
                    description = "04 同時修改多項";
                    return true;
                case 5:
                    description = "05 修改其他項";
                    return false;
            }
            return false;

        }

        public static bool CheckL4500Items(this LcItem newItem, LcItem oldItem)
        {
            return newItem.開狀金額 < oldItem.開狀金額;
        }

        public static StringBuilder BuildGoodsContent(this LcItem item,String format,String delimiter, StringBuilder content = null)
        {
            if (content == null)
                content = new StringBuilder();

            if (format == null)
                format = "{0}. 品名:{1} 規格:{2} 單價:{3} 數量:{4} 金額:{5} 備註:{6}";

            int idx = 1;
            if (!String.IsNullOrEmpty(item.Goods))
            {
                content.Append(String.Format(format
                                , idx++, item.Goods, null, null, null, null, null));
                if (delimiter != null)
                    content.Append(delimiter);
            }
            for (int i = 0; i < item.GoodsDetails.Count; i++)
            {
                var g = item.GoodsDetails[i];
                content.Append(String.Format(format
                            , idx++, g.ProductName, g.ProductSize, g.UnitPrice, g.Quantity, g.Amount, g.Remark));
                if (delimiter != null)
                    content.Append(delimiter);
            }
            return content;
        }

        public static bool CheckL5300Items(this LcItem newItem, LcItem oldItem,
            SpecificNote newSN, SpecificNote oldSN, String newNotifyingBank, String oldNotifyingBank)
        {
            return newItem.BuildGoodsContent("{0}{1}{2}{3}{4}{5}{6}", null).ToString()
                != oldItem.BuildGoodsContent("{0}{1}{2}{3}{4}{5}{6}", null).ToString()
                || newSN.原留印鑑相符 != oldSN.原留印鑑相符
                || newSN.受益人單獨蓋章 != oldSN.受益人單獨蓋章
                || newSN.分批交貨 != oldSN.分批交貨
                || newSN.最後交貨日 != oldSN.最後交貨日
                || newSN.接受發票早於開狀日 != oldSN.接受發票早於開狀日
                || newSN.接受發票金額大於開狀金額 != oldSN.接受發票金額大於開狀金額
                || newSN.接受發票人地址與受益人地址不符 != oldSN.接受發票人地址與受益人地址不符
                || newSN.押匯起始日 != oldSN.押匯起始日
                || newSN.押匯發票起始日 != oldSN.押匯發票起始日
                || newSN.其他 != oldSN.其他
                || newNotifyingBank != oldNotifyingBank;
        }

        public static void DoApprove(this Documentary item, Naming.DocumentLevel toLevel, String approver, String memo)
        {
            DateTime now = DateTime.Now;
            item.DocumentaryAllowance.Add(
                new DocumentaryAllowance
                {
                    Approver = approver,
                    ApprovalDate = now,
                    Memo = memo
                });

            item.DocumentaryLevel.Add(new DocumentaryLevel
            {
                DocLevel = (int)toLevel,
                LevelDate = now
            });

            item.CurrentLevel = (int)toLevel;
        }

        public static void DoDeny(this Documentary item, Naming.DocumentLevel denyLevel, String denier, String rejectReason)
        {
            DateTime now = DateTime.Now;
            item.DocumentaryDenial.Add(
                new DocumentaryDenial
                {
                    DenialDate = now,
                    Denier = denier,
                    Reason = rejectReason
                });

            item.DocumentaryLevel.Add(new DocumentaryLevel
            {
                DocLevel = (int)denyLevel,
                LevelDate = now
            });

            item.CurrentLevel = (int)denyLevel;
        }

        public static String GetGoodsDescription(this LcItem item,int length = 80)
        {
            StringBuilder goodsDesc = new StringBuilder();
            goodsDesc.Append(item.Goods);
            if (item.GoodsDetails.Count > 0)
            {
                goodsDesc.Append(String.Join("\r\n",
                    item.GoodsDetails.OrderBy(g => g.sno).Select(g =>
                        String.Concat($"品名:{g.ProductName}",
                            String.IsNullOrEmpty(g.ProductSize) ? "" : $"規格:{g.ProductSize}",
                            String.IsNullOrEmpty(g.UnitPrice) ? "" : $"單價:{g.UnitPrice}",
                            String.IsNullOrEmpty(g.Quantity) ? "" : $"數量:{g.Quantity}",
                            String.IsNullOrEmpty(g.Amount) ? "" : $"金額:{g.Amount}",
                            String.IsNullOrEmpty(g.Remark) ? "" : $"備註:{g.Remark}")).ToArray()));
            }
            return goodsDesc.ToString().GetEfficientStringMaxSize(0, length);
        }

        public static bool ForFpgService(this GenericManager<LcEntityDataContext> mgr, int docID, BeneficiaryServiceGroup.ServiceDefinition? serviceID = null)
        {
            var item = mgr.GetTable<Documentary>().Where(d => d.DocID == docID).FirstOrDefault();
            if (item != null)
            {
                return item.ForFpgService(serviceID);
            }
            return false;
        }

        public static bool ForFpgService(this Documentary item, BeneficiaryServiceGroup.ServiceDefinition? serviceID = null)
        {
            switch ((Naming.DocumentTypeDefinition)item.DocType)
            {
                case Naming.DocumentTypeDefinition.開狀申請書:
                    return item.CreditApplicationDocumentary.ForFpgService(serviceID);
                case Naming.DocumentTypeDefinition.修狀申請書:
                    return item.AmendingLcApplication.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.ForFpgService(serviceID);
                case Naming.DocumentTypeDefinition.押匯申請書:
                    return item.NegoDraft.ForFpgService(serviceID);
                case Naming.DocumentTypeDefinition.信用狀註銷申請書:
                    return item.CreditCancellation.LetterOfCredit.CreditApplicationDocumentary.ForFpgService(serviceID);
            }
            return false;
        }

        public static bool ForService(this Documentary item, BeneficiaryServiceGroup.ServiceDefinition? serviceID = null)
        {
            switch ((Naming.DocumentTypeDefinition)item.DocType)
            {
                case Naming.DocumentTypeDefinition.開狀申請書:
                    return item.CreditApplicationDocumentary.ForService(serviceID);
                case Naming.DocumentTypeDefinition.修狀申請書:
                    return item.AmendingLcApplication.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.ForService(serviceID);
                case Naming.DocumentTypeDefinition.押匯申請書:
                    return item.NegoDraft.ForFpgService(serviceID);
                case Naming.DocumentTypeDefinition.信用狀註銷申請書:
                    return item.CreditCancellation.LetterOfCredit.CreditApplicationDocumentary.ForService(serviceID);
            }
            return false;
        }


        public static bool ForFpgService(this CreditApplicationDocumentary item, BeneficiaryServiceGroup.ServiceDefinition? serviceID = null)
        {
            return item.FpgLcItem != null && (!serviceID.HasValue || item.BeneficiaryData.Organization.OrganizationStatus.BeneficiaryGroup.ServiceID == (int)serviceID);
        }

        public static bool ForService(this CreditApplicationDocumentary item, BeneficiaryServiceGroup.ServiceDefinition? serviceID = null)
        {
            return item.BeneficiaryData.Organization.ForService(serviceID);
        }

        public static bool ForService(this Organization item, BeneficiaryServiceGroup.ServiceDefinition? serviceID = null)
        {
            return item.OrganizationStatus?.BeneficiaryGroup?.ServiceID.HasValue == true 
                        && (!serviceID.HasValue || item.OrganizationStatus.BeneficiaryGroup.ServiceID == (int)serviceID);
        }


        public static bool ForFpgService(this BeneficiaryData item, BeneficiaryServiceGroup.ServiceDefinition? serviceID = null)
        {
            return item.Organization.ForFpgService(serviceID);
        }

        public static bool ForService(this BeneficiaryData item, BeneficiaryServiceGroup.ServiceDefinition? serviceID = null)
        {
            return item.Organization.ForService(serviceID);
        }


        public static bool ForFpgService(this Organization item, BeneficiaryServiceGroup.ServiceDefinition? serviceID = null)
        {
            return item.OrganizationStatus != null && item.OrganizationStatus.FpgNegoBeneficiary == true
                        && (!serviceID.HasValue || item.OrganizationStatus.BeneficiaryGroup.ServiceID==(int)serviceID);
        }


        public static bool ForFpgService(this NegoDraft item, BeneficiaryServiceGroup.ServiceDefinition? serviceID = null)
        {
            return item.LcID.HasValue && item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.ForFpgService(serviceID);
        }

        public static bool ForService(this NegoDraft item, BeneficiaryServiceGroup.ServiceDefinition? serviceID = null)
        {
            return item.LcID.HasValue
                ? item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.BeneficiaryData.Organization.ForService(serviceID)
                : item.NegoLC.BeneficiaryData.Organization.ForService(serviceID);
        }

        public static bool ForFpgService(this AmendingLcApplication item, BeneficiaryServiceGroup.ServiceDefinition? serviceID = null)
        {
            return item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.FpgLcItem != null
                        && (!serviceID.HasValue || item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.BeneficiaryData.Organization.OrganizationStatus.BeneficiaryGroup.ServiceID == (int)serviceID);
        }

        public static bool ForService(this AmendingLcApplication item, BeneficiaryServiceGroup.ServiceDefinition? serviceID = null)
        {
            return item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.BeneficiaryData.Organization.ForService(serviceID);
        }


        public static String CheckDraftNo(this NegoDraft item)
        {
            if (item.ForFpgService())
                return item.AppNo();
            else
                return item.DraftNo;
        }

        public static String GetApprovalMemo(this Documentary item, Naming.DocumentLevel docLevel)
        {
            var levelItem = item.DocumentaryLevel.Where(l => l.DocLevel == (int?)docLevel)
                .OrderByDescending(l => l.LevelDate)
                .FirstOrDefault();
            if(levelItem!=null)
            {
                var approval = item.DocumentaryAllowance.Where(a => a.ApprovalDate == levelItem.LevelDate).FirstOrDefault();
                if (approval != null)
                    return approval.Memo;
            }
            return null;
        }

        public static String GetAccount(this String[] accountField)
        {
            if (accountField == null)
            {
                return null;
            }

            for (int i = 0; i < accountField.Length; i++)
            {
                accountField[i] = accountField[i].GetEfficientString();
                if (accountField[i] == null || !Regex.IsMatch(accountField[i], "^\\d+$"))
                    return null;
            }

            return String.Join("-", accountField);
        }

        public static String[] SetAccountField(this String account, params int[] segment)
        {
            account = account.GetEfficientString();
            if (account == null)
                return null;

            if (segment == null || segment.Length < 1)
            {
                return account.Split('-');
            }

            account = account.Replace("-", "");
            int idx = 0;
            String getSegment(int length)
            {
                String seg = (idx + length) > account.Length ? null : account.Substring(idx, length);
                idx += length;
                return seg;
            }
            var result = segment.Select(s => getSegment(s)).ToArray();
            return result;
        }

        public static bool ForReserved(this Documentary item)
        {
            return item.DesiredDate.HasValue && item.DesiredDate > DateTime.Today;
        }


    }

    public partial class LcEntityManager<TEntity> : GenericManager<LcEntityDataContext, TEntity>
        where TEntity : class,new()
    {
        public LcEntityManager() : base() { }
        public LcEntityManager(GenericManager<LcEntityDataContext> manager) : base(manager) { }

        //public void AbortTransaction(int docID, String approver)
        //{

        //    var item = this.GetTable<Documentary>().Where(d => d.DocID == docID).FirstOrDefault();

        //    if (item != null)
        //    {
        //        var now = DateTime.Now;
        //        item.DocumentaryAllowances.Add(new DocumentaryAllowance
        //        {
        //            Approver = approver,
        //            ApprovalDate = now
        //        });

        //        item.DocumentaryLevels.Add(new DocumentaryLevel { 
        //            DocLevel = (int)Naming.DocumentLevel.待註記,
        //            LevelDate = now
        //        });

        //        item.CurrentLevel = (int)Naming.DocumentLevel.待註記;

        //        this.SubmitChanges();
        //    }
        //}

        public LcEntityDataContext Context
        {
            get
            {
                return (LcEntityDataContext)this._db;
            }
        }
    }

    public partial class LcAmendatory
    {
        public String Amendatory { get; set; }
        public String Original { get; set; }
    }

}
