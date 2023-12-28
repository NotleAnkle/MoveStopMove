using _Framework.Event.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponButton : ItemButton
{
    [SerializeField] WeaponType weaponType;
    public override void TaskOnClick()
    {
        //LevelManager.Instance.Player.ChangeWeapon(weaponType);
        this.PostEvent(EventID.OnPlayerWeaponChange, weaponType);
    }
}
