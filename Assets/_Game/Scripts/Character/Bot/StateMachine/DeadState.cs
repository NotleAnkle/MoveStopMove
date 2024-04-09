using _Framework.StateMachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeadState : IState<Bot>
{
    private float timer = 0f;
    public void OnEnter(Bot t)
    {
        t.StopMove();
        t.ChangeAnim(Constant.ANIM_DEAD);
        t.TurnOffTargetCircle();
    }

    public void OnExecute(Bot t)
    {
        timer += Time.deltaTime;
        if(timer > 1f)
        {
            t.OnDespawn();
        }
        
    }

    public void OnExit(Bot t)
    {
        
    }
}
