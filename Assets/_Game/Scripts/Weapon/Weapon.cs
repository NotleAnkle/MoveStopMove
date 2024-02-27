using _Framework.Pool.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : GameUnit
{
    [SerializeField] WeaponType type;
    [SerializeField] PoolType bulletType;
    

    public bool IsActive => gameObject.activeSelf;

    private Character owner;

    public void OnInit(Character character)
    {
        owner = character;
    }

    public void Throw(Vector3 target)
    {
        {
            Bullet bullet = SimplePool.Spawn<Bullet>(bulletType, TF.position, Quaternion.identity);
            bullet.OnInit(target, owner);
            owner.WeaponDisable();
        }
    }
}
