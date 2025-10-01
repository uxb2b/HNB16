using System;

namespace WebHome.Models
{
    public class PagingViewModel
    {
        public int PageIndex { get; set; } = 1; // �ثe����(1-based)
        public int PageSize { get; set; } = 10; // �C������
        public int TotalCount { get; set; } // �`����
        public int TotalAmount { get; set; } // �`���B(�p�����)
        public int PageCount => (int)Math.Ceiling((double)TotalCount / PageSize);
        public string PageAction { get; set; } // �����ɩI�s�� action/js function
        public string PageParam { get; set; } // ��L�Ѽ�
    }
}