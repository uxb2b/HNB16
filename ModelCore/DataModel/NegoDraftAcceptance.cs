using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class NegoDraftAcceptance
{
    public int AcceptanceID { get; set; }

    public int DraftID { get; set; }

    public int? L4700ID { get; set; }

    public virtual Documentary Acceptance { get; set; }

    public virtual NegoDraft Draft { get; set; }
}
