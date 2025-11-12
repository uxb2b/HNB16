using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class NegoInvoiceDetail
{
    public int ProductID { get; set; }

    public int InvoiceID { get; set; }

    /// <summary>
    /// 明細排列序號
    /// </summary>
    public short? No { get; set; }

    /// <summary>
    /// 產品規格
    /// </summary>
    public string Spec { get; set; }

    /// <summary>
    /// 數量
    /// </summary>
    public decimal? Piece { get; set; }

    /// <summary>
    /// 數量
    /// </summary>
    public decimal? Piece2 { get; set; }

    /// <summary>
    /// 件數單位
    /// </summary>
    public string PieceUnit { get; set; }

    /// <summary>
    /// 件數單位
    /// </summary>
    public string PieceUnit2 { get; set; }

    /// <summary>
    /// 重量
    /// </summary>
    public decimal? Weight { get; set; }

    /// <summary>
    /// 重量單位
    /// </summary>
    public string WeightUnit { get; set; }

    /// <summary>
    /// 每單位運費
    /// </summary>
    public decimal? UnitFreight { get; set; }

    /// <summary>
    /// 單價
    /// </summary>
    public decimal? UnitCost { get; set; }

    /// <summary>
    /// 單價
    /// </summary>
    public decimal? UnitCost2 { get; set; }

    /// <summary>
    /// 總運費
    /// </summary>
    public decimal? FreightAmount { get; set; }

    /// <summary>
    /// 金額
    /// </summary>
    public decimal? CostAmount { get; set; }

    /// <summary>
    /// 金額
    /// </summary>
    public decimal? CostAmount2 { get; set; }

    /// <summary>
    /// 原幣金額
    /// </summary>
    public decimal? OriginalPrice { get; set; }

    /// <summary>
    /// 單一欄位備註
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 相關號碼
    /// </summary>
    public string RelateNumber { get; set; }

    /// <summary>
    /// 課稅別
    /// 1：應稅
    /// 2：零稅率
    /// 3：免稅
    /// 9：混合應稅與免稅或零稅率 (限收銀機發票無法分辨時使用)
    /// </summary>
    public byte? TaxType { get; set; }

    public string ItemNo { get; set; }

    public virtual NegoInvoice Invoice { get; set; }
}
