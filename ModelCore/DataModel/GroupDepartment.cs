using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class GroupDepartment
{
    public int GroupID { get; set; }

    public string DepartID { get; set; }

    public string Department { get; set; }

    public virtual ICollection<FpgLcItem> FpgLcItem { get; set; } = new List<FpgLcItem>();

    public virtual BeneficiaryGroup Group { get; set; }
}
