using _Framework;
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
    [SerializeField] private LevelData levelData;

    public Player Player => player;
    public int BotNumberLeft => botNumber + bots.Count;

    public int PlayerRank { get; private set;}

    private SingleLevelData curLevelData;
    private GameObject curMap;
    private int botNumber = 50;
    private int curLevel = 1;

    private void Start()
    {
        curLevelData = levelData.GetLevel(0);
        curMap = Instantiate(curLevelData.map);
    }

    public void OnInit() 
    {
        player.OnGameStart();
        for (int i = 0; i < curLevelData.inMapBotNumber; i++)
        {
            SpawnBot();
        }
    }

    public void OnNextLevel()
    {
        if (curLevel < 3)
        {
            curLevel++;
        }
        else curLevel = 1;
        
        curLevelData = levelData.GetLevel(curLevel - 1);

        Destroy(curMap.gameObject);
        curMap = Instantiate(curLevelData.map);

        ClearCache();
    }

    public void OnReset()
    {
        DespawnBot();
        player.OnInit();
        bots.Clear();
        botNumber = curLevelData.totalBotNumber;
        SetTargetIndicatorAlpha(0);
    }

    //kiem tra character duoc event tra ve
    public void CheckCharacterDie(Character character)
    {
        UIManager.Instance.GetUI<UIPlay>().OnCharacterDie();
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
            OnFail();
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
        UIManager.Instance.CloseUI<UISetting>();
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
        UIManager.Instance.OpenUI<UIEndGame>().OnVictory();
        player.ChangeAnim(Constant.ANIM_DANCE_WIN);
    }

    public void OnRevive()
    {
        GameManager.ChangeState(GameState.GamePlay);
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

    private void ClearCache()
    {
        Cache<Bullet>.Clear();
        Cache<Bot>.Clear();
        Cache<Character>.Clear();
    }
}
