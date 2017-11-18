using System;
using System.Collections.Generic;

using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;

using GrandTheftMultiplayer.Shared.Math;

public class Team : Script
{
    Random rand = new Random();

    public readonly string name;
    public List<Client> members = new List<Client>();
    public readonly int maxMembers;
    /// <summary>
    /// Teams of the same -group- are allies.
    /// </summary>
    public readonly int group;
    public List<Vector3> spawns = new List<Vector3>();
    public List<Weapon> weapons = new List<Weapon>();
    public List<PedHash> skins = new List<PedHash>();

    public Team()
    {
        //maxMembers = 1;
    }

    public Team(string name, int maxMembers, int group)
    {
        this.name = name;
        this.maxMembers = maxMembers;
        this.group = group;
        API.setWorldSyncedData(name + "FULL", false);
    }

    public void SendChatMessageToGroup(string message)
    {
        foreach(Client player in members)
        {
            API.sendChatMessageToPlayer(player, message);
        }
    }

    public void RespawnAllMembers()
    {
        foreach(Client player in members)
        {
            RespawnMember(player);
        }
    }

    public void RespawnMember(Client player)
    {
        player.position = spawns[rand.Next(0, spawns.Count - 1)];
    }

    public void ArmAllPlayers()
    {
        foreach(Client player in members)
        {
            ArmPlayer(player);
        }
    }

    public void ArmPlayer(Client player)
    {
        foreach(Weapon weapon in weapons)
        {
            weapon.GiveWeaponToPlayer(player, true, true);
        }
    }

    /// <summary>
    /// Returns -1 if team is full. Disabled: Returns 0 if already in team. Returns 1 if successful.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public int AddMember(Client player)
    {
        if(members.Count >= maxMembers)
        {
            API.sendChatMessageToPlayer(player, "This team is full.");
            return -1;
        }
        else
        {
            /*foreach(Client member in members)
            {
                if(member == player)
                {
                    API.sendChatMessageToPlayer(player, "You are already in this team.");
                    return 0;
                }
            }*/

            API.sendChatMessageToPlayer(player, "Switching to " + name);

            if(members.Count >= maxMembers)
            {
                API.setWorldSyncedData(name + "FULL", true);
            }

            API.setPlayerSkin(player, skins[rand.Next(0, skins.Count - 1)]);
            //API.setWorldSyncedData(player.name + "T", name);
            player.setSyncedData("TEAM", name);
            members.Add(player);
            player.team = group;
            player.kill();
            return 1;
        }
    }

    public void RemoveMember(Client player)
    {
        if (members.Contains(player))
        {
            members.Remove(player);

            if (API.getWorldSyncedData(name + "FULL") == true)
            {
                API.setWorldSyncedData(name + "FULL", false);
            }
        }
        else
        {
            API.consoleOutput("ERROR: Tried to remove a Client from a team they weren't part of.");
        }
    }

    public Client GetRandomMember()
    {
        if (members.Count != 0)
        {
            return members[rand.Next(0, members.Count - 1)];
        }
        return null;
    }

    public void AddSkins(params string[] names)
    {
        foreach (string name in names)
        {
            skins.Add(API.pedNameToModel(name));
        }
    }

    public void AddWeapons(params Weapon[] weapons)
    {
        foreach(Weapon weapon in weapons)
        {
            this.weapons.Add(weapon);
        }
    }

    public void AddSpawns(params Vector3[] spawnLocations)
    {
        foreach(Vector3 spawn in spawnLocations)
        {
            spawns.Add(spawn);
        }
    }
}