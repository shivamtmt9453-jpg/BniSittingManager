namespace BniSittingManager.Areas.Admin.Models
{
    public class UnseatedMemberVM
    {
        public int RoundID { get; set; }
        public string? RoundName { get; set; }

        public string? UserName { get; set; }
        public string? MemberId { get; set; }

        public string? Phone { get; set; }
        public string? Email { get; set; }

        public string? Type { get; set; }
    }
}
