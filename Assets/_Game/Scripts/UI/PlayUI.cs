using _Framework.Event.Scripts;
using _UI.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : UICanvas
{
    [SerializeField] private GameObject tutorial;
    [SerializeField] private Text botNumberText;
    private int botNumberLeft = 50;
    float timer = 0f;

    //dang ky event
    private void Awake()
    {
        this.RegisterListener(EventID.OnCharacterDie, _ => OnBotDie());
    }

    public override void Open()
    {
        base.Open();
        botNumberLeft = LevelManager.Instance.BotNumberLeft;
        botNumberText.text = "Alive: " + botNumberLeft;
        LevelManager.Instance.SetTargetIndicatorAlpha(1);
        GameManager.ChangeState(GameState.GamePlay);
    }

    private void OnBotDie()
    {
        botNumberLeft--;
        botNumberText.text = "Alive: " + botNumberLeft;
    }

    private void Update()
    {
        if(timer < 2f)
        {
            if (Input.GetMouseButton(0))
            {
                timer += Time.deltaTime;
                tutorial.SetActive(false);
            }
            else
            {
                tutorial.SetActive(true);
            }
        }
    }

    public void OnSettingButtonClick()
    {
        CloseDirectly();
        UIManager.Instance.OpenUI<UISetting>();
    }
}
