using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class SpecificNotes
{
    public int NoteID { get; set; }

    public bool? 原留印鑑相符 { get; set; }

    public bool? 受益人單獨蓋章 { get; set; }

    public bool? 分批交貨 { get; set; }

    public DateTime? 最後交貨日 { get; set; }

    public bool? 接受發票早於開狀日 { get; set; }

    public bool? 接受發票金額大於開狀金額 { get; set; }

    public string 其他 { get; set; }

    public DateTime? 押匯起始日 { get; set; }

    public DateTime? 押匯發票起始日 { get; set; }

    public bool? 接受發票人地址與受益人地址不符 { get; set; }

    public bool? 接受發票電子訊息 { get; set; }

    public bool? 貨品明細以發票為準 { get; set; }

    public bool? 接受發票金額大於匯票金額 { get; set; }

    public bool? 以發票收執聯或扣抵聯正本押匯 { get; set; }

    public bool? 發票影本可接受 { get; set; }

    public string NonCSCTerms { get; set; }

    public bool? IsUsanceLcInterestPayByBuyer { get; set; }

    public bool? IsAcceptanceChargePayByBuyer { get; set; }

    public string SpecialMessageForCS { get; set; }

    public bool? IsCSCTerms { get; set; }

    public string CSCSalesDept { get; set; }

    public virtual ICollection<AmendingLcApplication> AmendingLcApplication { get; set; } = new List<AmendingLcApplication>();

    public virtual ICollection<CreditApplicationDocumentary> CreditApplicationDocumentary { get; set; } = new List<CreditApplicationDocumentary>();

    public virtual ICollection<DefaultSpecificNotes> DefaultSpecificNotes { get; set; } = new List<DefaultSpecificNotes>();

    public virtual ICollection<LetterOfCreditVersion> LetterOfCreditVersion { get; set; } = new List<LetterOfCreditVersion>();
}
