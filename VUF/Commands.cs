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

public class Commands : Script
{
    [Command("kill", Alias = "killme,suicide")]
    public void Suicide(Client player)
    {
        player.kill();
    }

#region TEMP Commands

    [Command("veh", Alias = "v")]
    public void SpawnCar(Client player, string vehicleName, int color1, int color2)
    {
        API.createVehicle(API.vehicleNameToModel(vehicleName), player.position, player.rotation, color1, color2);
    }

    [Command("coords")]
    public void GetCoords(Client player)
    {
        API.sendChatMessageToPlayer(player, player.position.X + ", " + player.position.X + ", " + player.position.X);
    }

    [Command("goto")]
    public void Goto(Client player, Client targetPlayer)
    {
        if (targetPlayer.exists)
        {
            API.setEntityPosition(player, targetPlayer.position);
        }
    }

    [Command("tp")]
    public void GotoCoords(Client player, float X, float Y, float Z)
    {
        API.setEntityPosition(player, new Vector3(X, Y, Z));
    }

#endregion

    [Command("switch", Alias = "switchteam")]
    public void SwitchTeam(Client player)
    {
        API.triggerClientEvent(player, "onSwitch", true);
    }

    [Command("vip")]
    public void EnterVipDraft(Client player)
    {
        if (GameManager.isDraftActive == true)
        {
            if (!GameManager.applicants.Contains(player))
            {
                GameManager.applicants.Add(player);
                API.sendChatMessageToPlayer(player, "You have entered the random VIP selection.");
            }
            else
            {
                API.sendChatMessageToPlayer(player, "You have already done this.");
            }
        }
        else
        {
            API.sendChatMessageToPlayer(player, "The VIP selection process is not active.");
        }
    }

    [Command("t", Alias = "team", GreedyArg = true)]
    public void SendTeamChat(Client player, string sentence)
    {
        string teamName;
        int team = API.getPlayerTeam(player);
        if(team == 0)
        {
            teamName = "Government";
        }
        else
        {
            teamName = "Mercenaries";
        }

        string msg = player.name + "[" + teamName + "]: " + sentence;
        List<Client> players = API.getAllPlayers();

        foreach (Client client in players)
        {
            if (API.getPlayerTeam(client) == team)
            {
                API.sendChatMessageToPlayer(client, msg);
            }
        }
    }

    [Command("g", Alias = "group,sq,squad", GreedyArg = true)]
    public void SendGroupChat(Client player, string sentence)
    {
        Team team = GameManager.FindPlayerTeam(player);

        if (team != null)
        {
            string msg = player.name + "[" + team.name + "]: " + sentence;
            team.SendChatMessageToGroup(msg);
        }
        else
        {
            API.sendChatMessageToPlayer(player, "You are not in a team.");
        }
    }
}
