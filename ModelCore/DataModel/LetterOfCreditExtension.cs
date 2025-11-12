using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class LetterOfCreditExtension
{
    public int LcID { get; set; }

    public int? Status { get; set; }

    public int? AvailableAmt { get; set; }

    public int? GuaranteeAmount { get; set; }

    public int? ReturnGuaranteeAmount { get; set; }

    public virtual LetterOfCredit Lc { get; set; }
}
