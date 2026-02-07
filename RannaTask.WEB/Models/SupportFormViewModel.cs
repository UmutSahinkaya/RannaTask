using System.ComponentModel.DataAnnotations;

namespace RannaTask.WEB.Models
{
    public class SupportFormViewModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
    }

    public class CreateSupportFormViewModel
    {
        [Required(ErrorMessage = "Konu başlığı zorunludur")]
        [MaxLength(150, ErrorMessage = "Konu başlığı en fazla 150 karakter olabilir")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Mesaj zorunludur")]
        [MaxLength(500, ErrorMessage = "Mesaj en fazla 500 karakter olabilir")]
        public string Message { get; set; }
    }
}
