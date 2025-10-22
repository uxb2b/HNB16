using System;

namespace WebHome.Models
{
    public class CancelAppQueryModel
    {
        public string? BeneType { get; set; }
        public string? LcNo { get; set; }
        public string? BeneInNo { get; set; }
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
