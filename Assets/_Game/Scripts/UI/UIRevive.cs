using _UI.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIRevive : UICanvas
{
    [SerializeField] private Text sec;
    private float timer = 5f;
    public override void Open()
    {
        base.Open();
        timer = 5f;
        sec.text = timer.ToString();
    }
    public void OnReject()
    {
        CloseDirectly();
        SoundManager.Instance.Play(AudioType.SFX_ButtonClick);
        UIManager.Instance.OpenUI<UIEndGame>().OnFail();
    }

    public void OneSecPass()
    {
        SoundManager.Instance.Play(AudioType.SFX_Count);
        timer -= 1f;
        sec.text = timer.ToString();
        if(timer < 0.0001f)
        {
            OnReject();
        }
    }

    public void OnAccept()
    {
        LevelManager.Instance.OnRevive();
        CloseDirectly();
    }
}
