using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class RevisionLog
{
    public int LogID { get; set; }

    public string Actor { get; set; }

    public DateTime RevisionDate { get; set; }

    public Guid? RevsionSource { get; set; }

    public virtual ICollection<BeneficiaryData> BeneficiaryData { get; set; } = new List<BeneficiaryData>();

    public virtual ICollection<OrganizationBranchSettings> OrganizationBranchSettings { get; set; } = new List<OrganizationBranchSettings>();

    public virtual ICollection<OrganizationStatus> OrganizationStatus { get; set; } = new List<OrganizationStatus>();
}
