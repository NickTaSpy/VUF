using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;

using GrandTheftMultiplayer.Shared.Math;

public class Vehicles : Script
{
    public static List<NetHandle> vehicles;

    public Vehicles()
    {
        API.onVehicleDeath += OnVehicleDeathHandler;
        API.onPlayerExitVehicle += OnPlayerExitVehicleHandler;

        // ---- SWAT Vehicles ---- //
        API.createVehicle((VehicleHash)1912215274, new Vector3(445.1998, -1026.091, 28.42171), new Vector3(0.5562082, 1.012805, 40.96253), 111, 1, 0); // police3 1
        API.createVehicle((VehicleHash)1912215274, new Vector3(440.9985, -1026.579, 28.49809), new Vector3(0.5167511, 0.9624939, 42.28933), 111, 1, 0); // police3 2
        API.createVehicle((VehicleHash)1912215274, new Vector3(436.7538, -1026.896, 28.57737), new Vector3(0.5849969, 0.8757461, 46.08618), 111, 1, 0); // police3 3
        API.createVehicle((VehicleHash)1912215274, new Vector3(432.7074, -1027.531, 28.64887), new Vector3(0.6919616, 0.7971498, 49.95726), 111, 1, 0); // police3 4
        API.createVehicle((VehicleHash)1912215274, new Vector3(427.7566, -1027.924, 28.73663), new Vector3(0.7513831, 0.7267215, 52.00016), 111, 1, 0); // police3 5
        API.createVehicle((VehicleHash)456714581, new Vector3(454.4361, -1024.64, 28.47339), new Vector3(1.278937, 0.4113689, 92.86674), 111, 1, 0); // policet 1
        API.createVehicle((VehicleHash)456714581, new Vector3(454.2607, -1020.324, 28.33616), new Vector3(0.8852493, -1.071976, 91.18797), 111, 1, 0); // policet 2
        API.createVehicle((VehicleHash)456714581, new Vector3(454.2404, -1015.773, 28.41381), new Vector3(1.166458, 0.2457956, 90.03165), 111, 1, 0); // policet 3
        API.createVehicle((VehicleHash)(-1647941228), new Vector3(450.3737, -1011.899, 28.10443), new Vector3(0.5673326, 0.6854766, 89.62288), 1, 1, 0); // fbi2 1
        API.createVehicle((VehicleHash)(-1647941228), new Vector3(433.4791, -1013.372, 28.3748), new Vector3(1.621883, 1.541115, 89.93412), 1, 1, 0); // fbi2 2
        API.createVehicle((VehicleHash)(-34623805), new Vector3(439.5472, -1013.439, 28.10537), new Vector3(2.325675, -4.665363, 139.5677), 111, 1, 0); // policeb 1
        API.createVehicle((VehicleHash)(-34623805), new Vector3(441.1389, -1014.301, 28.12979), new Vector3(1.731322, -6.670093, 163.945), 111, 1, 0); // policeb 2
        API.createVehicle((VehicleHash)(-34623805), new Vector3(443.1007, -1014.329, 28.0939), new Vector3(0.765241, -9.83606, -161.3758), 111, 1, 0); // policeb 3

        // ---- Bodyguard/VIP Vehicles ---- //
        API.createVehicle((VehicleHash)(-604842630), new Vector3(229.0567, -367.5257, 43.76673), new Vector3(0.7836313, 1.493032, -108.1322), 1, 1, 0); // Cognoscenti2 1
        API.createVehicle((VehicleHash)(-604842630), new Vector3(237.6503, -370.3131, 43.87197), new Vector3(0.6340828, 1.733838, -108.0648), 1, 1, 0); // Cognoscenti2 2
        API.createVehicle((VehicleHash)666166960, new Vector3(245.6635, -373.1134, 44.25681), new Vector3(0.7657914, 1.470812, -108.1165), 1, 1, 0); // baller6 1
        API.createVehicle((VehicleHash)666166960, new Vector3(253.1416, -375.7762, 44.38214), new Vector3(1.312611, 1.448513, -106.9972), 1, 1, 0); // baller6 2
        API.createVehicle((VehicleHash)(-1961627517), new Vector3(261.6479, -378.9248, 44.32645), new Vector3(1.011269, 0.8153141, -110.696), 1, 1, 0); // stretch 1

        // ---- Assassins Vehicles ---- //
        API.createVehicle((VehicleHash)(-909201658), new Vector3(-355.0742, -117.6049, 38.20299), new Vector3(0.1976756, -12.84848, 101.6586), 1, 1, 0); // fcr 1
        API.createVehicle((VehicleHash)(-909201658), new Vector3(-355.071, -117.6229, 38.19904), new Vector3(0.1565174, -15.00109, 101.6696), 1, 1, 0); // fcr 2
        API.createVehicle((VehicleHash)(-114291515), new Vector3(-355.8466, -119.924, 38.0463), new Vector3(0.2631184, -4.945786, 111.0056), 1, 1, 0); // bati 1
        API.createVehicle((VehicleHash)(-114291515), new Vector3(-356.5315, -121.8739, 38.04445), new Vector3(0.2307454, -8.693155, 115.2954), 1, 1, 0); // bati 2
        API.createVehicle((VehicleHash)788045382, new Vector3(-358.2126, -127.2584, 38.17161), new Vector3(0.2126414, -9.666724, 104.467), 1, 1, 0); // sanchez 1
        API.createVehicle((VehicleHash)788045382, new Vector3(-358.8239, -129.6281, 38.16889), new Vector3(0.06485472, -11.76263, 102.6776), 1, 1, 0); // sanchez 2
        API.createVehicle((VehicleHash)(-1543762099), new Vector3(-366.3639, -108.8264, 38.63121), new Vector3(-0.04607499, 0.5973284, 160.0278), 1, 1, 0); // gresley 1
        API.createVehicle((VehicleHash)(-1543762099), new Vector3(-371.1554, -107.0867, 38.62337), new Vector3(-0.05750858, -0.004722821, 161.0604), 1, 1, 0); // gresley 2


        vehicles = API.getAllVehicles();
        foreach(NetHandle vehicle in vehicles)
        {
            API.setEntityData(vehicle, "SPAWN_POS", API.getEntityPosition(vehicle));
            API.setEntityData(vehicle, "SPAWN_ROT", API.getEntityRotation(vehicle));
        }
    }

    public void Delay(int ms, Action action)
    {
        new Task(() => {
            API.sleep(ms);
            action();
        }).Start();
    }

    private void RespawnVehicle(NetHandle vehicle)
    {
        int model = API.getEntityModel(vehicle);
        int color1 = API.getVehiclePrimaryColor(vehicle);
        int color2 = API.getVehicleSecondaryColor(vehicle);

        Vector3 spawnPos = API.getEntityData(vehicle, "SPAWN_POS");
        Vector3 spawnRot = API.getEntityData(vehicle, "SPAWN_ROT");

        API.deleteEntity(vehicle);

        Vehicle newVehicle = API.createVehicle((VehicleHash)model, spawnPos, spawnRot, color1, color2);

        API.setEntityData(newVehicle, "SPAWN_POS", spawnPos);
        API.setEntityData(newVehicle, "SPAWN_ROT", spawnRot);
    }

    public static float GetDistance(Vector3 pos1, Vector3 pos2)
    {
        return (float) Math.Sqrt(Math.Pow(pos2.X - pos1.X, 2) + Math.Pow(pos2.Y - pos1.Y, 2) + Math.Pow(pos2.Z - pos1.Z, 2));
    }


    private void OnVehicleDeathHandler(NetHandle vehicle)
    {
        Delay(20000, () =>
        {
            RespawnVehicle(vehicle);
        });
    }

    /// <summary>
    /// Checks if a vehicle is empty for a specified time and performs the specified action.
    /// </summary>
    /// <param name="ms">How long to check if empty.</param>
    /// <param name="step">How quickly to check if empty(ms).</param>
    /// <param name="vehicle"></param>
    /// <param name="action">What to do if empty for the whole period.</param>
    private void StartIdleProcedure(int ms, int step, NetHandle vehicle, Action action)
    {
        new Task(() => {
            for(int i=0; i<ms; i+=step)
            {
                API.sleep(step);
                if(API.getVehicleOccupants(vehicle).Count() != 0)
                {
                    return;
                }
            }
            action();
        }).Start();
    }

    private void OnPlayerExitVehicleHandler(Client player, NetHandle vehicle)
    {
        if(API.getVehicleOccupants(vehicle).Count() == 0 && GetDistance(API.getEntityPosition(vehicle),  API.getEntityData(vehicle, "SPAWN_POS")) > 5)
        {
            StartIdleProcedure(30000, 20, vehicle, () =>
            {
                RespawnVehicle(vehicle);
            });
        }
    }
}