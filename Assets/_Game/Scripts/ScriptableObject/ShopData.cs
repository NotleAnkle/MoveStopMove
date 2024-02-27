using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.LightingExplorerTableColumn;

[CreateAssetMenu(fileName = "ShopData", menuName = "ScriptableObjects/ShopData", order = 1)]
public class ShopData : ScriptableObject
{
    public ShopItemDatas<HairType> hairs;
    public ShopItemDatas<PantType> pants;
    public ShopItemDatas<AccessoryType> accessories;
    public ShopItemDatas<SkinType> skins;
}

[System.Serializable]
public class ShopItemDatas<T> where T : System.Enum
{
    [SerializeField] List<ShopItemData<T>> listItem;
    public List<ShopItemData<T>> ListItem => listItem;
}

[System.Serializable]
public class ShopItemData<T> : ShopItemData where T : System.Enum
{
    public T type;
}
public class ShopItemData
{
    public Sprite icon;
    public int cost;
}