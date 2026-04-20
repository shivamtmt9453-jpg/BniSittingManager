namespace BniSittingManager.Models
{
    public class BniUsers
    {
        public int UserId { get; set; }
        public string? BniMemberId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? CompanyName { get; set; }
        public string? Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
