using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class NegoDraftDeal
{
    public int DraftID { get; set; }

    public string AcceptanceBranch { get; set; }

    public virtual BankData AcceptanceBranchNavigation { get; set; }

    public virtual NegoDraft Draft { get; set; }
}
