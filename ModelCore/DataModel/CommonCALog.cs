using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class CommonCALog
{
    public int LogID { get; set; }

    public DateTime? LOG_TIME { get; set; }

    public string LOG_TYPE { get; set; }

    public string LOG_SEQ { get; set; }

    public string USER_PID { get; set; }

    public string LOG_CATEGORY { get; set; }

    public string LOG_MESSAGE { get; set; }

    public string LOG_DOC { get; set; }

    public string LOG_CIPHER { get; set; }

    public string LOG_RESULT { get; set; }

    public virtual ApLogCategory LOG_CATEGORYNavigation { get; set; }

    public virtual BankUser USER_P { get; set; }
}
