using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class FpgBeneficiaryStatus
{
    public int BeneID { get; set; }

    public decimal? 押匯允差比例 { get; set; }

    public int? 押匯允差比例狀態 { get; set; }

    public string 押匯允差比例退回原因 { get; set; }

    public virtual BeneficiaryData Bene { get; set; }
}
