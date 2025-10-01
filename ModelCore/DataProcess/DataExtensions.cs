using CommonLib.DataAccess;
using ModelCore.DataModel;
using ModelCore.Locale;

using System;
using System.Linq;
using System.Linq.Expressions;
using EAI.Service.Transaction;
using CommonLib.Utility;
using System.Xml;
using ModelCore.Properties;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

using System.Collections.Generic;
using CommonLib.Helper;
using CommonLib.Core.Utility;
using ModelCore.Models.ViewModel;

namespace ModelCore.DataProcess
{
    public static class DataExtensions
    {
        public static Reimbursement BuildDataItem(this Reimbursement item, GenericManager<LcEntityDataContext> models, ReimbursementQueryViewModel viewModel, NegoDraft draftItem, Naming.DocumentLevel initLevel = Naming.DocumentLevel.文件預覽)
        {
            bool newItem = false;
            if (item == null)
            {
                DateTime now = DateTime.Now;

                item = new Reimbursement
                {
                    NegoDraft = draftItem,
                    Documentary = new Documentary
                    {
                        CurrentLevel = (int)initLevel,
                        DocDate = DateTime.Now,
                        DocType = (int)Naming.DocumentTypeDefinition.還款改貸申請書
                    }
                };

                item.Documentary.DocumentaryLevel.Add(new DocumentaryLevel
                {
                    DocLevel = (int)initLevel,
                    LevelDate = now
                });

                models.GetTable<Reimbursement>().InsertOnSubmit(item);
                newItem = true;
            }
            else
            {

            }

            item.Amount = draftItem.Amount;
            item.PayableAccount = draftItem.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.CustomerOfBranchExtension.約定還款帳號;

            if (viewModel.HasLoan != true)
            {
                item.ReimbursementNo = String.Format("R{0:000}{1:00000}", DateTime.Today.Year - 1911,
                    models.GetTable<Reimbursement>()
                        .Where(r => r.Documentary.DocDate >= new DateTime(DateTime.Today.Year, 1, 1)).Count() + 1);

                if (!newItem)
                    models.DeleteAllOnSubmit<NegoLoan>(n => n.LoanID == item.ReimID);
            }
            else
            {
                item.ReimbursementNo = String.Format("L{0:000}{1:00000}", DateTime.Today.Year - 1911,
                    models.GetTable<Reimbursement>()
                        .Where(r => r.Documentary.DocDate >= new DateTime(DateTime.Today.Year, 1, 1)).Count() + 1);

                NegoLoan loan = item.NegoLoan;
                if (loan == null)
                {
                    loan = item.NegoLoan = new NegoLoan
                    {

                    };

                    CustomerOfBranch cust = draftItem.LetterOfCreditVersion.LetterOfCredit?.CreditApplicationDocumentary.CustomerOfBranch
                        ?? draftItem.NegoLC?.CustomerOfBranch;

                    CustomerOfBranchExtension extension = cust?.CustomerOfBranchExtension;
                    if (extension != null)
                    {
                        if (extension.UseCreditInsurance == true)
                        {
                            loan.CreditInsuranceLoan = new CreditInsuranceLoan
                            {
                                LoanPercentage = cust.CustomerOfBranchExtension.CreditPercentage,
                                LoanAmount = draftItem.Amount * cust.CustomerOfBranchExtension.CreditPercentage / 100m,
                            };
                            loan.UseCreditInsurance = true;
                            if (!extension.ChristeningDate.HasValue)
                            {
                                extension.ChristeningDate = DateTime.Now;
                            }
                        }
                    }
                }

                loan.InitLoanDate = viewModel.InitLoanDate;
                loan.LoanDay = viewModel.LoanDay;
                loan.LoanInterestRate = viewModel.LoanInterestRate;

            }
            models.SubmitChanges();
            return item;
        }

        public static NegoLoanRepayment BuildDataItem(this NegoLoanRepayment item, GenericManager<LcEntityDataContext> models, NegoLoanRepaymentQueryViewModel viewModel, NegoLoan loan, Naming.DocumentLevel initLevel = Naming.DocumentLevel.文件預覽)
        {
            if (item == null)
            {
                DateTime now = DateTime.Now;

                item = new NegoLoanRepayment
                {
                    NegoLoan = loan,
                    Documentary = new Documentary
                    {
                        CurrentLevel = (int)initLevel,
                        DocDate = DateTime.Now,
                        DocType = (int)Naming.DocumentTypeDefinition.改貸還款
                    }
                };

                item.Documentary.DocumentaryLevel.Add(new DocumentaryLevel
                {
                    DocLevel = (int)initLevel,
                    LevelDate = now
                });

                models.GetTable<NegoLoanRepayment>().InsertOnSubmit(item);
            }

            item.RepaymentAmount = viewModel.RepaymentAmount;
            item.RepaymentDate = viewModel.RepaymentDate;
            item.InterestAmount = viewModel.InterestAmount ?? 0;

            models.SubmitChanges();
            return item;
        }

    }
}
