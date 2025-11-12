using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class NegoHost
{
    public int HostID { get; set; }

    public string HostUrl { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<NegoPrompt> NegoPrompt { get; set; } = new List<NegoPrompt>();
}
