using _UI.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponShop : UICanvas
{
    [SerializeField] private WeaponData data;
    [SerializeField] private Image img;
    [SerializeField] private Text txtCost;
    [SerializeField] private GameObject[] buttons;

    [SerializeField] private Text txtCoin;

    WeaponType weaponType;


    public override void Open()
    {
        base.Open();
        SelectWeapon(UserData.Ins.playerWeapon);
        txtCoin.text = UserData.Ins.coin.ToString();
    }

    public void SelectWeapon(WeaponType type)
    {
        weaponType = type;

        WeaponItem item = data.GetWeaponItem(type);
        img.sprite = item.icon;

        ShopItem.State state = UserData.Ins.GetEnumData(type.ToString(), ShopItem.State.Buy);
        switch (state)
        {
            case ShopItem.State.Buy:
                SetButton(0);
                txtCost.text = item.cost.ToString();
                break;
            case ShopItem.State.Bought:
                SetButton(1);
                break;
            case ShopItem.State.Equipped:
                SetButton(2);
                break;
        }
    }

    private void SetButton(int index)
    {
        // 0: Buy, 1: Bought, 2: Equipped
        for(int i =  0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
        buttons[index].SetActive(true);
    }

    public void BuyButton()
    {
        int coin = Int32.Parse(txtCoin.text);
        int cost = Int32.Parse(txtCost.text);
        if (coin > cost)
        {
            
        }
        UserData.Ins.SetEnumData(weaponType.ToString(), ShopItem.State.Bought);
        SetButton(1);
    }
    public void EquipButton()
    {
        UserData.Ins.SetEnumData(weaponType.ToString(), ShopItem.State.Equipped);
        UserData.Ins.SetEnumData(UserData.Ins.playerWeapon.ToString(), ShopItem.State.Bought);
        UserData.Ins.SetEnumData(UserData.Key_Player_Weapon, ref UserData.Ins.playerWeapon, weaponType);

        LevelManager.Instance.Player.TryCloth(UIShop.ShopType.weapon, weaponType);

        SetButton(2);
    }

    public void Next()
    {
        SelectWeapon(data.NextType(weaponType));
    }
    public void Previous()
    {
        SelectWeapon(data.PrevType(weaponType));
    }
    public void Back()
    {
        Close(0f);
        UIManager.Instance.OpenUI<MainMenu>();
    }
}
