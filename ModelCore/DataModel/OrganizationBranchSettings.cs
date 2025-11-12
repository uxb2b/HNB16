using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class OrganizationBranchSettings
{
    public int SettingID { get; set; }

    public int CompanyID { get; set; }

    public string BankCode { get; set; }

    public int? Status { get; set; }

    public decimal? StepCharge { get; set; }

    public decimal? HandlingCharge { get; set; }

    public Guid? RecordID { get; set; }

    public int? LogID { get; set; }

    public virtual BankData BankCodeNavigation { get; set; }

    public virtual Organization Company { get; set; }

    public virtual RevisionLog Log { get; set; }
}
