using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

/// <summary>
/// 開狀申請書
/// </summary>
public partial class CreditApplicationDocumentary
{
    public int DocumentaryID { get; set; }

    public string ApplicationNo { get; set; }

    public DateTime ApplicationDate { get; set; }

    /// <summary>
    /// 參考BeneficiaryData
    /// </summary>
    public int ApplicantID { get; set; }

    /// <summary>
    /// 參考BeneficiaryData
    /// </summary>
    public int BeneficiaryID { get; set; }

    /// <summary>
    /// 參考BeneficiaryData
    /// </summary>
    public string IssuingBankCode { get; set; }

    /// <summary>
    /// 即為付款人(付款行與原意不符)
    /// </summary>
    public string PayableBankCode { get; set; }

    public string AdvisingBankCode { get; set; }

    public bool AtSight { get; set; }

    public int UsanceDays { get; set; }

    /// <summary>
    /// 參考開狀申請檢附文件
    /// </summary>
    public int? AttachableDocumentID { get; set; }

    /// <summary>
    /// 參考特別指示條款
    /// </summary>
    public int? SpecificNotesID { get; set; }

    /// <summary>
    /// 參考金額、日期申請資料
    /// </summary>
    public int? LcItemsID { get; set; }

    /// <summary>
    /// CDS開狀申請之申請書檔名
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 非本行制式特別指示之申請原因及依據
    /// </summary>
    public string Instrunction { get; set; }

    /// <summary>
    /// 參考申請人歷史記錄檔
    /// </summary>
    public int? ApplicantDetailsID { get; set; }

    /// <summary>
    /// 參考受益人歷史記錄檔
    /// </summary>
    public int? BeneDetailsID { get; set; }

    /// <summary>
    /// 註記臨櫃申請
    /// 1:是
    /// 0或NULL:否
    /// </summary>
    public bool? OverTheCounter { get; set; }

    /// <summary>
    /// 信用狀受益人已接受
    /// 1:是
    /// 其它:否
    /// </summary>
    public bool? IsAccepted { get; set; }

    public virtual BankData AdvisingBankCodeNavigation { get; set; }

    public virtual CustomerOfBranchVersion ApplicantDetails { get; set; }

    public virtual AttachableDocument AttachableDocument { get; set; }

    public virtual CustomerOfBranchVersion BeneDetails { get; set; }

    public virtual BeneficiaryData Beneficiary { get; set; }

    public virtual ICollection<CreditLoan> CreditLoan { get; set; } = new List<CreditLoan>();

    public virtual CustomerOfBranch CustomerOfBranch { get; set; }

    public virtual Documentary Documentary { get; set; }

    public virtual FpgLcItem FpgLcItem { get; set; }

    public virtual ICollection<GuaranteeDeposit> GuaranteeDeposit { get; set; } = new List<GuaranteeDeposit>();

    public virtual LcItems LcItems { get; set; }

    public virtual ICollection<LetterOfCredit> LetterOfCredit { get; set; } = new List<LetterOfCredit>();

    public virtual BankData PayableBankCodeNavigation { get; set; }

    public virtual SpecificNotes SpecificNotes { get; set; }
}
