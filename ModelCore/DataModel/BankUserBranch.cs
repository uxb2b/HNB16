using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class BankUserBranch
{
    public string BRANCH_ID { get; set; }

    public int USER_ID { get; set; }

    public int? GROUP_ID { get; set; }

    public int? CAN_AUDIT { get; set; }

    public int? NOTIFY { get; set; }

    public virtual BankData BRANCH { get; set; }

    public virtual BankUser USER { get; set; }
}
