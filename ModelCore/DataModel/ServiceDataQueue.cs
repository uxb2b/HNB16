using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class ServiceDataQueue
{
    public int DataPortLogID { get; set; }

    public int? ServiceID { get; set; }

    public virtual DataPortLog DataPortLog { get; set; }

    public virtual BeneficiaryServiceGroup Service { get; set; }
}
