using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.LightingExplorerTableColumn;
using UnityEngine.UI;

public class Skin : MonoBehaviour
{
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform head;
    [SerializeField] private Renderer pant;
    [SerializeField] private PantData pantData;

    private Weapon currentWeapon;
    private Hair currentHair;
    private Accessory currentAccessory;

    public Weapon CurWeapon => currentWeapon;

    [SerializeField] bool isCanChange = false;

    public void ChangeWeapon(WeaponType weaponType)
    {
        currentWeapon = SimplePool.Spawn<Weapon>((PoolType)weaponType, rightHand);
    }

    public void ChangeAccessory(AccessoryType accessoryType)
    {
        if (isCanChange && accessoryType != AccessoryType.None)
        {
            currentAccessory = SimplePool.Spawn<Accessory>((PoolType)accessoryType, leftHand);
        }
    }

    public void ChangeHair(HairType hatType)
    {
        if (isCanChange && hatType != HairType.None)
        {
            currentHair = SimplePool.Spawn<Hair>((PoolType)hatType, head);
        }
    }

    public void ChangePant(PantType pantType)
    {
        pant.material = pantData.GetPant(pantType);
    }

    public void OnDespawn()
    {
        SimplePool.Despawn(currentWeapon);
        if (currentAccessory) SimplePool.Despawn(currentAccessory);
        if (currentHair) SimplePool.Despawn(currentHair);
    }

    public void DespawnHair()
    {
        if (currentHair) SimplePool.Despawn(currentHair);
    }
    public void DespawnAccessory()
    {
        if (currentAccessory) SimplePool.Despawn(currentAccessory);
    }

    internal void DespawnWeapon()
    {
        if (currentWeapon) SimplePool.Despawn(currentWeapon);
    }
}
