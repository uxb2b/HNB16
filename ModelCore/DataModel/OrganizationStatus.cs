using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class OrganizationStatus
{
    public int CompanyID { get; set; }

    public bool? SelectedAsBeneficiary { get; set; }

    public bool? FpgNegoBeneficiary { get; set; }

    public string ReserveAccount { get; set; }

    public int? GroupID { get; set; }

    public int? ReserveAccountStatus { get; set; }

    public string ReserveAccountName { get; set; }

    public string ReserveAccountReceiptNo { get; set; }

    public string CustomerID { get; set; }

    public int? LogID { get; set; }

    public Guid? RecordID { get; set; }

    public int? CustomerStatus { get; set; }

    public decimal? HandlingCharge { get; set; }

    public decimal? StepCharge { get; set; }

    public virtual Organization Company { get; set; }

    public virtual BeneficiaryGroup Group { get; set; }

    public virtual RevisionLog Log { get; set; }
}
