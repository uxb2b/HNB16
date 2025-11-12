using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class CreditCancellationInfo
{
    public int CancellationID { get; set; }

    public DateTime? CancellationDate { get; set; }

    public virtual CreditCancellation Cancellation { get; set; }
}
