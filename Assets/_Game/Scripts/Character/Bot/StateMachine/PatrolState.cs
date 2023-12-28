using _Framework.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState<Bot>
{
    public void OnEnter(Bot t)
    {
        t.SetRotationDefault();
        t.ChangeAnim(Constant.ANIM_RUN);
        t.ContinueMove();
        t.MoveToRandomPoint();
    }

    public void OnExecute(Bot t)
    {
        if (t.IsDestination)
        {
            t.MoveToRandomPoint();
        }
        if(t.IsHasTarget)
        {
            t.ChangeState(new AttackState());
        }
    }

    public void OnExit(Bot t)
    {
        //t.ChangeAnim(Constant.ANIM_IDLE);
    }
}
