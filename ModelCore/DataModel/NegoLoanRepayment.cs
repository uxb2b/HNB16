using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

/// <summary>
/// 改貸還款申請
/// </summary>
public partial class NegoLoanRepayment
{
    public int RepaymentID { get; set; }

    /// <summary>
    /// 還款日期
    /// </summary>
    public DateTime? RepaymentDate { get; set; }

    /// <summary>
    /// 還款本金
    /// </summary>
    public decimal? RepaymentAmount { get; set; }

    /// <summary>
    /// 嘗還利息
    /// </summary>
    public decimal? InterestAmount { get; set; }

    /// <summary>
    /// 參考改貸來源
    /// </summary>
    public int LoanID { get; set; }

    /// <summary>
    /// EAI電文交易狀態記錄
    /// </summary>
    public int? TxnCode { get; set; }

    public virtual Documentary Repayment { get; set; }
}
