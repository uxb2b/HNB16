using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

/// <summary>
/// 文件狀態變更記錄
/// </summary>
public partial class DocumentaryLevel
{
    public int DocumentaryID { get; set; }

    /// <summary>
    /// 文件狀態
    /// </summary>
    public int? DocLevel { get; set; }

    /// <summary>
    /// 變更日期
    /// </summary>
    public DateTime LevelDate { get; set; }

    public virtual LevelExpression DocLevelNavigation { get; set; }

    public virtual Documentary Documentary { get; set; }
}
