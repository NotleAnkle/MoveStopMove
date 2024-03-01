using _Framework.Event.Scripts;
using _UI.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Player : Character
{
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private AttackRange attackRange;

    private bool IsAttacking = false;

    #region override
    public override void OnInit()
    {
        SumCoin();
        base.OnInit();
        attackRange.OnInit();
        EquipedCloth();
        indicator.SetName("You");
        SetRotationDefault();
    }

    public override void PowerUp()
    {
        base.PowerUp();
        attackRange.OnInit();
        float rate = (this.Range - Constant.RANGE_DEFAULT) / (Constant.RANGE_MAX - Constant.RANGE_DEFAULT);
        CameraFollower.Instance.SetRateOffset(rate);
    }

    public override void OnDeath()
    {
        base.OnDeath();
        LevelManager.Instance.OnFail();
    }

    public override void EquipedCloth()
    {
        base.EquipedCloth();
        currentSkin.WearEquipedCloth();
        curWeapon.OnInit(this);
    }
    #endregion

    void Update()
    {
        if (GameManager.IsState(GameState.GamePlay))
        {
            if (joystick.Direction != Vector2.zero)
            {
                Run();
            }
            else
            {
                if (IsHasTarget)
                {
                    if (IsAttackable)
                    {
                        Attack();
                    }
                }
                else
                {
                    Idle();
                }
            }
        }
        
    }

    #region action
    private void Run()
    {
        CancelAttack();
        Vector2 deltaPos = joystick.Direction * speed * Time.deltaTime;
        Vector3 nextPos = new Vector3(deltaPos.x, 0f, deltaPos.y) + transform.position;

        transform.position = CheckGround(nextPos);
        ChangeAnim(Constant.ANIM_RUN);

        Turn();
    }
    private void Idle()
    {
        ChangeAnim(Constant.ANIM_IDLE);
    }
    public void Attack()
    {
        if (curWeapon.IsActive)
        {
            IsAttackable = false;
            IsAttacking = true;
            ChangeAnim(Constant.ANIM_ATTACK);
            Vector3 pos = GetFirstTargetPos();
            TurnTo(pos);
            StartCoroutine(Throw(pos));
        }
    }
    public void CancelAttack()
    {
        IsAttacking = false;
        IsAttackable = true;
    }
    private IEnumerator Throw(Vector3 target)
    {
        yield return new WaitForSeconds(Constant.TIME_ATTACK_DELAY);
        if (curWeapon.IsActive && IsAttacking)
        {
            curWeapon.Throw(target);
        }
        IsAttacking = false;

        yield return new WaitForSeconds(0.1f);
        Idle();
    }

    //quay mat ve phia joystick
    private void Turn()
    {
        model.transform.forward = new Vector3(joystick.Direction.x, 0f, joystick.Direction.y);
    }
    private Vector3 CheckGround(Vector3 nextPos)
    {
        RaycastHit hit;

        if (Physics.Raycast(nextPos + Vector3.up * 1.5f, Vector3.down, out hit, 2f, groundLayer))
        {
            return hit.point;
        }
        return TF.position;
    }
    #endregion

    private void SumCoin()
    {
        int coin = UserData.Ins.coin;
        coin += Point;
        UserData.Ins.SetIntData(UserData.Key_Coin,ref UserData.Ins.coin, coin);
    }
}
