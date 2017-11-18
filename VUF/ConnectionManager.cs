using GrandTheftMultiplayer.Server.API;

using GrandTheftMultiplayer.Server.Elements;


public class ConnectionManager : Script
{
    public ConnectionManager()
    {
        API.onPlayerBeginConnect += OnPlayerBeginConnectHandler;
        API.onPlayerConnected += OnPlayerConnectedHandler;
        API.onPlayerFinishedDownload += OnPlayerFinishedDownloadHandler;
        API.onPlayerDisconnected += OnPlayerDisconnectedHandler;
    }

    private void OnPlayerBeginConnectHandler(Client player, CancelEventArgs e)
    {
        API.sendChatMessageToAll(player.name + "has joined the server.");
    }

    private void OnPlayerConnectedHandler(Client player)
    {
        API.sendChatMessageToPlayer(player, "Welcome to the server.");
        player.setSyncedData("TEAM", "N/A");
        //API.setWorldSyncedData(player.name + "T", "N/A");
    }

    private void OnPlayerFinishedDownloadHandler(Client player)
    {
        API.sendNotificationToPlayer(player, "Downloads complete.");
        //API.setPlayerToSpectator(player);
        API.triggerClientEvent(player, "onSwitch", false);
    }

    private void OnPlayerDisconnectedHandler(Client player, string reason)
    {
        API.sendChatMessageToAll(player.name + "has left the server. (" + reason + ")");
        //API.resetWorldSyncedData(player.name + "T");
        player.resetSyncedData("TEAM");

        Team team = GameManager.FindPlayerTeam(player);
        if (team != null)
        {
            team.RemoveMember(player);
        }
    }
}
