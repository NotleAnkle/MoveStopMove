using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 5)]
public class LevelData : ScriptableObject
{
    [SerializeField] SingleLevelData[] datas;
    public SingleLevelData GetLevel(int index)
    {
        return datas[index];
    }
}
[System.Serializable]
public class SingleLevelData
{
    public int totalBotNumber;
    public int inMapBotNumber;
    public GameObject map;
}
