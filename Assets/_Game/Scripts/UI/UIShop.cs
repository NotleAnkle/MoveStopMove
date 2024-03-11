using _Framework;
using _UI.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : UICanvas
{
    public enum ShopType { hair, pant, accessory, skin, weapon}
    [SerializeField] private ShopData data;
    [SerializeField] private GameObject contentPanel;

    [SerializeField] private Text txtCoin;
    [SerializeField] private Text txtCost;
    [SerializeField] private ShopItemBar[] bars;
    [SerializeField] private GameObject[] buttons;

    private ShopItemBar curBar;
    private ShopItem curItem;
    private ShopType shopType;

    private ShopItem itemEquiped;

    [SerializeField] private ShopItem prefab; 
    MiniPool<ShopItem> miniPool = new MiniPool<ShopItem>();

    private void Awake()
    {
        miniPool.OnInit(prefab, 12, contentPanel.transform);

        for (int i = 0; i < bars.Length; i++)
        {
            bars[i].SetShop(this);
        }
    }
    public override void Open()
    {
        base.Open();
        curBar = bars[0];
        SelectBar(curBar);

        txtCoin.text = UserData.Ins.coin.ToString();
        CameraFollower.Instance.ChangeState(CameraFollower.State.Shop);
    }
    internal void SelectBar(ShopItemBar selectBar)
    {
        if(selectBar != null)
        {
            curBar.Active(false);
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
        ShopItem.State state = UserData.Ins.GetEnumData(item.Type.ToString(), ShopItem.State.Buy);
        item.SetState(state);
        curItem = item;
        SetButtonState(item);
    }

    private void SetButtonState(ShopItem item)
    {
        switch (item.state)
        {
            case ShopItem.State.Buy:
                SetButton(0);
                txtCost.text = item.Cost.ToString();
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
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
        buttons[index].SetActive(true);
    }

    public void ButtonClick()
    {
        SoundManager.Instance.Play(AudioType.SFX_ButtonClick);
        switch (curItem.state)
        {
            case ShopItem.State.Buy:
                OnBuyButtonClick();
                break;
            case ShopItem.State.Bought:
                OnEquipButtonClick();
                break;
            case ShopItem.State.Equipped:
                //UserData.Ins.SetEnumData(curItem.Type.ToString(), ShopItem.State.Bought);
                //SelectItem(curItem);
                break;
        }
    }

    private void OnBuyButtonClick()
    {
        SoundManager.Instance.Play(AudioType.SFX_ButtonClick);
        if (UserData.Ins.coin >= curItem.Cost)
        {
            int coin = UserData.Ins.coin - curItem.Cost;
            UserData.Ins.SetIntData(UserData.Key_Coin, ref UserData.Ins.coin, coin);
            UserData.Ins.SetEnumData(curItem.Type.ToString(), ShopItem.State.Bought);
            SelectItem(curItem);
            SetButton(1);
        }

    }

    private void OnEquipButtonClick()
    {
        SoundManager.Instance.Play(AudioType.SFX_ButtonClick);

        if (curItem != null)
        {
            UserData.Ins.SetEnumData(curItem.Type.ToString(), ShopItem.State.Equipped);
            if (itemEquiped)
            {
                UserData.Ins.SetEnumData(itemEquiped.Type.ToString(), ShopItem.State.Bought);
                itemEquiped.SetState(ShopItem.State.Bought);
                itemEquiped = curItem;
            }
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
            SelectItem(itemEquiped);
            SetButton(2);
        }
    }

    private void InitItemFrames<T>(List<ShopItemData<T>> items) where T : System.Enum
    {
        for(int i = 0; i < items.Count; i++)
        {
            ShopItem.State state = UserData.Ins.GetEnumData(items[i].type.ToString(), ShopItem.State.Buy);
            ShopItem item = miniPool.Spawn();
            item.SetState(state);
            item.SetData<T>(items[i], this);

            if(item.state == ShopItem.State.Equipped)
            {
                itemEquiped = item;
                item.SetState(ShopItem.State.Equipped);
            }
        }
    }

    public void Back()
    {
        curBar.Active(false);
        CloseDirectly();
        SoundManager.Instance.Play(AudioType.SFX_ButtonClick);
        UIManager.Instance.OpenUI<MainMenu>();
    }
}
