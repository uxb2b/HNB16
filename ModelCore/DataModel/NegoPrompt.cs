using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class NegoPrompt
{
    public int PromptID { get; set; }

    public DateTime ImportDate { get; set; }

    public string PromptContent { get; set; }

    public int? HostID { get; set; }

    public int? LogID { get; set; }

    public virtual NegoHost Host { get; set; }

    public virtual DataPortLog Log { get; set; }

    public virtual ICollection<NegoDraft> NegoDraft { get; set; } = new List<NegoDraft>();
}
