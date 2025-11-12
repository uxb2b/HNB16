using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class FpgNegoRemittanceDispatch
{
    public int FpgNegoRemittanceLogID { get; set; }

    public virtual FpgNegoRemittanceLog FpgNegoRemittanceLog { get; set; }
}
