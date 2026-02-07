namespace RannaTask.WEB.Common
{
    public static class SessionKeys
    {
        public const string JWTToken = "JWTToken";
        public const string Username = "Username";
        public const string UserRole = "UserRole";
        public const string UserId = "UserId";
    }

    public static class UserRoles
    {
        public const string Customer = "Customer";
        public const string Manager = "Manager";
        public const string Admin = "Admin";
    }

    public static class Messages
    {
        public const string LoginRequired = "Lütfen önce giriş yapın.";
        public const string UnauthorizedAccess = "Bu işlem için yetkiniz yok.";
        public const string LoginSuccess = "Giriş başarılı!";
        public const string LogoutSuccess = "Başarıyla çıkış yaptınız.";
        public const string UpdateSuccess = "Güncelleme başarılı!";
        public const string DeleteSuccess = "Silme işlemi başarılı!";
        public const string CreateSuccess = "Kayıt başarıyla oluşturuldu!";
        public const string ErrorOccurred = "Bir hata oluştu. Lütfen tekrar deneyin.";
    }
}
