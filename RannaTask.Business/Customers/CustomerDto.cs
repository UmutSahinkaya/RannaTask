using RannaTask.Entities.Common;

namespace RannaTask.Business.Customers
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
    }
}
