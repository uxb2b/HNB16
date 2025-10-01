namespace WebHome.Models
{
    public class LoginViewModel
    {
        public string? Account { get; set; }
        public string? Password { get; set; }
    }

    // 新增: 前端選取資料 POST 處理下一步
    public class NextStepRequest
    {
        public string? beneType { get; set; }
        public string? bene { get; set; }
        public string? inputType { get; set; }
    }

}
