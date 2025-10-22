using System;

namespace WebHome.Models
{
    public class PagingViewModel
    {
        public int PageIndex { get; set; } = 1; // ヘ玡计(1-based)
        public int PageSize { get; set; } = 10; // –掸计
        public int TotalCount { get; set; } // 羆掸计
        public decimal TotalAmount { get; set; } // 羆肂(惠陪ボ) - changed to decimal for money
        public int PageCount => (int)Math.Ceiling((double)TotalCount / PageSize);
        public string? PageAction { get; set; } // だ㊣ action/js function
        public string? PageParam { get; set; } // ㄤ把计
    }
}