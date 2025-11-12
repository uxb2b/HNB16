using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class Organization
{
    public string ContactName { get; set; }

    public string Fax { get; set; }

    public string LogoURL { get; set; }

    public string CompanyName { get; set; }

    public int CompanyID { get; set; }

    public string ReceiptNo { get; set; }

    public string Phone { get; set; }

    public string ContactFax { get; set; }

    public string ContactPhone { get; set; }

    public string ContactMobilePhone { get; set; }

    public string RegAddr { get; set; }

    public string UndertakerName { get; set; }

    public string UndertakerID { get; set; }

    public string Addr { get; set; }

    public string EnglishName { get; set; }

    public string EnglishAddr { get; set; }

    public string EnglishRegAddr { get; set; }

    public string ContactEmail { get; set; }

    public string UndertakerPhone { get; set; }

    public string UndertakerFax { get; set; }

    public string UndertakerMobilePhone { get; set; }

    public string InvoiceSignature { get; set; }

    public virtual ICollection<ApplicantBeneficiaryPair> ApplicantBeneficiaryPair { get; set; } = new List<ApplicantBeneficiaryPair>();

    public virtual BeneficiaryData BeneficiaryData { get; set; }

    public virtual ICollection<BeneficiaryTransferInto> BeneficiaryTransferInto { get; set; } = new List<BeneficiaryTransferInto>();

    public virtual ICollection<CustomerInbox> CustomerInbox { get; set; } = new List<CustomerInbox>();

    public virtual ICollection<CustomerOfBranch> CustomerOfBranch { get; set; } = new List<CustomerOfBranch>();

    public virtual DefaultSpecificNotes DefaultSpecificNotes { get; set; }

    public virtual ICollection<DocumentOwner> DocumentOwner { get; set; } = new List<DocumentOwner>();

    public virtual ICollection<OrganizationBranchSettings> OrganizationBranchSettings { get; set; } = new List<OrganizationBranchSettings>();

    public virtual OrganizationExtension OrganizationExtension { get; set; }

    public virtual OrganizationStatus OrganizationStatus { get; set; }

    public virtual ICollection<Organization> Company { get; set; } = new List<Organization>();

    public virtual ICollection<Organization> Enterprise { get; set; } = new List<Organization>();
}
