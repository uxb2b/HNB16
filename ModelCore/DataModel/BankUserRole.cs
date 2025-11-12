using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class BankUserRole
{
    public string RoleID { get; set; }

    public string RoleName { get; set; }

    public string Memo { get; set; }

    public virtual ICollection<BankUser> BankUser { get; set; } = new List<BankUser>();
}
