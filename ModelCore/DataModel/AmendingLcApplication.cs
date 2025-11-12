using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class AmendingLcApplication
{
    public int DocumentaryID { get; set; }

    /// <summary>
    /// 參考修狀參照到的版本來源，可以是原開狀申請書或前一次修狀申請書
    /// </summary>
    public int? SourceID { get; set; }

    public string AmendmentNo { get; set; }

    public DateTime? ApplicationDate { get; set; }

    /// <summary>
    /// 參考特別指示條款
    /// </summary>
    public int? SpecificNotesID { get; set; }

    /// <summary>
    /// 參考開狀申請檢附文件
    /// </summary>
    public int? AttachableDocumentID { get; set; }

    /// <summary>
    /// 參考金額、日期申請資料
    /// </summary>
    public int? LcItemsID { get; set; }

    /// <summary>
    /// Deprecated
    /// </summary>
    public string TransactionMessage { get; set; }

    /// <summary>
    /// 修狀受益人已接受
    /// 1:是
    /// 其它:否
    /// </summary>
    public bool IsAccepted { get; set; }

    /// <summary>
    /// Deprecated
    /// </summary>
    public string SysDocID { get; set; }

    /// <summary>
    /// CDS修狀申請之申請書檔名
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 已列修狀通知書次數
    /// </summary>
    public int? PrintNotice { get; set; }

    /// <summary>
    /// 非本行制式特別指示之申請原因及依據
    /// </summary>
    public string Instruction { get; set; }

    /// <summary>
    /// 註記臨櫃申請
    /// 1:是
    /// 0或NULL:否
    /// </summary>
    public bool? OverTheCounter { get; set; }

    public virtual AmendingLcInformation AmendingLcInformation { get; set; }

    public virtual AttachableDocument AttachableDocument { get; set; }

    public virtual ICollection<CreditLoan> CreditLoan { get; set; } = new List<CreditLoan>();

    public virtual Documentary Documentary { get; set; }

    public virtual LcItems LcItems { get; set; }

    public virtual LetterOfCreditVersion Source { get; set; }

    public virtual SpecificNotes SpecificNotes { get; set; }
}
