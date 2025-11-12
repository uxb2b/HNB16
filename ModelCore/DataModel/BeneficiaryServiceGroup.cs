using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class BeneficiaryServiceGroup
{
    public int ServiceID { get; set; }

    public string ServiceName { get; set; }

    public string PostUrl { get; set; }

    public string ConfirmUrl { get; set; }

    public virtual ICollection<BeneficiaryGroup> BeneficiaryGroup { get; set; } = new List<BeneficiaryGroup>();

    public virtual ICollection<DataPortLog> DataPortLog { get; set; } = new List<DataPortLog>();

    public virtual ICollection<ResponseDataQueue> ResponseDataQueue { get; set; } = new List<ResponseDataQueue>();

    public virtual ICollection<ServiceDataQueue> ServiceDataQueue { get; set; } = new List<ServiceDataQueue>();
}
