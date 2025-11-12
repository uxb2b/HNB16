using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class FpgLcMonthlySummary
{
    public int ReportID { get; set; }

    public int Year { get; set; }

    public int Month { get; set; }

    public int Catalog { get; set; }

    public string BranchName { get; set; }

    public string ApplicantNo { get; set; }

    public string BeneficiaryNo { get; set; }

    public string ProductCategory { get; set; }

    public int RecordCount { get; set; }

    public string ApplicantName { get; set; }

    public decimal? Amount { get; set; }
}
