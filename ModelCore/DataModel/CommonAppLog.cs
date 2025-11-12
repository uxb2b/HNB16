using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class CommonAppLog
{
    public int LogID { get; set; }

    public DateTime? LOG_TIME { get; set; }

    public string LOG_SEQ { get; set; }

    public string USER_PID { get; set; }

    public string ACTION_CATEGORY { get; set; }

    public string ACTION_RESULT { get; set; }

    public string ACTION_MESSAGE { get; set; }

    public virtual ApLogCategory ACTION_CATEGORYNavigation { get; set; }

    public virtual BankUser USER_P { get; set; }
}
