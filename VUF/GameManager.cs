using System;
using System.Collections.Generic;
using System.Timers;

using GrandTheftMultiplayer.Server.API;

using GrandTheftMultiplayer.Server.Elements;

using GrandTheftMultiplayer.Shared.Math;

public class GameManager : Script
{
    Random rand = new Random();

    public bool isGameActive;
    Timer gameStartTimer;
    Timer gameDurationTimer;

    public static Team vip = new Team("VIP", 1, 0);

    public static Team swat = new Team("SWAT", 1, 0);
    public static Team army = new Team("Army", 1, 0);
    public static Team bodyguards = new Team("Bodyguards", 1, 0);

    public static Team mercs = new Team("Mercs", 1, 1);
    public static Team assassins = new Team("Assassins", 1, 1);

    public static List<Team> teams = new List<Team>();

    public static bool isDraftActive;
    public static List<Client> applicants = new List<Client>();

    public GameManager()
    {
        API.onUpdate += OnUpdateHandler;
        API.onClientEventTrigger += OnClientEvent;
        API.onPlayerRespawn += OnPlayerRespawnHandler;

        gameStartTimer = new Timer(30000); // 30 secs
        gameStartTimer.Elapsed += OnGameStart;

        gameDurationTimer = new Timer(300000); // 5 mins
        gameDurationTimer.Elapsed += OnGameTimeUp;

        isGameActive = false;
        isDraftActive = false;
        
        teams.Add(vip);
        teams.Add(swat);
        teams.Add(bodyguards);
        teams.Add(mercs);
        teams.Add(assassins);

        vip.AddSkins("ReporterCutscene", "Bankman");
        vip.AddWeapons(new Weapon("HeavyPistol", 144), new Weapon("KnuckleDuster", 1));
        vip.AddSpawns(new Vector3(238.4927, -405.5849, 47.92432));

        swat.AddSkins("SWAT01SMY");
        swat.AddWeapons(new Weapon("SawnoffShotgun", 15), new Weapon("BZGas", 3), new Weapon("AssaultRifle", 120), new Weapon("SMG", 90));
        swat.AddSpawns(new Vector3(260.4082, -1489.325, 29.29161), new Vector3(456.8914, -1008.366, 28.31837), new Vector3(458.5671, -1008.342, 28.27585),
        new Vector3(424.4714, -1007.444, 29.61497), new Vector3(426.7649, -1007.491, 29.61084), new Vector3(456.5244, -1002.962, 30.71727), new Vector3(443.8906, -1002.582, 30.71659),
        new Vector3(440.5023, -1002.911, 30.71562), new Vector3(424.3476, -1003.922, 30.70946), new Vector3(426.4367, -1003.379, 30.71013));

        army.AddSkins("Marine03SMY", "Marine01SMY");

        bodyguards.AddSkins("FBISuit01", "HighSec01SMM", "HighSec02SMM");
        bodyguards.AddWeapons(new Weapon("HeavyPistol", 144), new Weapon("Nightstick", 1));
        bodyguards.AddSpawns(new Vector3(232.2155, -404.946, 47.92432), new Vector3(234.348, -405.6843, 47.92432), new Vector3(237.2814, -407.3879, 47.92432),
        new Vector3(240.386, -408.0085, 47.92432), new Vector3(243.3273, -408.9338, 47.92432), new Vector3(245.5109, -393.3376, 46.30566),
        new Vector3(239.5705, -390.9218, 46.30545));

        mercs.AddSkins("BlackOps01SMY", "BlackOps02SMY");
        mercs.AddWeapons(new Weapon("SpecialCarbine", 210), new Weapon("CombatPistol", 60), new Weapon("Knife", 1), new Weapon("BZGas", 3));

        assassins.AddSkins("ChemSec01SMM");
        assassins.AddWeapons(new Weapon("MicroSMG", 96), new Weapon("SwitchBlade", 1), new Weapon("CompactRifle", 60));
        assassins.AddSpawns(new Vector3(-356.4784, -130.4867, 39.43676), new Vector3(-354.4721, -124.3784, 39.42702), new Vector3(-353.0675, -120.5959, 39.43065),
        new Vector3(-364.0782, -104.6166, 39.54303), new Vector3(-368.4073, -103.4676, 39.5429), new Vector3(-373.2346, -101.3657, 39.54302),
        new Vector3(-349.9528, -112.2495, 39.43022));
    }
    
    private void OnUpdateHandler()
    {
        if (API.getAllPlayers().Count < 1)
        {
            if (isGameActive)
            {
                EndGame("Not enough players. The game has been canceled.");
            }
        }
        else
        {
            if (!isGameActive && !isDraftActive)
            {
                API.sendChatMessageToAll("Game starting in " + gameStartTimer.Interval/1000 + " seconds!");
                //IsGameActive = true;
                StartVipSelection();
                gameStartTimer.Start();
            }
            else if (isGameActive)
            {
                if (vip.members.Count == 0)
                {
                    EndGame("The VIP doesn't exist. The Mercenaries have won.");
                }
                else if (vip.members[0].dead)
                {
                    EndGame("The VIP is dead. The Mercenaries have won.");
                }
            }
        }

    }

    private void StartVipSelection()
    {
        isDraftActive = true;
        applicants.Clear();
        API.sendChatMessageToAll("Starting random VIP selection...~n~Type ~y~' /vip ' ~s~to possibly become the VIP.");
    }

    private void EndVipSelection()
    {
        Client winner;

        isDraftActive = false;
        if(applicants.Count != 0)
        {
            winner = applicants[rand.Next(0, applicants.Count - 1)];
            API.sendChatMessageToAll("~y~" + winner.name + "~s~ is the new VIP.");
            vip.AddMember(winner);
            
        }
        else
        {
            API.sendChatMessageToAll("No VIP applicants found. Choosing at random...");
            winner = GetRandomTeamMember();
            if(winner == null)
            {
                API.consoleOutput("ERROR: Tried to select a VIP when there are no members in any team.");
                return;
            }
        }
        Team team = FindPlayerTeam(winner);
        team.RemoveMember(winner);
    }

    private void OnGameStart(System.Object source, System.Timers.ElapsedEventArgs e)
    {
        isGameActive = true;
        EndVipSelection();
        foreach (Team team in teams)
        {
            team.RespawnAllMembers();
            team.ArmAllPlayers();
        }
        gameDurationTimer.Start();
    }

    private void OnGameTimeUp(System.Object source, System.Timers.ElapsedEventArgs e)
    {
        EndGame("Time's up! The VIP has escaped.");
    }

    private void EndGame(string reason)
    {
        API.sendChatMessageToAll(reason);
        gameDurationTimer.Stop();
        isGameActive = false;
    }

    public static Team FindPlayerTeam(Client player)
    {
        //string teamName = API.getWorldSyncedData(player.name + "T");
        string teamName = player.getSyncedData("TEAM");

        foreach(Team team in teams)
        {
            if(team.name == teamName)
            {
                return team;
            }
        }
        return null;
    }

    public Client GetRandomTeamMember()
    {
        foreach(Team team in teams)
        {
            if(team.members.Count != 0)
            {
                return team.GetRandomMember();
            }
        }
        return null;
    }

    private void OnPlayerRespawnHandler(Client player)
    {
        Team team = FindPlayerTeam(player);
        if (team != null)
        {
            team.RespawnMember(player);
            team.ArmPlayer(player);
        }
    }

    private void OnClientEvent(Client player, string eventName, params object[] arguments)
    {
        if (eventName == "Switch")
        {
            string targetTeam = arguments[0].ToString();

            Team team = FindPlayerTeam(player);

            if(team != null && team.name == targetTeam)
            {
                API.sendChatMessageToPlayer(player, "You are already in this team.");
                return;
            }

            switch (targetTeam)
            {
                case "Mercs":
                    if(mercs.AddMember(player) == 1 && team != null)
                    {
                        team.RemoveMember(player);
                    }
                    break;
                case "Assassins":
                    if (assassins.AddMember(player) == 1 && team != null)
                    {
                        team.RemoveMember(player);
                    }
                    break;
                case "SWAT":
                    if (swat.AddMember(player) == 1 && team != null)
                    {
                        team.RemoveMember(player);
                    }
                    break;
                case "Bodyguards":
                    if (bodyguards.AddMember(player) == 1 && team != null)
                    {
                        team.RemoveMember(player);
                    }
                    break;
                case "Army":
                    if (army.AddMember(player) == 1 && team != null)
                    {
                        team.RemoveMember(player);
                    }
                    break;
            }
        }
    }
}
