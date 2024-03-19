using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public enum State { Buy, Bought, Equipped, Try}
    [SerializeField] private Image img;
    [SerializeField] private GameObject lockImg;
    [SerializeField] private GameObject equipImg;
    [SerializeField] private Button bt;
    private int cost;
    public int Cost => cost;

    public State state;
    public Enum Type;
    UIShop shop;

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
        equipImg.SetActive(false);
        switch (state)
        {
            case State.Buy:
                lockImg.SetActive(true);
                break;
            case State.Bought:
                lockImg.SetActive(false);
                break;
            case State.Equipped:
                bt.Select();
                lockImg.SetActive(false);
                equipImg.SetActive(true);
                break;
            case State.Try:
                bt.Select();
                lockImg.SetActive(true);
                equipImg.SetActive(true);
                break;
        }
        this.state = state;
    }
}
