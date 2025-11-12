using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class FpgLcMonthlyReport
{
    public int ReportID { get; set; }

    public int Year { get; set; }

    public int Month { get; set; }

    public int Catalog { get; set; }

    public string BankCode { get; set; }

    public int ApplicantID { get; set; }

    public int BeneID { get; set; }

    public string ProductCategory { get; set; }

    public int RecordCount { get; set; }

    public virtual BeneficiaryData Bene { get; set; }

    public virtual CustomerOfBranch CustomerOfBranch { get; set; }
}
