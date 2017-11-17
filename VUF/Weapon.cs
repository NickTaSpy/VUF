using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Server.ArrayExtensions;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Server;

using GrandTheftMultiplayer.Shared.Math;

public class Weapon : Script
{
    public string model;
    public WeaponHash hash;

    public WeaponTint tint;

    public int ammo;

    public List<WeaponComponent> components;

    public Weapon(string model, int ammo, WeaponTint tint = WeaponTint.Normal, params WeaponComponent[] components)
    {
        this.model = model;
        hash = API.weaponNameToModel(model);

        this.tint = tint;
        this.ammo = ammo;

        this.components = components.ToList<WeaponComponent>();
    }

    /*public Weapon(string model, int ammo)
    {
        this.model = model;
        hash = API.weaponNameToModel(model);

        this.ammo = ammo;
    }*/

    public Weapon()
    {

    }

    public void GiveWeaponToPlayer(Client player, bool equipNow, bool ammoLoaded)
    {
        API.givePlayerWeapon(player, hash, ammo, equipNow, ammoLoaded);
        API.setPlayerWeaponTint(player, hash, tint);
        foreach(WeaponComponent component in components)
        {
            API.givePlayerWeaponComponent(player, hash, component);
        }
    }
}