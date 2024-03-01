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
        GameManager.ChangeState(GameState.Revive);
        LevelManager.Instance.SetTargetIndicatorAlpha(0);
    }
    public void OnReject()
    {
        UIManager.Instance.CloseUI<UIRevive>();
        UIManager.Instance.OpenUI<UIRank>().OnFail();
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
