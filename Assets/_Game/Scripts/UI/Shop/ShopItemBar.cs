using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemBar : MonoBehaviour
{
    [SerializeField] private Image bg;
    [SerializeField] private UIShop.ShopType type;

    UIShop shop;

    public UIShop.ShopType Type => type;

    public void SetShop(UIShop shop)
    {
        this.shop = shop;
    }

    public void Selected()
    {
        shop.SelectBar(this);
    }

    public void Active(bool activeState)
    {
        bg.enabled = !activeState;
    }
}
