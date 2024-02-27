using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIRevive : UICanvas
{
    [SerializeField] private Text sec;
    private float timer = 5f;
    private void OnEnable()
    {
        timer = 5f;
        sec.text = timer.ToString();
    }
    public void OnReject()
    {
        UIManager.Instance.CloseUI<UIRevive>();
        UIManager.Instance.OpenUI<UIRank>();
    }

    public void OneSecPass()
    {
        timer -= 1f;
        sec.text = timer.ToString();
        if(timer < 0.0001f)
        {
            OnReject();
        }
    }
}
