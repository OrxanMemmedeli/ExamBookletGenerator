using Microsoft.AspNetCore.SignalR;

namespace ExamBookletGenerator.Hubs;

public class UserActivityHub : Hub
{
    private readonly UserSessionManagerService _userSessionManager;

    public UserActivityHub(UserSessionManagerService userSessionManager)
    {
        _userSessionManager = userSessionManager;
    }

    public async Task GetActiveUsers()
    {
        var activeUsers = _userSessionManager.GetActiveUsers();
        await Clients.Caller.SendAsync("ReceiveActiveUsers", activeUsers);
    }
}
