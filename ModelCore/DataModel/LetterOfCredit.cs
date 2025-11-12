using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class LetterOfCredit
{
    public int LcID { get; set; }

    public int? ApplicationID { get; set; }

    public string LcNo { get; set; }

    public decimal? 可用餘額 { get; set; }

    public DateTime LcDate { get; set; }

    public bool AppCountersign { get; set; }

    public int? PrintNotice { get; set; }

    public bool? 受益人匯票簽核認可 { get; set; }

    public string NotifyingBank { get; set; }

    public virtual CreditApplicationDocumentary Application { get; set; }

    public virtual ICollection<CreditCancellation> CreditCancellation { get; set; } = new List<CreditCancellation>();

    public virtual ICollection<GuaranteeDeposit> GuaranteeDeposit { get; set; } = new List<GuaranteeDeposit>();

    public virtual LetterOfCreditExtension LetterOfCreditExtension { get; set; }

    public virtual ICollection<LetterOfCreditVersion> LetterOfCreditVersion { get; set; } = new List<LetterOfCreditVersion>();

    public virtual NegoLC NegoLC { get; set; }

    public virtual BankData NotifyingBankNavigation { get; set; }
}
