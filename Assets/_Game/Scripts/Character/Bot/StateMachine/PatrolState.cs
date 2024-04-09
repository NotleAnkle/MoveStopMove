using _Framework.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState<Bot>
{
    private float targetCooldown;
    public void OnEnter(Bot t)
    {
        t.SetRotationDefault();
        t.ChangeAnim(Constant.ANIM_RUN);
        t.ContinueMove();
        t.MoveToRandomPoint();
    }

    public void OnExecute(Bot t)
    {
        targetCooldown = targetCooldown > 0f ? (targetCooldown - Time.deltaTime) : -1f;
        if (t.IsDestination)
        {
            if (Random.Range(0, 1f) > Constant.BOT_RATION_IDLE)
            {
                t.MoveToRandomPoint();
            }
            else
            {
                t.ChangeState(new IdleState());
            }
        }
        if (t.IsHasTarget && targetCooldown < 0.0001f)
        {
            if (Random.Range(0, 1f) > Constant.BOT_RATION_ATTACK)
            {
                t.ChangeState(new AttackState());
            }
            else
            {
                targetCooldown = 1f;
            }
        }
    }

    public void OnExit(Bot t)
    {
        //t.ChangeAnim(Constant.ANIM_IDLE);
    }
}
