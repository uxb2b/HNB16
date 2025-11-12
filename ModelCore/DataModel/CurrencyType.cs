using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class CurrencyType
{
    public int CurrencyID { get; set; }

    public string CurrencyName { get; set; }

    public string AbbrevName { get; set; }

    public virtual ICollection<LcItems> LcItems { get; set; } = new List<LcItems>();
}
