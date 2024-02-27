using _Framework.Event.Scripts;
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

    private void Awake()
    {
        RegisterEvent();
    }

    private void OnEnable()
    {
        botNumberLeft = 50;
        botNumberText.text = "Alive: " + botNumberLeft;
    }

    private void RegisterEvent()
    {
        this.RegisterListener(EventID.OnCharacterDie, _ => OnBotDie());
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
}
