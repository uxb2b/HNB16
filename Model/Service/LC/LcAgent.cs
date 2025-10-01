using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using ModelCore.NegoManagement;
using CommonLib.Utility;
using ModelCore.LcManagement;
using EAI.Service.Transaction;
using ModelCore.DataModel;
using ModelCore.Locale;
using ModelCore.EventMessageApp;
using ModelCore.Properties;
using ModelCore.BankManagement;

namespace ModelCore.Service.LC
{
    public static class LcAgent
    {
        private static int __CountToCheckCheckCancellation = 0;

        static LcAgent()
        {

        }

        public static void StartUp()
        {

        }

        public static void CreateActiveCancellation()
        {
            if (Interlocked.Increment(ref __CountToCheckCheckCancellation) == 1)
            {
                ThreadPool.QueueUserWorkItem(stateInfo =>
                {
                    do
                    {
                        try
                        {
                            doCheckLc();
                        }
                        catch (Exception ex)
                        {
                            ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                        }

                    } while (Interlocked.Decrement(ref __CountToCheckCheckCancellation) > 0);
                });
            }

        }

        private static void doCheckLc()
        {
            using (LcManager mgr = new LcManager())
            {
                decimal availableAmt;
                //				decimal dblAvailableAmt;
                var items = mgr.CheckReadyToCancelLc();
                foreach (var item in items)
                {
                    //LR017-->保證金餘額,信用狀餘額
                    Txn_LR017 txn = new Txn_LR017();
                    txn.Account = item.LcNo.Replace("-", "").Trim();
                    availableAmt = 0;

                    if (txn.Commit())
                    {
                        availableAmt = decimal.Parse(txn.SQBAL);
                        item.可用餘額 = availableAmt;
                        if (availableAmt <= 0)
                            item.CreditApplicationDocumentary.Documentary.DoApprove(Naming.DocumentLevel.信用狀餘額為零, Settings.Default.SystemID, null);
                        mgr.SubmitChanges();
                    }
                    else if ("XL215".Equals(txn.RspCode))
                    {
                        availableAmt = 0;
                        item.可用餘額 = 0;
                        item.CreditApplicationDocumentary.Documentary.DoApprove(Naming.DocumentLevel.信用狀餘額為零, Settings.Default.SystemID, null);
                        mgr.SubmitChanges();
                    }
                    else
                    {
                        availableAmt = item.可用餘額.HasValue ? item.可用餘額.Value : 0;
                    }


                    if (availableAmt > 0)
                    {
                        var cancellation = mgr.CreateActiveCancellation(item);
                        //傳送email及訊息匣訊息
                        BankManager.DoActiveCancellationR3801(cancellation.CancellationID);
                        MessageNotification.CreateInboxMessage(cancellation.CancellationID, Naming.MessageTypeDefinition.MSG_CANCELLATION_UNASKED, Naming.MessageReceipent.ForApplicantAndBank);
                        MessageNotification.CreateMailMessage(mgr,cancellation.CancellationID, Naming.MessageTypeDefinition.MSG_CANCELLATION_UNASKED, Naming.MessageReceipent.ForApplicantAndBank);
                    }
                }
            }

        }
    }
}
