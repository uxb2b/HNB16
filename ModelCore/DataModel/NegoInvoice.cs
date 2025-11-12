using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class NegoInvoice
{
    public int InvoiceID { get; set; }

    public int? NegoDraftID { get; set; }

    public string InvoiceNo { get; set; }

    public string LadingNo { get; set; }

    public decimal? InvoiceAmount { get; set; }

    public DateTime? InvoiceDate { get; set; }

    public DateTime? ImportDate { get; set; }

    public string ContractNo { get; set; }

    public string TaxNo { get; set; }

    public string InvoiceContent { get; set; }

    public short? DownloadFlag { get; set; }

    public string ReceiptNo { get; set; }

    public string AttachedFile { get; set; }

    public virtual ICollection<NegoAffair> NegoAffair { get; set; } = new List<NegoAffair>();

    public virtual NegoDraft NegoDraft { get; set; }

    public virtual ICollection<NegoInvoiceDetail> NegoInvoiceDetail { get; set; } = new List<NegoInvoiceDetail>();
}
