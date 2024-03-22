using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public enum State {Lock = 0, Unlock = 1}
    [SerializeField] private Image img;
    [SerializeField] private GameObject lockImg;
    [SerializeField] private GameObject equipImg;
    [SerializeField] private Button bt;
    private int cost;
    public int Cost => cost;

    public State state;
    public Enum Type;
    UIShop shop;

    private void OnEnable()
    {
        SetEquipped(false);
    }
    public void SetShop(UIShop shop)
    {
        this.shop = shop;
    }

    public void Selected()
    {
        shop.SelectItem(this);
    }

    public void SetData<T>(ShopItemData<T> data, UIShop shop) where T : Enum
    {
        img.sprite = data.icon;
        cost = data.cost;
        Type = data.type;
        this.shop = shop;
    }
    public void SetState(State state)
    {
        SetLock(state == State.Lock);
        this.state = state;
    }
    public void SetEquipped(bool isEquipped)
    {
        equipImg.SetActive(isEquipped);
    }

    private void SetLock(bool isLock)
    {
        lockImg.SetActive(isLock);
    }
    public void SetSelect()
    {
        bt.Select();
    }
}
