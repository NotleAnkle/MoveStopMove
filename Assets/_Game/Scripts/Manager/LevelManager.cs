using _Framework.Event.Scripts;
using _Framework.Pool.Scripts;
using _Framework.Singleton;
using _Game.Utils;
using _UI.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Diagnostics;
using UnityEngine.UIElements;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private Player player;
    [SerializeField] private List<Bot> bots = new List<Bot>();

    public Player Player => player;

    private int botNumber = 50;

    public int PlayerRank;
    public string PlayerKiller;

    private void Awake()
    {
        this.RegisterListener(EventID.OnCharacterDie, (param) => CheckLimit((Character)param));
    }

    public void OnInit() 
    {
        for (int i = 0; i < 8; i++)
        {
            SpawnBot();
        }
    }
    public void OnReset()
    {
        player.OnInit();
        bots.Clear();
        botNumber = 50;
    }

    public void CheckLimit(Character character)
    {
        if (character != player)
        {
            bots.Remove((Bot)character);
        }
        else
        {
            PlayerRank = bots.Count + botNumber;
        }
        if (GameManager.IsState(GameState.GamePlay))
        {
            if(botNumber > 1)
            {
                SpawnBot();
            }
            else
            {
                if(bots.Count < 1)
                {
                    OnVictory();
                }
            }
        }
        
        
    }
    private void SpawnBot()
    {
        Bot bot = SimplePool.Spawn<Bot>(PoolType.Bot, Utilities.GetRandomPoint(Vector3.zero, 50f), Quaternion.identity);
        bot.OnInit();
        bots.Add(bot);
        botNumber--;
    }

    public void SetTargetIndicatorAlpha(float alpha)
    {
        List<GameUnit> list = SimplePool.GetAllUnitIsActive(PoolType.TargetIndicator);

        for (int i = 0; i < list.Count; i++)
        {
            (list[i] as TargetIndicator).SetAlpha(alpha);
        }
    }

    public void OnFail()
    {
        UIManager.Instance.CloseAll();
        GameManager.ChangeState(GameState.Revive);
        StartCoroutine(FailCountdown());
    }

    private IEnumerator FailCountdown()
    {
        yield return new WaitForSeconds(0.5f);
        UIManager.Instance.OpenUI<UIRevive>();
    }

    public void OnVictory()
    {
        GameManager.ChangeState(GameState.Revive);
        SetTargetIndicatorAlpha(0);
        UIManager.Instance.OpenUI<UIRank>().OnVictory();
        player.ChangeAnim(Constant.ANIM_DANCE_WIN);
    }
}
