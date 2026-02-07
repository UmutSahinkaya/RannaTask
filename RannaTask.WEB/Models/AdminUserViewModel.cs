namespace RannaTask.WEB.Models
{
    public class AdminUserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }

        public string RoleName => Role switch
        {
            1 => "Customer",
            2 => "Manager",
            3 => "Admin",
            _ => "Unknown"
        };

        public string RoleBadgeClass => Role switch
        {
            1 => "bg-primary",
            2 => "bg-warning text-dark",
            3 => "bg-danger",
            _ => "bg-secondary"
        };
    }
}
