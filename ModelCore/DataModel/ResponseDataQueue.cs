using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class ResponseDataQueue
{
    public int LogID { get; set; }

    public int? ServiceID { get; set; }

    public virtual DataPortLog Log { get; set; }

    public virtual BeneficiaryServiceGroup Service { get; set; }
}
