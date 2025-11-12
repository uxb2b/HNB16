using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

/// <summary>
/// 文件退回記錄
/// </summary>
public partial class DocumentaryDenial
{
    public int DocID { get; set; }

    /// <summary>
    /// 退回者
    /// </summary>
    public string Denier { get; set; }

    /// <summary>
    /// 退回原因
    /// </summary>
    public string Reason { get; set; }

    /// <summary>
    /// 退回日期
    /// </summary>
    public DateTime DenialDate { get; set; }

    public virtual Documentary Doc { get; set; }
}
