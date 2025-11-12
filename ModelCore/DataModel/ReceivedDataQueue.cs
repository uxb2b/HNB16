using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class ReceivedDataQueue
{
    public int DataPortLogID { get; set; }

    public virtual DataPortLog DataPortLog { get; set; }
}
