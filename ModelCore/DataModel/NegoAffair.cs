using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class NegoAffair
{
    public int NegoDraftID { get; set; }

    public int NegoInvoiceID { get; set; }

    public decimal NegoAmount { get; set; }

    public virtual NegoDraft NegoDraft { get; set; }

    public virtual NegoInvoice NegoInvoice { get; set; }
}
