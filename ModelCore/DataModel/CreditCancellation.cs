using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class CreditCancellation
{
    public int DocumentaryID { get; set; }

    public int? RegistrationID { get; set; }

    /// <summary>
    /// 參考註銷登錄(L4500)
    /// </summary>
    public string 註銷申請號碼 { get; set; }

    /// <summary>
    /// 參考註銷信用狀來源
    /// </summary>
    public int LcID { get; set; }

    public DateTime? 申請日期 { get; set; }

    /// <summary>
    /// Deprecated
    /// </summary>
    public string status { get; set; }

    /// <summary>
    /// Deprecated
    /// </summary>
    public string SysDocID { get; set; }

    /// <summary>
    /// CDS註申請之申請書檔名
    /// </summary>
    public string FileName { get; set; }

    public virtual CreditCancellationInfo CreditCancellationInfo { get; set; }

    public virtual Documentary Documentary { get; set; }

    public virtual ICollection<GuaranteeDeposit> GuaranteeDeposit { get; set; } = new List<GuaranteeDeposit>();

    public virtual LetterOfCredit Lc { get; set; }
}
