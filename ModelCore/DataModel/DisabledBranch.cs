using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class DisabledBranch
{
    public string BankCode { get; set; }

    public virtual BankData BankCodeNavigation { get; set; }
}
