using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum HairType
{
    None = 0,
    Arrow = 1,
    Cowboy = 2,
    Crown = 3,
    Ear = 4,
    Flower = 5,
    Hair = 6,
    Hat = 7,
    Hat_Cap = 8,
    Hat_Yellow = 9,
    Headphone = 10,
    Horn = 11,
    Rau = 12,
}

[CreateAssetMenu(fileName = "HairData", menuName = "ScriptableObjects/HairData", order = 2)]
public class HairData : ScriptableObject
{
    [SerializeField] GameObject[] Prefabs;

    public GameObject GetPrefab(HairType hairType)
    {
        return Prefabs[(int)hairType];
    }
}
