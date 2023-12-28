using _UI.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShop : UICanvas
{
    [SerializeField] GameObject[] panels;

    private int curIndex = 0;
    private void OnEnable()
    {
        panels[0].SetActive(true);
    }

    public void Back()
    {
        CloseAll();
        UIManager.Instance.CloseUI<WeaponShop>();
        UIManager.Instance.OpenUI<MainMenu>();
    }

    private void CloseAll()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
    }

    public void NextPanel()
    {
        if(curIndex < panels.Length - 1)
        {
            panels[curIndex].SetActive(false);
            curIndex++;
            panels[curIndex].SetActive(true);
        }
    }

    public void PrePanel()
    {
        if(curIndex > 0)
        {
            panels[curIndex].SetActive(false);
            curIndex--;
            panels[curIndex].SetActive(true);
        }
    }
}
