using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

/// <summary>
/// 文件核准記錄
/// </summary>
public partial class DocumentaryAllowance
{
    public int DocID { get; set; }

    /// <summary>
    /// 核准人
    /// </summary>
    public string Approver { get; set; }

    /// <summary>
    /// 核准日期
    /// </summary>
    public DateTime ApprovalDate { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    public string Memo { get; set; }

    public virtual Documentary Doc { get; set; }
}
