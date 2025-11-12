using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class GoodsDetail
{
    public int ItemID { get; set; }

    public int Sno { get; set; }

    public string ProductName { get; set; }

    public string ProductSize { get; set; }

    public string UnitPrice { get; set; }

    public string Quantity { get; set; }

    public string Amount { get; set; }

    public string Remark { get; set; }

    public virtual LcItems Item { get; set; }
}
