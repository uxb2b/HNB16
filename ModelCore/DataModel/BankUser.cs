using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class BankUser
{
    public int USER_ID { get; set; }

    public string PID { get; set; }

    public string PASSWORD { get; set; }

    public string ROLE_ID { get; set; }

    public string USER_NAME { get; set; }

    public string TEL_NO { get; set; }

    public string EMAIL { get; set; }

    public int? LOGIN_COUNT { get; set; }

    public int? SUPERVISOR { get; set; }

    public string STATUS_CODE { get; set; }

    public int? AUDITOR_ROLE { get; set; }

    public virtual ICollection<BankUserBranch> BankUserBranch { get; set; } = new List<BankUserBranch>();

    public virtual ICollection<BankUserDeputy> BankUserDeputyDEPUTY { get; set; } = new List<BankUserDeputy>();

    public virtual BankUserDeputy BankUserDeputyUSER { get; set; }

    public virtual ICollection<BankUserLogin> BankUserLogin { get; set; } = new List<BankUserLogin>();

    public virtual ICollection<CommonAppLog> CommonAppLog { get; set; } = new List<CommonAppLog>();

    public virtual ICollection<CommonCALog> CommonCALog { get; set; } = new List<CommonCALog>();

    public virtual ICollection<BankUser> InverseSUPERVISORNavigation { get; set; } = new List<BankUser>();

    public virtual BankUserRole ROLE { get; set; }

    public virtual BankUser SUPERVISORNavigation { get; set; }
}
