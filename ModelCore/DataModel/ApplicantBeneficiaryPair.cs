using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class ApplicantBeneficiaryPair
{
    public int ApplicantID { get; set; }

    public int BeneID { get; set; }

    public string BeneficiaryChiefTitle { get; set; }

    public string BeneficiaryChiefEmail { get; set; }

    public virtual Organization Applicant { get; set; }

    public virtual BeneficiaryData Bene { get; set; }
}
