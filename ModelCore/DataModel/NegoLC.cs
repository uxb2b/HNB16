using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class NegoLC
{
    public int LcID { get; set; }

    public int? Status { get; set; }

    public DateTime? ImportDate { get; set; }

    public string IssuingBank { get; set; }

    public string AdvisingBank { get; set; }

    public string PayableBank { get; set; }

    public string LCType { get; set; }

    public int? CompanyID { get; set; }

    public int? BeneficiaryID { get; set; }

    public short? DownloadFlag { get; set; }

    public int? ApplicantDetailsID { get; set; }

    public int? BeneDetailsID { get; set; }

    public virtual BankData AdvisingBankNavigation { get; set; }

    public virtual CustomerOfBranchVersion ApplicantDetails { get; set; }

    public virtual CustomerOfBranchVersion BeneDetails { get; set; }

    public virtual BeneficiaryData Beneficiary { get; set; }

    public virtual CustomerOfBranch CustomerOfBranch { get; set; }

    public virtual LetterOfCredit Lc { get; set; }

    public virtual BankData PayableBankNavigation { get; set; }
}
