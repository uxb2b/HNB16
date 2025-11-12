using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class ApLogCategory
{
    public string CODE_KEY { get; set; }

    public string CODE_VALUE { get; set; }

    public string MEMO1 { get; set; }

    public string MEMO2 { get; set; }

    public virtual ICollection<CommonAppLog> CommonAppLog { get; set; } = new List<CommonAppLog>();

    public virtual ICollection<CommonCALog> CommonCALog { get; set; } = new List<CommonCALog>();
}
