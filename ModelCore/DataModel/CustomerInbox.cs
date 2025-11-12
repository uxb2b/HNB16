using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class CustomerInbox
{
    public long MsgID { get; set; }

    public string ReceiptNo { get; set; }

    public int? TypeID { get; set; }

    public int DocumentaryID { get; set; }

    public DateTime? MsgDate { get; set; }

    public int? CompanyID { get; set; }

    public virtual Organization Company { get; set; }

    public virtual Documentary Documentary { get; set; }
}
