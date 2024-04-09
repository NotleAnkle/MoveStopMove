using _Framework.Event.Scripts;
using _UI.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIPlay : UICanvas
{
    [SerializeField] private GameObject tutorial;
    [SerializeField] private Text botNumberText;
    private int botNumberLeft = 50;
    float timer = 0f;

    public override void Open()
    {
        base.Open();
        botNumberLeft = LevelManager.Instance.BotNumberLeft;
        botNumberText.text = "Alive: " + botNumberLeft;
        GameManager.ChangeState(GameState.GamePlay);
    }

    public  void OnCharacterDie()
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
        UIManager.Instance.OpenUI<UISetting>();
    }
}
