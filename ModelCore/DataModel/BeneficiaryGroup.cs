using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class BeneficiaryGroup
{
    public int GroupID { get; set; }

    public string GroupName { get; set; }

    public int? ServiceID { get; set; }

    public virtual ICollection<GroupDepartment> GroupDepartment { get; set; } = new List<GroupDepartment>();

    public virtual ICollection<OrganizationStatus> OrganizationStatus { get; set; } = new List<OrganizationStatus>();

    public virtual BeneficiaryServiceGroup Service { get; set; }
}
