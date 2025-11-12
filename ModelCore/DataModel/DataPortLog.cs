using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class DataPortLog
{
    public int LogID { get; set; }

    public int? Direction { get; set; }

    public string ContentPath { get; set; }

    public int? Catalog { get; set; }

    public DateTime? TransportTime { get; set; }

    public int? ExceptionLogID { get; set; }

    public string FpgTableName { get; set; }

    public string FpgFileName { get; set; }

    public int? ServiceID { get; set; }

    public virtual AlertDataQueue AlertDataQueue { get; set; }

    public virtual ICollection<DataProcessLog> DataProcessLog { get; set; } = new List<DataProcessLog>();

    public virtual ExceptionLog ExceptionLog { get; set; }

    public virtual ICollection<FpgNegoRemittanceLog> FpgNegoRemittanceLog { get; set; } = new List<FpgNegoRemittanceLog>();

    public virtual ICollection<NegoPrompt> NegoPrompt { get; set; } = new List<NegoPrompt>();

    public virtual NegoPromptRequestQueue NegoPromptRequestQueue { get; set; }

    public virtual ReceivedDataQueue ReceivedDataQueue { get; set; }

    public virtual ResponseDataQueue ResponseDataQueue { get; set; }

    public virtual BeneficiaryServiceGroup Service { get; set; }

    public virtual ServiceDataQueue ServiceDataQueue { get; set; }
}
