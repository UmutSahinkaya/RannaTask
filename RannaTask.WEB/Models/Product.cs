namespace RannaTask.WEB.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }
        public int? CreatedBy { get; set; }
        public string? CreatedByFullName { get; set; }  // Nullable
        public DateTime Created { get; set; }
    }
}
