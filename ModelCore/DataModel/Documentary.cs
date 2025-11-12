using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

/// <summary>
/// 系統文件主體
/// </summary>
public partial class Documentary
{
    public int DocID { get; set; }

    /// <summary>
    /// 文件類型索引
    /// </summary>
    public int? DocType { get; set; }

    /// <summary>
    /// 文件日期
    /// </summary>
    public DateTime? DocDate { get; set; }

    /// <summary>
    /// 目前狀態
    /// </summary>
    public int? CurrentLevel { get; set; }

    /// <summary>
    /// Deprecated
    /// </summary>
    public string SysDocID { get; set; }

    /// <summary>
    /// 附件檔存檔路徑
    /// </summary>
    public string AttachedFile { get; set; }

    /// <summary>
    /// 預約日期
    /// </summary>
    public DateTime? DesiredDate { get; set; }

    public virtual AmendingLcApplication AmendingLcApplication { get; set; }

    public virtual ICollection<BankInbox> BankInbox { get; set; } = new List<BankInbox>();

    public virtual CreditApplicationDocumentary CreditApplicationDocumentary { get; set; }

    public virtual CreditCancellation CreditCancellation { get; set; }

    public virtual LevelExpression CurrentLevelNavigation { get; set; }

    public virtual ICollection<CustomerInbox> CustomerInbox { get; set; } = new List<CustomerInbox>();

    public virtual DocumentType DocTypeNavigation { get; set; }

    public virtual DocumentDispatch DocumentDispatch { get; set; }

    public virtual DocumentOwner DocumentOwner { get; set; }

    public virtual ICollection<DocumentProcessLog> DocumentProcessLog { get; set; } = new List<DocumentProcessLog>();

    public virtual ICollection<DocumentaryAllowance> DocumentaryAllowance { get; set; } = new List<DocumentaryAllowance>();

    public virtual ICollection<DocumentaryDenial> DocumentaryDenial { get; set; } = new List<DocumentaryDenial>();

    public virtual ICollection<DocumentaryLevel> DocumentaryLevel { get; set; } = new List<DocumentaryLevel>();

    public virtual NegoDraft NegoDraft { get; set; }

    public virtual NegoDraftAcceptance NegoDraftAcceptance { get; set; }
}
