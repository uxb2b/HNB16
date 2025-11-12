using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class ExceptionLog
{
    public int LogID { get; set; }

    public DateTime? LogTime { get; set; }

    public string Message { get; set; }

    public virtual ICollection<DataPortLog> DataPortLog { get; set; } = new List<DataPortLog>();

    public virtual ICollection<DataProcessLog> DataProcessLog { get; set; } = new List<DataProcessLog>();
}
