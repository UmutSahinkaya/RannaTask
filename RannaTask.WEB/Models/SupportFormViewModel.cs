using System.ComponentModel.DataAnnotations;

namespace RannaTask.WEB.Models
{
    public class SupportFormViewModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public int Status { get; set; } // 0=Pending, 1=Processed, 2=Deleted
        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public string CloseReason { get; set; }  // Admin'in kapama sebebi

        public string StatusText => Status switch
        {
            0 => "Beklemede",
            1 => "İşleme Alındı",
            2 => "Kapatıldı",
            _ => "Bilinmiyor"
        };

        public string StatusBadgeClass => Status switch
        {
            0 => "bg-warning text-dark",
            1 => "bg-info",
            2 => "bg-danger",
            _ => "bg-secondary"
        };
    }

    public class AdminSupportFormViewModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public string CloseReason { get; set; }

        public string StatusText => Status switch
        {
            0 => "Beklemede",
            1 => "İşleme Alındı",
            2 => "Kapatıldı",
            _ => "Bilinmiyor"
        };

        public string StatusBadgeClass => Status switch
        {
            0 => "bg-warning text-dark",
            1 => "bg-info",
            2 => "bg-danger",
            _ => "bg-secondary"
        };
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
