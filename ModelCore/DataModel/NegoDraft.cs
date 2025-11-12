using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class NegoDraft
{
    public int DocumentaryID { get; set; }

    public int? NegoLcVersionID { get; set; }

    /// <summary>
    /// 押匯申請號碼：年
    /// </summary>
    public int? AppYear { get; set; }

    /// <summary>
    /// 押匯申請號碼：流水號
    /// </summary>
    public int? AppSeq { get; set; }

    /// <summary>
    /// 受益人指定的匯票號碼
    /// </summary>
    public string DraftNo { get; set; }

    /// <summary>
    /// 押匯提示日期
    /// </summary>
    public DateTime NegoDate { get; set; }

    /// <summary>
    /// 匯票出貨日期
    /// </summary>
    public DateTime? ShipmentDate { get; set; }

    /// <summary>
    /// 押匯金額
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 貨品名稱
    /// </summary>
    public string ItemName { get; set; }

    /// <summary>
    /// 貨品數量
    /// </summary>
    public decimal? ItemQuantity { get; set; }

    /// <summary>
    /// 貨品金額
    /// </summary>
    public decimal? ItemSubtotal { get; set; }

    /// <summary>
    /// 匯票之發票日期
    /// </summary>
    public DateTime? InvoiceDate { get; set; }

    /// <summary>
    /// 匯票正面圖章存檔路徑
    /// </summary>
    public string FrontSeal { get; set; }

    /// <summary>
    /// 匯票背面圖章存檔路徑
    /// </summary>
    public string BackSeal { get; set; }

    /// <summary>
    /// 註記CDS押匯資料已下載
    /// 1：是
    /// 其他：否
    /// </summary>
    public short? DownloadFlag { get; set; }

    /// <summary>
    /// 匯票影像(PDF)數據，以BASE64編碼儲存
    /// </summary>
    public string DraftContent { get; set; }

    /// <summary>
    /// 參考押匯提示清單
    /// </summary>
    public int? PromptID { get; set; }

    /// <summary>
    /// Deprecated
    /// </summary>
    public int? AppNoBase { get; set; }

    /// <summary>
    /// CDS匯票資源名稱
    /// </summary>
    public string ResourceName { get; set; }

    /// <summary>
    /// 檢附發票張數
    /// </summary>
    public int? InvoiceCount { get; set; }

    public virtual Documentary Documentary { get; set; }

    public virtual FpgNegoDraft FpgNegoDraft { get; set; }

    public virtual ICollection<GuaranteeDeposit> GuaranteeDeposit { get; set; } = new List<GuaranteeDeposit>();

    public virtual ICollection<NegoAffair> NegoAffair { get; set; } = new List<NegoAffair>();

    public virtual ICollection<NegoDraftAcceptance> NegoDraftAcceptance { get; set; } = new List<NegoDraftAcceptance>();

    public virtual NegoDraftDeal NegoDraftDeal { get; set; }

    public virtual NegoDraftExtension NegoDraftExtension { get; set; }

    public virtual ICollection<NegoInvoice> NegoInvoice { get; set; } = new List<NegoInvoice>();

    public virtual LetterOfCreditVersion NegoLcVersion { get; set; }

    public virtual ICollection<NegoLoan> NegoLoan { get; set; } = new List<NegoLoan>();

    public virtual NegoPrompt Prompt { get; set; }
}
