using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class DocumentOwner
{
    public int DocID { get; set; }

    public int OwnerID { get; set; }

    public virtual Documentary Doc { get; set; }

    public virtual Organization Owner { get; set; }
}
