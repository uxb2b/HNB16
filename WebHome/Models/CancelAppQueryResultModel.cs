using System;
using System.Collections.Generic;

namespace WebHome.Models
{
    public class CancelAppQueryItem
    {
        public string? LcNo { get; set; }
        public string? Bank { get; set; }
        public DateTime? IssueDate { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AvailableAmount { get; set; }
        public string? BeneInNo { get; set; }
        public string? Status { get; set; }
        public string? LcType { get; set; }
        public string? BeneType { get; set; }
    }

    public class CancelAppQueryResultModel : PagingViewModel
    {
        public List<CancelAppQueryItem> Items { get; set; } = new List<CancelAppQueryItem>();
    }
}
