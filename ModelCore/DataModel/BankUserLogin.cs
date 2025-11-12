using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class BankUserLogin
{
    public int USER_ID { get; set; }

    public DateTime LAST_LOGIN_TIME { get; set; }

    public virtual BankUser USER { get; set; }
}
