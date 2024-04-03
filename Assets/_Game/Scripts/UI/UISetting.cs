using _UI.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISetting : UICanvas
{
    [SerializeField] private GameObject[] sound;
    [SerializeField] private GameObject[] vibration;

    // 0: off, 1: on

    public override void Open()
    {
        base.Open();
        SetObj(sound, UserData.Ins.soundIsOn);
        SetObj(vibration, UserData.Ins.vibrate);
    }

    public void OnSwitchSound()
    {
        bool isSoundOn = !UserData.Ins.soundIsOn;
        SetObj(sound, isSoundOn);

        UserData.Ins.SetBoolData(UserData.Key_SoundIsOn, ref UserData.Ins.soundIsOn, isSoundOn);
        SoundManager.Instance.Play(AudioType.SFX_ButtonClick);
    }
    private void SetObj(GameObject[] objs, bool isOn)
    {
        //isOn: 1.active + 0.deactive, !isOn: 1.deactive + 0.active
        objs[isOn ? 1 : 0].SetActive(true);
        objs[isOn ? 0 : 1].SetActive(false);
    }
    public void OnSwitchVir()
    {
        bool isVirationOn = !UserData.Ins.vibrate;
        SetObj(vibration, isVirationOn);
        UserData.Ins.SetBoolData(UserData.Key_Vibrate, ref UserData.Ins.vibrate, isVirationOn);
    }
    public void OnHomeButtonClick()
    {
        CloseDirectly();
        UIManager.Instance.OpenUI<UIMainMenu>();
    }

    public void OnContinueButtonClick()
    {
        CloseDirectly();
        UIManager.Instance.OpenUI<UIPlay>();
    }
}
