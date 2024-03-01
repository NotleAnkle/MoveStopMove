using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NameData", menuName = "ScriptableObjects/NameData", order = 4)]
public class NameData : ScriptableObject
{
    [SerializeField] List<string> names;

    public string getRandomName()
    {
        int index = Random.Range(0, names.Count);
        string name = names[index];

        return name;
    }
}
