using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class AmendingLcRegistry
{
    public int RegistrationID { get; set; }

    public decimal? 匯率 { get; set; }

    public decimal? 增加信用狀記帳金額 { get; set; }

    public int? 延長匯票期限 { get; set; }

    public decimal? 改狀費金額 { get; set; }

    public string 延長信用狀原因 { get; set; }

    public string 延長匯票期限原因 { get; set; }

    public decimal? 改狀手續費 { get; set; }

    public string 沖銷原因 { get; set; }

    public string 放款作業專員 { get; set; }

    public string 作業資訊組負責人 { get; set; }

    public decimal? 沖銷存入保證金金額 { get; set; }

    public string 交易憑證編號 { get; set; }
}
