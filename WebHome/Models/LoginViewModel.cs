namespace WebHome.Models
{
    public class LoginViewModel
    {
        public string? Account { get; set; }
        public string? Password { get; set; }
    }

    // �s�W: �e�ݿ����� POST �B�z�U�@�B
    public class NextStepRequest
    {
        public string? beneType { get; set; }
        public string? bene { get; set; }
        public string? inputType { get; set; }
    }

}
