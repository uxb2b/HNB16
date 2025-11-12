using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class DataProcessLog
{
    public int ProcID { get; set; }

    public int DataPortLogID { get; set; }

    public DateTime ProcessDate { get; set; }

    public int? ExceptionLogID { get; set; }

    public string ProcessContent { get; set; }

    public virtual DataPortLog DataPortLog { get; set; }

    public virtual ExceptionLog ExceptionLog { get; set; }
}
