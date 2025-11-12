using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class AttachableDocument
{
    public int AttachmentID { get; set; }

    public bool? 匯票付款申請書 { get; set; }

    public bool? 匯票承兌申請書 { get; set; }

    public bool? 統一發票 { get; set; }

    public string 其他 { get; set; }

    public bool? 電子發票證明聯 { get; set; }

    public virtual ICollection<AmendingLcApplication> AmendingLcApplication { get; set; } = new List<AmendingLcApplication>();

    public virtual ICollection<CreditApplicationDocumentary> CreditApplicationDocumentary { get; set; } = new List<CreditApplicationDocumentary>();

    public virtual ICollection<LetterOfCreditVersion> LetterOfCreditVersion { get; set; } = new List<LetterOfCreditVersion>();
}
