using _Framework;
using _Framework.Event.Scripts;
using _Framework.StateMachine;
using _Game.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    [SerializeField] GameObject targetCircle;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] BotAttackRange attackRange;
    [SerializeField] NameData nameData;

    private Vector3 targetPos;
    private IState<Bot> curState;

    public bool IsDestination => Vector3.Distance(targetPos, TF.position) < 0.1f;

    #region override
    public override void OnInit()
    {
        base.OnInit();
        attackRange.OnInit(this);
        agent.speed = speed;
        curState = new IdleState();
        TurnOffTargetCircle();
        indicator.SetName(nameData.getRandomName());
        ChangeSkin(Utilities.RandomEnumValue<SkinType>());
        GetRandomScore();
    }
    public override void OnDeath()
    {
        base.OnDeath();
        ChangeState(new DeadState());
    }
    public override void OnDespawn()
    {
        SimplePool.Despawn(this);
    }
    public override void PowerUp()
    {
        base.PowerUp();
        attackRange.OnInit(this);
    }
    public override void EquipedCloth()
    {
        base.EquipedCloth();
        WearRandomCloth();
    }
    public void WearRandomCloth()
    {
        ChangeHair(Utilities.RandomEnumValue<HairType>());
        ChangePant(Utilities.RandomEnumValue<PantType>());
        ChangeAccessory(Utilities.RandomEnumValue<AccessoryType>());
        ChangeWeapon(Utilities.RandomEnumValue<WeaponType>());
    }
    #endregion

    void Update()
    {
        if (curState != null)
        {
            curState.OnExecute(this);
        }
    }

    #region action
    public void Attack()
    {
        if (curWeapon.IsActive)
        {
            IsAttackable = false;
            ChangeAnim(Constant.ANIM_ATTACK);
            Vector3 pos = GetFirstTargetPos();
            TurnTo(pos);
            StartCoroutine(Throw(pos));
        }
    }
    private IEnumerator Throw(Vector3 target)
    {
        yield return new WaitForSeconds(Constant.TIME_ATTACK_DELAY);
        if (curWeapon.IsActive)
        {
            curWeapon.Throw(target);
        }
    }

    public void MoveTo(Vector3 pos)
    {
        ContinueMove();
        targetPos = pos;
        agent.SetDestination(pos);
    }
    public void ChangeState(IState<Bot> state)
    {
        if (curState != null)
        {
            curState.OnExit(this);
        }

        curState = state;

        if (curState != null)
        {
            curState.OnEnter(this);
        }
    }
    #endregion

    public void TurnOnTargetCircle()
    {
        targetCircle?.SetActive(true);
    }
    public void TurnOffTargetCircle()
    {
       targetCircle?.SetActive(false);
    }

    #region navmesh
    public void StopMove()
    {
        agent.enabled = false;
    }

    public void ContinueMove()
    {
        agent.enabled = true;
    }
    public Vector3 GetRandomPoint()
    {
        Vector3 center = Vector3.zero;
        float maxDistance = 50f;
        // Get Random Point inside Sphere which position is center, radius is maxDistance
        Vector3 randomPos = Random.insideUnitSphere * maxDistance + center;

        NavMeshHit hit; // NavMesh Sampling Info Container

        // from randomPos find a nearest point on NavMesh surface in range of maxDistance
        NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas);

        return (hit.position);
    }

    public void MoveToRandomPoint()
    {
        MoveTo(GetRandomPoint());
    }
    #endregion

}
