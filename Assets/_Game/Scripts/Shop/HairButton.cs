using _Framework.Event.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairButton : ItemButton
{
    [SerializeField] private HairType hairType;
    public override void TaskOnClick()
    {
        //LevelManager.Instance.Player.ChangeHair(hairType);
        
        this.PostEvent(EventID.OnPlayerHairChange, hairType);
    }
}
