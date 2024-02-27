using _UI.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinShop : UICanvas
{
    [SerializeField] private List<Image> optionImgs;
    [SerializeField] private List<GameObject> optionPanels;

    private void OnEnable()
    {
        OpenPanel(0);
    }

    public void OpenPanel(int index)
    {
        CloseAllPanel();
        optionImgs[index].enabled = false;
        optionPanels[index].SetActive(true);
    }

    public void CloseAllPanel() 
    {
        for(int i = 0; i < optionPanels.Count; i++)
        {
            optionImgs[i].enabled = true;
            optionPanels[i].SetActive(false);
        }
    }

    public void Back()
    {
        UIManager.Instance.CloseUI <SkinShop> ();
        UIManager.Instance.OpenUI<MainMenu>();
    }
}
