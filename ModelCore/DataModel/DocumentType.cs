using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

/// <summary>
/// 文件類型
/// </summary>
public partial class DocumentType
{
    public int TypeID { get; set; }

    /// <summary>
    /// 文件類別名稱
    /// </summary>
    public string TypeName { get; set; }

    public virtual ICollection<Documentary> Documentary { get; set; } = new List<Documentary>();
}
