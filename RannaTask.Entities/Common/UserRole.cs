namespace RannaTask.Entities.Common
{
    public enum UserRole
    {
        Customer = 1,  // Default role for new registrations
        Manager = 2,   // Limited admin privileges  
        Admin = 3      // Full system access
    }

    public enum NotificationType
    {
        Info = 0,
        Success = 1,
        Warning = 2,
        Error = 3,
        SupportFormCreated = 10,
        SupportFormUpdated = 11,
        NewOrder = 20,
        NewUser = 30
    }
}
