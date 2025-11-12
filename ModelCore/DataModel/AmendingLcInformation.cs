using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class AmendingLcInformation
{
    public int AmendingLcApplicationID { get; set; }

    public DateTime AmendingDate { get; set; }

    public string InformationNo { get; set; }

    public virtual AmendingLcApplication AmendingLcApplication { get; set; }

    public virtual ICollection<LetterOfCreditVersion> LetterOfCreditVersion { get; set; } = new List<LetterOfCreditVersion>();
}
