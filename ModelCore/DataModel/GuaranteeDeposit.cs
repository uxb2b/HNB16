using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class GuaranteeDeposit
{
    public int DepositID { get; set; }

    public int? AppID { get; set; }

    public int? LcID { get; set; }

    public int? CancellationID { get; set; }

    public int? DraftID { get; set; }

    public string LcaNo { get; set; }

    public string LcNo { get; set; }

    public string LcdNo { get; set; }

    public int? UnreturnGuaranteeAmount { get; set; }

    public int? ReturnGuaranteeAmount { get; set; }

    public string ReturnAccountNo { get; set; }

    public string Fidacc { get; set; }

    public string Fiadcno { get; set; }

    public int? Fiadcamt { get; set; }

    public string Fiadcmo { get; set; }

    public int? AdjId { get; set; }

    public int? Status { get; set; }

    public DateOnly? FiaDate { get; set; }

    public string FiaTxseq { get; set; }

    public string RefuseId { get; set; }

    public string RefuseName { get; set; }

    public string AuditOfficer { get; set; }

    public string AuditManager { get; set; }

    public int? Version { get; set; }

    public DateOnly? ApplyDate { get; set; }

    public string DraftNo { get; set; }

    public int? LcProcedureExpense { get; set; }

    public string LcexpPaymentWay { get; set; }

    public string LcexpTransferAccountNo { get; set; }

    public string RejectReason { get; set; }

    public string LcexpCheckNo { get; set; }

    public string Fiaacc { get; set; }

    public string Fiaacno { get; set; }

    public int? Fiaacamt { get; set; }

    public string Fiaacmo { get; set; }

    public string LcStampTaxFlag { get; set; }

    public string FoaOsq { get; set; }

    public DateTime? Starttime { get; set; }

    public virtual CreditApplicationDocumentary App { get; set; }

    public virtual CreditCancellation Cancellation { get; set; }

    public virtual NegoDraft Draft { get; set; }

    public virtual LetterOfCredit Lc { get; set; }
}
