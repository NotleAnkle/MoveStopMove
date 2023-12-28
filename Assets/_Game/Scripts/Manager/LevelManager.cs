using _Framework.Event.Scripts;
using _Framework.Singleton;
using _Game.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Diagnostics;
using UnityEngine.UIElements;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private Player player;
    [SerializeField] private CameraFollow mainCamera;
    [SerializeField] private List<Bot> bots = new List<Bot>();

    public CameraFollow Camera => mainCamera;
    public Player Player => player;

    private int botNumber = 50;

    public void OnInit() 
    {
        player.OnInit();

        for (int i = 0; i < 8; i++)
        {
            SpawnBot();
        }

        this.RegisterListener(EventID.OnCharacterDie, _ => CheckLimit());
    }

    public void CheckLimit()
    {
        if(botNumber > 1)
        {
            SpawnBot();
        }
    }
    private void SpawnBot()
    {
        Bot bot = SimplePool.Spawn<Bot>(PoolType.Bot, Utilities.GetRandomPoint(Vector3.zero, 50f), Quaternion.identity);
        bot.OnInit();
        bots.Add(bot);

        botNumber--;
    }
}
