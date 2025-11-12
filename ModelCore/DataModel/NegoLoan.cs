using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class NegoLoan
{
    public int LoanID { get; set; }

    public int? DraftID { get; set; }

    public int? DraftNo { get; set; }

    public int? Status { get; set; }

    public int? LoanAmount { get; set; }

    public int? InterestRateWay { get; set; }

    public double? LoanInterestRate { get; set; }

    public string OtherAppointment { get; set; }

    public string RejectReason { get; set; }

    public string FiaDate { get; set; }

    public string FiaTxseq { get; set; }

    public DateTime? ConfirmDate { get; set; }

    public int? LoanRateTotal { get; set; }

    public int? LoanRateDays { get; set; }

    public int? LoanRatePlus { get; set; }

    public int? LoanRateMonths { get; set; }

    public int? MrateType { get; set; }

    public string RateMemo { get; set; }

    public string RejectDate { get; set; }

    public virtual NegoDraft Draft { get; set; }
}
