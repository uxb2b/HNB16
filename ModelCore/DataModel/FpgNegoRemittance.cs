using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class FpgNegoRemittance
{
    public int FpgNegoDraftID { get; set; }

    public DateTime? RemittanceDate { get; set; }

    public int? Status { get; set; }

    public virtual FpgNegoDraft FpgNegoDraft { get; set; }

    public virtual ICollection<FpgNegoRemittanceLog> FpgNegoRemittanceLog { get; set; } = new List<FpgNegoRemittanceLog>();
}
