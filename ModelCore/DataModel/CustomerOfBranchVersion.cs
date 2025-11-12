using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class CustomerOfBranchVersion
{
    public int VersionID { get; set; }

    public string BankCode { get; set; }

    public int? CompanyID { get; set; }

    public string CompanyName { get; set; }

    public string PayableAccount { get; set; }

    public string Addr { get; set; }

    public string Phone { get; set; }

    public string ContactEmail { get; set; }

    public string Undertaker { get; set; }

    public int? PostponeMonths { get; set; }

    public int? UsancelimitedDays { get; set; }

    public string PID { get; set; }

    public string EnglishName { get; set; }

    public string EnglishAddr { get; set; }

    public string RegAddr { get; set; }

    public string EnglishRegAddr { get; set; }

    public string Fax { get; set; }

    public string UndertakerID { get; set; }

    public string UndertakerName { get; set; }

    public string ContactName { get; set; }

    public string ContactPhone { get; set; }

    public string PRODUCTS { get; set; }

    public string OldBankCode { get; set; }

    public virtual ICollection<BeneficiaryData> BeneficiaryData { get; set; } = new List<BeneficiaryData>();

    public virtual ICollection<CreditApplicationDocumentary> CreditApplicationDocumentaryApplicantDetails { get; set; } = new List<CreditApplicationDocumentary>();

    public virtual ICollection<CreditApplicationDocumentary> CreditApplicationDocumentaryBeneDetails { get; set; } = new List<CreditApplicationDocumentary>();

    public virtual ICollection<CustomerOfBranch> CustomerOfBranch { get; set; } = new List<CustomerOfBranch>();

    public virtual ICollection<NegoLC> NegoLCApplicantDetails { get; set; } = new List<NegoLC>();

    public virtual ICollection<NegoLC> NegoLCBeneDetails { get; set; } = new List<NegoLC>();

    public virtual ICollection<OrganizationExtension> OrganizationExtension { get; set; } = new List<OrganizationExtension>();
}
