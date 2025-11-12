using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class BeneficiaryData
{
    public int OrganizationID { get; set; }

    public bool? AppCountersign { get; set; }

    public string AppAccount { get; set; }

    public string Approver01 { get; set; }

    public string Approver02 { get; set; }

    public int? Status { get; set; }

    public int? DraftType { get; set; }

    public int? CustomerOfBranchVersionID { get; set; }

    public int? LogID { get; set; }

    public Guid? RecordID { get; set; }

    public string NegoCenter { get; set; }

    public virtual ICollection<ApplicantBeneficiaryPair> ApplicantBeneficiaryPair { get; set; } = new List<ApplicantBeneficiaryPair>();

    public virtual ICollection<CreditApplicationDocumentary> CreditApplicationDocumentary { get; set; } = new List<CreditApplicationDocumentary>();

    public virtual CustomerOfBranchVersion CustomerOfBranchVersion { get; set; }

    public virtual FpgBeneficiaryStatus FpgBeneficiaryStatus { get; set; }

    public virtual ICollection<FpgLcMonthlyReport> FpgLcMonthlyReport { get; set; } = new List<FpgLcMonthlyReport>();

    public virtual RevisionLog Log { get; set; }

    public virtual BankData NegoCenterNavigation { get; set; }

    public virtual ICollection<NegoLC> NegoLC { get; set; } = new List<NegoLC>();

    public virtual Organization Organization { get; set; }
}
