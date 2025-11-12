using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class BankData
{
    public string BankCode { get; set; }

    public string BranchName { get; set; }

    public string Address { get; set; }

    public string Phone { get; set; }

    public string CRC_Branch { get; set; }

    public int? BranchType { get; set; }

    public virtual ICollection<BankInbox> BankInbox { get; set; } = new List<BankInbox>();

    public virtual ICollection<BankUserBranch> BankUserBranch { get; set; } = new List<BankUserBranch>();

    public virtual ICollection<BeneficiaryData> BeneficiaryData { get; set; } = new List<BeneficiaryData>();

    public virtual ICollection<BeneficiaryTransferInto> BeneficiaryTransferInto { get; set; } = new List<BeneficiaryTransferInto>();

    public virtual ICollection<CreditApplicationDocumentary> CreditApplicationDocumentaryAdvisingBankCodeNavigation { get; set; } = new List<CreditApplicationDocumentary>();

    public virtual ICollection<CreditApplicationDocumentary> CreditApplicationDocumentaryPayableBankCodeNavigation { get; set; } = new List<CreditApplicationDocumentary>();

    public virtual ICollection<CustomerOfBranch> CustomerOfBranch { get; set; } = new List<CustomerOfBranch>();

    public virtual DisabledBranch DisabledBranch { get; set; }

    public virtual ICollection<LetterOfCredit> LetterOfCredit { get; set; } = new List<LetterOfCredit>();

    public virtual ICollection<NegoDraftDeal> NegoDraftDeal { get; set; } = new List<NegoDraftDeal>();

    public virtual ICollection<NegoDraftExtension> NegoDraftExtensionLcBranchNavigation { get; set; } = new List<NegoDraftExtension>();

    public virtual ICollection<NegoDraftExtension> NegoDraftExtensionNegoBranchNavigation { get; set; } = new List<NegoDraftExtension>();

    public virtual ICollection<NegoLC> NegoLCAdvisingBankNavigation { get; set; } = new List<NegoLC>();

    public virtual ICollection<NegoLC> NegoLCPayableBankNavigation { get; set; } = new List<NegoLC>();

    public virtual ICollection<OrganizationBranchSettings> OrganizationBranchSettings { get; set; } = new List<OrganizationBranchSettings>();
}
