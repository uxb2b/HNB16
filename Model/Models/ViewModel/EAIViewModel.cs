using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelCore.DataModel;

namespace ModelCore.Models.ViewModel
{
    public interface IInterestRateQueryViewModel  
    {
        String InterestKind { get; set; }
        String InterestAttribute { get; set; }
        String InterestType { get; set; }
        decimal? ABSIncInterestRate { get; set; }
        String PlusMinus
        {
            get;
            set;
        }
        decimal? InterestRateOfBank { get; set; }
    }

    public class L4500ViewModel : QueryViewModel
    {
        public String AccountNo { get; set; }
        public String BranchNo { get; set; }
        public String CancellationReason { get; set; }
        public decimal? IncLcAmt { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? Security { get; set; }
        public String IncomingAccount
        {
            get => IncomingAccountField.GetAccount();
            set => IncomingAccountField = value.SetAccountField();
        }
        public String[] IncomingAccountField { get; set; }
        public PaymentNotification.AllocationType? AllocationType { get; set; }
        public long? GTXNO { get; set; }
        public decimal? ServiceCharge { get; set; }
        public decimal? ServiceChargeRate { get; set; }
        public decimal? AdvanceRate { get; set; }
    }
}
