using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class BankUserDeputy
{
    public int USER_ID { get; set; }

    public string DEPUTY_ID { get; set; }

    public int? DEPUTED { get; set; }

    public virtual BankUser DEPUTY { get; set; }

    public virtual BankUser USER { get; set; }
}
