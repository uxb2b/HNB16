using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class FpgLcItem
{
    public int AppID { get; set; }

    public int GroupID { get; set; }

    public string DepartID { get; set; }

    public string ContactName { get; set; }

    public string ContactPhone { get; set; }

    public bool? AcceptEInvoice { get; set; }

    public decimal? 押匯允差比例 { get; set; }

    public bool? 接受發票地址與信用狀地址不符 { get; set; }

    public int? InvoiceBeforeDays { get; set; }

    public string CustomerNo { get; set; }

    public string 匯票起算基準 { get; set; }

    public virtual CreditApplicationDocumentary App { get; set; }

    public virtual GroupDepartment GroupDepartment { get; set; }
}
