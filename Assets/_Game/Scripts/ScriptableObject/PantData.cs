using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PantData", menuName = "ScriptableObjects/PantData", order = 3)]

public class PantData : ScriptableObject
{
    [SerializeField] Material[] pantMaterials;

    public Material GetPantMaterial(PantType pantType)
    {
        return pantMaterials[(int) pantType];
    }
}
