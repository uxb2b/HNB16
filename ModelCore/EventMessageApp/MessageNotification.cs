using CommonLib.DataAccess;
using ModelCore.CommonManagement;
using ModelCore.DataModel;
using ModelCore.Helper;
using ModelCore.Locale;
using ModelCore.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ModelCore.EventMessageApp
{
    public static class MessageNotification
    {
        public static void CreateInboxMessage(this int docID, Naming.MessageTypeDefinition typeID, Naming.MessageReceipent receipent)
        {
            ThreadPool.QueueUserWorkItem(stateInfo =>
            {
                try
                {
                    using (CommonInboxManager inboxMgr = new CommonInboxManager())
                    {
                        switch (receipent)
                        {
                            case Naming.MessageReceipent.ForAll:
                                inboxMgr.CreateInboxForApplicant(docID, typeID);
                                inboxMgr.CreateInboxForBeneciary(docID, typeID);
                                inboxMgr.CreateInboxForBank(docID, typeID);
                                break;
                            case Naming.MessageReceipent.ForApplicant:
                                inboxMgr.CreateInboxForApplicant(docID, typeID);
                                break;
                            case Naming.MessageReceipent.ForApplicantAndBank:
                                inboxMgr.CreateInboxForApplicant(docID, typeID);
                                inboxMgr.CreateInboxForBank(docID, typeID);
                                break;
                            case Naming.MessageReceipent.ForBank:
                                inboxMgr.CreateInboxForBank(docID, typeID);
                                break;
                            case Naming.MessageReceipent.ForBeneficiary:
                                inboxMgr.CreateInboxForBeneciary(docID, typeID);
                                break;
                            case Naming.MessageReceipent.ForCustomer:
                                inboxMgr.CreateInboxForApplicant(docID, typeID);
                                inboxMgr.CreateInboxForBeneciary(docID, typeID);
                                break;

                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelCore.Helper.Logger.Error(ex);
                }

            }, null);
        }

        public static void CreateInboxMessage(this String content, String bankCodeOfReceipent, Naming.MessageTypeDefinition TID)
        {
            ThreadPool.QueueUserWorkItem(stateInfo =>
            {
                try
                {
                    using (CommonInboxManager inboxMgr = new CommonInboxManager())
                    {

                        inboxMgr.CreateInboxForBank(content, bankCodeOfReceipent, TID);
                    }
                }
                catch (Exception ex)
                {
                    ModelCore.Helper.Logger.Error(ex);
                }

            }, null);
        }
        public static Action<Exception> ExceptionAlert { get; set; }
        public static Action<String> ExceptionMessageAlert { get; set; }

        public static void CreateMailMessage(this GenericManager<LcEntityDataContext> mgr, int? docID, Naming.MessageTypeDefinition typeID, Naming.MessageReceipent receipent)
        {
            mgr.CreateMailMessage(docID, typeID, receipent, null);
        }

        public static void CreateMailMessage(this GenericManager<LcEntityDataContext> mgr, int? docID, Naming.MessageTypeDefinition typeID, Naming.MessageReceipent? receipent = null, String email = null)
        {
            String mailTo = null;
            ModelCore.DataModel.MessageType msgType = null;
            String subject;
            String attachmentContent = null;

            try
            {
                using (CommonInboxManager inboxMgr = new CommonInboxManager(mgr))
                {
                    msgType = inboxMgr.GetTable<ModelCore.DataModel.MessageType>().Where(m => m.TypeID == (int?)typeID).FirstOrDefault();
                    if (msgType == null)
                        subject = "國內電子信用狀訊息通知";
                    else
                        subject = msgType.Subject;

                    NegoDraft item = inboxMgr.GetTable<NegoDraft>().Where(d => d.DraftID == docID).FirstOrDefault();
                    if (item != null && item.ForFpgService())
                    {
                        attachmentContent = item.DraftContent;
                    }

                    switch (receipent)
                    {
                        case Naming.MessageReceipent.ForAll:
                            mailTo = inboxMgr.GetMailReceipentOfApplicant(docID)
                                .JoinEmail(inboxMgr.GetMailReceipentOfBank(docID),
                                inboxMgr.GetMailReceipentOfBeneficiary(docID));
                            break;
                        case Naming.MessageReceipent.ForApplicant:
                            mailTo = inboxMgr.GetMailReceipentOfApplicant(docID);
                            break;
                        case Naming.MessageReceipent.ForApplicantAndBank:
                            mailTo = inboxMgr.GetMailReceipentOfApplicant(docID)
                                .JoinEmail(inboxMgr.GetMailReceipentOfBank(docID));
                            break;
                        case Naming.MessageReceipent.ForBank:
                            mailTo = inboxMgr.GetMailReceipentOfBank(docID);
                            break;
                        case Naming.MessageReceipent.ForBeneficiary:
                            mailTo = inboxMgr.GetMailReceipentOfBeneficiary(docID);
                            break;
                        case Naming.MessageReceipent.ForCustomer:
                            mailTo = inboxMgr.GetMailReceipentForCustomer(docID);
                            break;
                    }
                }

                mailTo = email.JoinEmail(mailTo);

                if (mailTo == null)
                    return;

                ThreadPool.QueueUserWorkItem(stateInfo =>
                {
                    String url = $"{AppSettings.Default.MailMessageUrl}?msgDocID={docID}&typeID={(int)typeID}";
                    try
                    {
                        using (GenericManager<LcEntityDataContext> models = new GenericManager<LcEntityDataContext>())
                        {
                            var docItem = models.GetTable<Documentary>().Where(d => d.DocID == docID).FirstOrDefault();

                            if (attachmentContent == null)
                            {
                                url.MailWebPage(mailTo, subject, null, docItem);
                            }
                            else
                            {
                                MailWebPage(url, mailTo, subject, null, docItem, attachmentContent);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelCore.Helper.Logger.Error(String.Format("URL Error:{0}\r\n{1}", url, ex));
                    }

                }, null);

            }
            catch (Exception ex)
            {
                ModelCore.Helper.Logger.Error(ex);
            }

        }

        public static void MailWebPage(this String url, String mailTo, String subject, String content = null, Documentary item = null, String attachmentContent = null)
        {
            MailWebPageBySmtp(url, mailTo, subject);
        }

        public static void MailWebPageBySmtp(String url, String mailTo, String subject)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(AppSettings.Default.WebMaster);
            message.To.Add(mailTo);
            message.Subject = subject;
            message.IsBodyHtml = true;
            String contentLocation = AppSettings.Default.ApplicationUrl;
            message.Headers.Add("Content-Location", contentLocation);
            message.Headers.Add("Content-Base", contentLocation);

            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                message.Body = wc.DownloadString(url);
            }

            SmtpClient smtpclient = new SmtpClient(AppSettings.Default.SmtpServer);
            smtpclient.Credentials = CredentialCache.DefaultNetworkCredentials;
            smtpclient.Send(message);
        }

    }
}
