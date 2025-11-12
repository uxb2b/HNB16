using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class LcItems
{
    public int ItemID { get; set; }

    public decimal? 開狀金額 { get; set; }

    public DateTime? 有效期限 { get; set; }

    public int CurrencyTypeID { get; set; }

    public string Goods { get; set; }

    public int 定日付款 { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string PaymentTerms { get; set; }

    public virtual ICollection<AmendingLcApplication> AmendingLcApplication { get; set; } = new List<AmendingLcApplication>();

    public virtual ICollection<CreditApplicationDocumentary> CreditApplicationDocumentary { get; set; } = new List<CreditApplicationDocumentary>();

    public virtual CurrencyType CurrencyType { get; set; }

    public virtual ICollection<GoodsDetail> GoodsDetail { get; set; } = new List<GoodsDetail>();

    public virtual ICollection<LetterOfCreditVersion> LetterOfCreditVersion { get; set; } = new List<LetterOfCreditVersion>();
}
