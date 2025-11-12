using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class FpgNegoRemittanceLog
{
    public int RemittanceID { get; set; }

    public int FpgNegoRemittanceID { get; set; }

    public int DataPortLogID { get; set; }

    public string BatchNo { get; set; }

    public int? SeqNo { get; set; }

    public string DPMTID { get; set; }

    public int? Status { get; set; }

    public string Description { get; set; }

    /// <summary>
    /// 押匯金額
    /// </summary>
    public decimal? Amount { get; set; }

    public virtual DataPortLog DataPortLog { get; set; }

    public virtual FpgNegoRemittance FpgNegoRemittance { get; set; }

    public virtual FpgNegoRemittanceDispatch FpgNegoRemittanceDispatch { get; set; }
}
