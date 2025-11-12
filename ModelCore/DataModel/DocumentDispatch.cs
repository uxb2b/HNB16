using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class DocumentDispatch
{
    public int DocumentaryID { get; set; }

    public virtual Documentary Documentary { get; set; }
}
