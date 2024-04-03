using _Framework.Event.Scripts;
using _Framework.Pool.Scripts;
using _Framework.Singleton;
using _Game.Utils;
using _UI.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private Player player;
    [SerializeField] private List<Bot> bots = new List<Bot>();

    public Player Player => player;

    private int botNumber = 50;

    public int BotNumberLeft => botNumber + bots.Count;

    public int PlayerRank { get; private set;}

    //dang ky event
    private void Awake()
    {
        this.RegisterListener(EventID.OnCharacterDie, (param) => CheckCharacter((Character)param));
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
        DespawnBot();
        player.OnInit();
        bots.Clear();
        botNumber = 50;
        SetTargetIndicatorAlpha(0);
    }

    //kiem tra character duoc event tra ve
    public void CheckCharacter(Character character)
    {
        if (character != player)
        {
            bots.Remove((Bot)character);
            if (botNumber > 1)
            {
                SpawnBot();
            }
            else
            {
                if (bots.Count < 1)
                {
                    OnVictory();
                }
            }
        }
        else
        {
            PlayerRank = bots.Count + botNumber;
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
        UIManager.Instance.OpenUI<UIRank>().OnVictory();
        player.ChangeAnim(Constant.ANIM_DANCE_WIN);
    }

    public void OnRevive()
    {
        GameManager.ChangeState(GameState.GamePlay);
        SetTargetIndicatorAlpha(1);
        Player.OnRevive();
        UIManager.Instance.OpenUI<UIPlay>();
    }

    private void DespawnBot()
    {
        for(int i = 0; i < bots.Count; i++)
        {
            bots[i].OnDespawn();
        }
    }
}
