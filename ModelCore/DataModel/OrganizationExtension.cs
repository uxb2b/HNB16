using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class OrganizationExtension
{
    public int CompanyID { get; set; }

    public string PRODUCTS { get; set; }

    public int? TOTAL_AMOUNT { get; set; }

    public int? NEED_CP { get; set; }

    public string CP_ACCOUNT { get; set; }

    public string SIGNET { get; set; }

    public string STATUS_CODE { get; set; }

    public DateTime? CREATE_DATE { get; set; }

    public string PID { get; set; }

    public string CA_NO { get; set; }

    public int? CustomerOfBranchVersionID { get; set; }

    public virtual Organization Company { get; set; }

    public virtual CustomerOfBranchVersion CustomerOfBranchVersion { get; set; }
}
