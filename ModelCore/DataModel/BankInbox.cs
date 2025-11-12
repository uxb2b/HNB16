using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class BankInbox
{
    public long MsgID { get; set; }

    public string BankCode { get; set; }

    public int? TypeID { get; set; }

    public int? DocumentaryID { get; set; }

    public DateTime? MsgDate { get; set; }

    public string MsgContent { get; set; }

    public virtual BankData BankCodeNavigation { get; set; }

    public virtual Documentary Documentary { get; set; }
}
