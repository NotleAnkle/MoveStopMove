using _UI.Scripts;
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

    [SerializeField] private GameObject panelRank;
    [SerializeField] private GameObject panelVictory;

    private void OnEnable()
    {
        txtCoin.text = LevelManager.Instance.Player.Point.ToString();
        UIManager.Instance.CloseUI<PlayUI>();
    }

    public void OnVictory()
    {
        panelRank.SetActive(false);
        panelVictory.SetActive(true);
    }
    public void OnFail()
    {
        panelRank.SetActive(true);
        panelVictory.SetActive(false);
        txtRank.text = "#" + LevelManager.Instance.PlayerRank.ToString();
        txtName.text = LevelManager.Instance.Player.killerName.ToString();
    }

    public void OnTouch()
    {
        UIManager.Instance.CloseUI<UIRank>();
        UIManager.Instance.OpenUI<MainMenu>();
    }
}
