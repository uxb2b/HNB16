using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class BeneficiaryTransferInto
{
    public int AccountID { get; set; }

    public int BeneID { get; set; }

    public string BankCode { get; set; }

    public string AccountNo { get; set; }

    public int? Status { get; set; }

    public virtual BankData BankCodeNavigation { get; set; }

    public virtual Organization Bene { get; set; }
}
