using _UI.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRank : UICanvas
{
    [SerializeField] private Text txtRank;
    [SerializeField] private Text txtName;
    [SerializeField] private Text txtCoin;

    private void OnEnable()
    {
        txtRank.text = "#" + LevelManager.Instance.PlayerRank.ToString();
    }

    public void OnTouch()
    {
        UIManager.Instance.CloseUI<UIRank>();
        GameManager.Instance.OnRestart();
    }
}
