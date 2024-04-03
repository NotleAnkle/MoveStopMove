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

    WeaponType weaponType, eWeaponType;


    public override void Open()
    {
        base.Open();
        eWeaponType = UserData.Ins.playerWeapon;
        SelectWeapon(eWeaponType);
        UpdateCoin();
        CameraFollower.Instance.ChangeState(CameraFollower.State.Shop);
    }

    public void SelectWeapon(WeaponType type)
    {
        weaponType = type;

        WeaponItem item = data.GetWeaponItem(type);
        img.sprite = item.icon;

        ShopItem.State state = UserData.Ins.GetEnumData(type.ToString(), ShopItem.State.Lock);
        switch (state)
        {
            case ShopItem.State.Lock:
                SetButton(ButtonState.Lock);
                txtCost.text = item.cost.ToString();
                break;
            case ShopItem.State.Unlock:
                SetButton(ButtonState.Unlock);
                break;
        }
        if(eWeaponType == weaponType)
        {
            SetButton(ButtonState.Equipped);
        }
    }

    enum ButtonState { Lock = 0, Unlock = 1, Equipped = 2, None = 3 }
    private void SetButton(ButtonState state)
    {
        int index = (int)state;
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
        if (index < buttons.Length) buttons[index].SetActive(true);
    }

    public void BuyButton()
    {
        SoundManager.Instance.Play(AudioType.SFX_ButtonClick);

        int coin = Int32.Parse(txtCoin.text);
        int cost = Int32.Parse(txtCost.text);
        if (coin > cost)
        {
            UserData.Ins.SetIntData(UserData.Key_Coin, ref UserData.Ins.coin, coin - cost);
            txtCoin.text = UserData.Ins.coin.ToString();
            UserData.Ins.SetEnumData(weaponType.ToString(), ShopItem.State.Unlock);
            SetButton(ButtonState.Unlock);
        }
    }
    public void EquipButton()
    {
        SoundManager.Instance.Play(AudioType.SFX_ButtonClick);

        eWeaponType = weaponType;
        UserData.Ins.SetEnumData(UserData.Key_Player_Weapon, ref UserData.Ins.playerWeapon, weaponType);

        LevelManager.Instance.Player.TryCloth(UIShop.ShopType.weapon, weaponType);

        SetButton(ButtonState.Equipped);
    }

    public void Next()
    {
        SoundManager.Instance.Play(AudioType.SFX_ButtonClick);

        SelectWeapon(data.NextType(weaponType));
    }
    public void Previous()
    {
        SoundManager.Instance.Play(AudioType.SFX_ButtonClick);

        SelectWeapon(data.PrevType(weaponType));
    }
    public void Back()
    {
        CloseDirectly();
        SoundManager.Instance.Play(AudioType.SFX_ButtonClick);
        UIManager.Instance.OpenUI<UIMainMenu>();
    }
    private void UpdateCoin()
    {
        txtCoin.text = UserData.Ins.coin.ToString();
    }
}
