using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Server.ArrayExtensions;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Server;

using GrandTheftMultiplayer.Shared.Math;


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
