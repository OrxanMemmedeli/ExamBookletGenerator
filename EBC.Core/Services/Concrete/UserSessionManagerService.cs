using EBC.Core.Constants;
using System.Collections.Concurrent;

public class UserSessionManagerService
{
    private readonly ConcurrentDictionary<Guid, UserSessionInfo> _activeSessions = new();
    private readonly Timer _sessionCleanupTimer;

    public UserSessionManagerService()
    {
        // Sessiya təmizləməsi üçün bir timer
        _sessionCleanupTimer = new Timer(CleanupExpiredSessions, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
    }

    public void AddOrUpdateUser(Guid userId, string userName, DateTime loginTime)
    {
        var sessionInfo = new UserSessionInfo
        {
            UserId = userId,
            UserName = userName,
            LoginTime = loginTime,
            LastActivityTime = DateTime.Now
        };

        _activeSessions[userId] = sessionInfo;
    }

    public void RemoveUser(Guid userId)
    {
        _activeSessions.TryRemove(userId, out _);
    }

    public List<UserSessionInfo> GetActiveUsers()
    {
        return _activeSessions.Values.ToList();
    }

    private void CleanupExpiredSessions(object state)
    {
        var expiredSessions = _activeSessions
            .Where(pair => pair.Value.LastActivityTime.AddMinutes(ApplicationCommonField.ExpireTimeSpan) < DateTime.Now)
            .Select(pair => pair.Key)
            .ToList();

        foreach (var userId in expiredSessions)
        {
            _activeSessions.TryRemove(userId, out _);
        }
    }
}

public class UserSessionInfo
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public DateTime LoginTime { get; set; }
    public DateTime LastActivityTime { get; set; }
}
