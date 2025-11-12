using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class FpgNegoDraft
{
    public int NegoDraftID { get; set; }

    public string 匯入銀行代碼 { get; set; }

    public string 匯入帳號 { get; set; }

    public string 營業員姓名 { get; set; }

    public string 營業員連絡電話 { get; set; }

    public string Remark { get; set; }

    public string 押匯提示地點 { get; set; }

    public string 匯入戶名 { get; set; }

    public string 匯入銀行名稱 { get; set; }

    public virtual FpgNegoRemittance FpgNegoRemittance { get; set; }

    public virtual NegoDraft NegoDraft { get; set; }
}
