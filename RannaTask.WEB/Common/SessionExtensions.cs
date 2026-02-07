using Microsoft.AspNetCore.Http;

namespace RannaTask.WEB.Common
{
    public static class SessionExtensions
    {
        public static bool IsAuthenticated(this ISession session)
        {
            return !string.IsNullOrEmpty(session.GetString(SessionKeys.JWTToken));
        }

        public static string? GetJwtToken(this ISession session)
        {
            return session.GetString(SessionKeys.JWTToken);
        }

        public static string? GetUsername(this ISession session)
        {
            return session.GetString(SessionKeys.Username);
        }

        public static string? GetUserRole(this ISession session)
        {
            return session.GetString(SessionKeys.UserRole);
        }

        public static int? GetUserId(this ISession session)
        {
            return session.GetInt32(SessionKeys.UserId);
        }

        public static bool IsAdmin(this ISession session)
        {
            var role = session.GetUserRole();
            return role == UserRoles.Admin || role == UserRoles.Manager;
        }

        public static bool IsOwnerOrAdmin(this ISession session, int? ownerId)
        {
            if (session.IsAdmin()) return true;
            var userId = session.GetUserId();
            return userId.HasValue && ownerId.HasValue && userId.Value == ownerId.Value;
        }
    }
}
