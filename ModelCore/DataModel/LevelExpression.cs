using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

/// <summary>
/// 文件狀態描述
/// </summary>
public partial class LevelExpression
{
    public int LevelID { get; set; }

    /// <summary>
    /// 狀態名稱
    /// </summary>
    public string Expression { get; set; }

    /// <summary>
    /// 狀態詳細資訊
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 狀態名稱企業端顯示使用
    /// </summary>
    public string BusinessExpression { get; set; }

    public virtual ICollection<CustomerOfBranch> CustomerOfBranch { get; set; } = new List<CustomerOfBranch>();

    public virtual ICollection<Documentary> Documentary { get; set; } = new List<Documentary>();

    public virtual ICollection<DocumentaryLevel> DocumentaryLevel { get; set; } = new List<DocumentaryLevel>();
}
