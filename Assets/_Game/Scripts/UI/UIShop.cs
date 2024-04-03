using _UI.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : UICanvas
{
    public enum ShopType { hair = 0, pant = 1, accessory = 2, skin = 3, weapon = 4}
    [SerializeField] private ShopData data;
    [SerializeField] private Transform contentPanel;

    [SerializeField] private Text txtCoin;
    [SerializeField] private Text txtCost;
    [SerializeField] private ShopItemBar[] bars;
    [SerializeField] private GameObject[] buttons;

    private ShopItemBar curBar;
    private ShopItem curItem;
    private ShopType shopType;

    private ShopItem itemEquiped;

    private List<Enum> equippedTypes = new List<Enum>();

    [SerializeField] private ShopItem prefab; 
    MiniPool<ShopItem> miniPool = new MiniPool<ShopItem>();

    private void Awake()
    {
        miniPool.OnInit(prefab, 12, contentPanel);

        for (int i = 0; i < bars.Length; i++)
        {
            bars[i].SetShop(this);
        }
    }
    public override void Open()
    {
        base.Open();

        curBar = bars[0];

        ReloadData();

        CameraFollower.Instance.ChangeState(CameraFollower.State.Shop);
    }

    private void ReloadData()
    {
        GetEquippedData();

        SelectBar(curBar);

        UpdateCoinText();
    }
    internal void SelectBar(ShopItemBar selectBar)
    {
        if(selectBar != null)
        {
            curBar.Active(false);
        }

        if (curBar != selectBar)
        {
            curItem = null;
        }

        curBar = selectBar;
        curBar.Active(true);
        shopType = curBar.Type;

        miniPool.Collect();

        switch (curBar.Type)
        {
            case ShopType.hair:
                InitItemFrames(data.hairs.ListItem);
                break;
            case ShopType.pant:
                InitItemFrames(data.pants.ListItem);
                break;
            case ShopType.accessory:
                InitItemFrames(data.accessories.ListItem);
                break;
            case ShopType.skin:
                InitItemFrames(data.skins.ListItem);
                break;
        }
    }
    internal void SelectItem(ShopItem item)
    {
        LevelManager.Instance.Player.TryCloth(curBar.Type, item.Type);
        curItem = item;
        SetButtonState(item);
        if(equippedTypes.Contains(item.Type))
        {
            SetButton(ButtonState.Equipped);
        }
    }

    private void SetButtonState(ShopItem item)
    {
        switch (item.state)
        {
            case ShopItem.State.Lock:
                SetButton(ButtonState.Lock);
                txtCost.text = item.Cost.ToString();
                break;
            case ShopItem.State.Unlock:
                SetButton(ButtonState.Unlock);
                break;
        }
    }

    enum ButtonState {Lock = 0,Unlock = 1, Equipped = 2, None = 3}
    private void SetButton(ButtonState state)
    {
        int index = (int) state;
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
        if(index < buttons.Length) buttons[index].SetActive(true);
    }

    public void OnBuyButtonClick()
    {
        SoundManager.Instance.Play(AudioType.SFX_ButtonClick);
        if (UserData.Ins.coin >= curItem.Cost)
        {
            int coin = UserData.Ins.coin - curItem.Cost;
            UserData.Ins.SetIntData(UserData.Key_Coin, ref UserData.Ins.coin, coin);

            UserData.Ins.SetEnumData(curItem.Type.ToString(), ShopItem.State.Unlock);
            curItem.SetState(ShopItem.State.Unlock);

            ReloadData();
            curItem.SetSelect();
        }

    }

    public void OnEquipButtonClick()
    {
        SoundManager.Instance.Play(AudioType.SFX_ButtonClick);

        if (curItem != null)
        {
            ResetEquippingItem();
            SavePlayerSkinData();
            
            ReloadData();
        }
    }

    private void SavePlayerSkinData()
    {
        switch (shopType)
        {
            case ShopType.hair:
                //save id do moi vao player
                UserData.Ins.SetEnumData(UserData.Key_Player_Hair, ref UserData.Ins.playerHair, (HairType)curItem.Type);
                break;
            case ShopType.pant:
                UserData.Ins.SetEnumData(UserData.Key_Player_Pant, ref UserData.Ins.playerPant, (PantType)curItem.Type);
                break;
            case ShopType.accessory:
                UserData.Ins.SetEnumData(UserData.Key_Player_Accessory, ref UserData.Ins.playerAccessory, (AccessoryType)curItem.Type);
                break;
            case ShopType.skin:
                UserData.Ins.SetEnumData(UserData.Key_Player_Skin, ref UserData.Ins.playerSkin, (SkinType)curItem.Type);
                break;
            case ShopType.weapon:
                break;
            default:
                break;
        }
    }

    public void OnTryButtonClick()
    {
        ResetEquippingItem();
        SavePlayerSkinData();

        ReloadData();
    }

    private void ResetEquippingItem()
    {
        if (itemEquiped)
        {
            itemEquiped.SetEquipped(false);
        }
        itemEquiped = curItem;
        itemEquiped.SetEquipped(true);
        equippedTypes[(int)shopType] = itemEquiped.Type;
    }
    private void InitItemFrames<T>(List<ShopItemData<T>> items) where T : System.Enum
    {
        // Tat nut buy/equip/equipped
        SetButton(ButtonState.None);
      

        for(int i = 0; i < items.Count; i++)
        {
            ShopItem.State state = UserData.Ins.GetEnumData(items[i].type.ToString(), ShopItem.State.Lock);
            ShopItem item = miniPool.Spawn();
            item.SetState(state);
            item.SetData<T>(items[i], this);
            if (equippedTypes.Contains(item.Type))
            {
                itemEquiped = item;
                if (!curItem)
                {
                    curItem = itemEquiped;
                }
                SelectItem(curItem);
                itemEquiped.SetEquipped(true);
                curItem.SetSelect();
            }
        }
    }

    public void Back()
    {
        curBar.Active(false);
        CloseDirectly();
        SoundManager.Instance.Play(AudioType.SFX_ButtonClick);
        UIManager.Instance.OpenUI<UIMainMenu>();

        LevelManager.Instance.Player.TakeOffCloth();
        LevelManager.Instance.Player.EquipedCloth();
    }

    private void UpdateCoinText()
    {
        txtCoin.text = UserData.Ins.coin.ToString();
    }

    private void GetEquippedData()
    {
        equippedTypes.Clear();
        //ShopType.hair = 0
        equippedTypes.Add(UserData.Ins.playerHair);
        //ShopType.pant = 1
        equippedTypes.Add(UserData.Ins.playerPant);
        //ShopType.pant = 2
        equippedTypes.Add(UserData.Ins.playerAccessory);
        //ShopType.pant = 3
        equippedTypes.Add(UserData.Ins.playerSkin);
    }
}
