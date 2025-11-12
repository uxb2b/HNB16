using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class CreditLoan
{
    public int LoadID { get; set; }

    public int? AppID { get; set; }

    public int? AmendingID { get; set; }

    public string LcaNo { get; set; }

    public string LcrNo { get; set; }

    public int? FixedPricePointer { get; set; }

    public double? RaisedInterestRate { get; set; }

    public string LoanAccountId { get; set; }

    public string LcApprovedNo { get; set; }

    public string AcceptApprovedNo { get; set; }

    public int? LcQuota { get; set; }

    public DateOnly? QuotaExpiryDate { get; set; }

    public int? LcHostBalance { get; set; }

    public string LoanManagerId { get; set; }

    public int? LcProcedureExpense { get; set; }

    public int? AcceptProcedureExpense { get; set; }

    public int? GuaranteeAmountRate { get; set; }

    public int? GuaranteeAmount { get; set; }

    public int? AuthorizingDuductacFlag { get; set; }

    public int? Cash { get; set; }

    public string CheckTransferAccountNo1 { get; set; }

    public string CheckNo1 { get; set; }

    public int? CheckAmount1 { get; set; }

    public string CheckTransferAccountNo2 { get; set; }

    public string CheckNo2 { get; set; }

    public int? CheckAmount2 { get; set; }

    public string DemandDepositAccountNo { get; set; }

    public int? WithdrawalAmount1 { get; set; }

    public int? WithdrawalAmount2 { get; set; }

    public string GuaranteeCondition { get; set; }

    public double? LoanAccountNo { get; set; }

    public string LcStampTaxFlag { get; set; }

    public string AcceptStampTaxFlag { get; set; }

    public string RiskKind1 { get; set; }

    public int? RiskPercent1 { get; set; }

    public string RiskKind2 { get; set; }

    public int? RiskPercent2 { get; set; }

    public string RiskKind3 { get; set; }

    public int? RiskPercent3 { get; set; }

    public string RiskKind4 { get; set; }

    public int? RiskPercent4 { get; set; }

    public string Fiaacc { get; set; }

    public int? Fiaacno { get; set; }

    public int? Fiaacamt { get; set; }

    public string Fiaacmo { get; set; }

    public string ApproveManagerId1 { get; set; }

    public string ApproveManagerId2 { get; set; }

    public int? LoanRateMonths { get; set; }

    public int? LoanRateDays { get; set; }

    public int? LoanRatePlus { get; set; }

    public double? TotalInterestRate { get; set; }

    public string OtherAppointment { get; set; }

    public string MrateType { get; set; }

    public string RateMemo { get; set; }

    public string ReturnAccount { get; set; }

    public int? LimitAmount { get; set; }

    public int? AdvanceBalance { get; set; }

    public int? GeneralAmount { get; set; }

    public int? GeneralAdvanceAmount { get; set; }

    public string BatchInsuranceApprovedNo { get; set; }

    public string CheckAml { get; set; }

    public string SmegCase { get; set; }

    public string SmegNoticeno { get; set; }

    public int? AdjId { get; set; }

    public int? Status { get; set; }

    public DateOnly? FiaDate { get; set; }

    public string FiaTxseq { get; set; }

    public string FoaOsq { get; set; }

    public string RefuseId { get; set; }

    public string RefuseName { get; set; }

    public string AuditOfficer { get; set; }

    public string AuditManager { get; set; }

    public int? Version { get; set; }

    public DateTime? ApplyDate { get; set; }

    public virtual AmendingLcApplication Amending { get; set; }

    public virtual CreditApplicationDocumentary App { get; set; }
}
