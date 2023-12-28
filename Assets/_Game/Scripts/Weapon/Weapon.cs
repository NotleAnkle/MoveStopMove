using _Framework.Pool.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    None = 0,
    Hammer = 1,
    Kinfe = 2,
    Boomerang = 3,
}
public class Weapon : GameUnit
{
    [SerializeField] WeaponType type;
    [SerializeField] PoolType bulletType;
    [SerializeField] private Renderer render;
    

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
            bullet.OnInit(target, owner, render.material);
            owner.WeaponDisable();
        }
    }
}
