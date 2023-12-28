using _Framework.Event.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantButton : ItemButton
{
    [SerializeField] private PantType pantType;
    public override void TaskOnClick()
    {
        this.PostEvent(EventID.OnPlayerPantChange, pantType);
    }
}
