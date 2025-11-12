using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class DocumentProcessLog
{
    public int DocID { get; set; }

    public DateTime ProcessDate { get; set; }

    public int Status { get; set; }

    public virtual Documentary Doc { get; set; }
}
