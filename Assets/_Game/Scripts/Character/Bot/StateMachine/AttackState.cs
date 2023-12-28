using _Framework.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState<Bot>
{
    private float timer;
    public void OnEnter(Bot t)
    {
        t.StopMove();
        t.Attack();
    }

    public void OnExecute(Bot t)
    {
        timer += Time.deltaTime;
        if (!t.IsHasTarget)
        {
            t.ChangeState(new PatrolState());
        }

        if(timer > 0.5f)
        {
            t.ChangeState(new IdleState());
        }
    }

    public void OnExit(Bot t)
    {
    }
}
