﻿using _Framework.Event.Scripts;
using _UI.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Player : Character
{
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private AttackRange attackRange;
    [SerializeField] private ParticleSystem reviveVFX;

    private bool isAttacking = false, isAttackReadyAfterMove = true;

    #region override
    private void Start()
    {
        EquipedCloth();
    }
    public override void OnInit()
    {
        SumCoin();
        base.OnInit();
        SetSize(Constant.RANGE_DEFAULT);
        attackRange.OnInit();
        indicator.SetName("You");
        SetRotationDefault();
        TakeOffTryClothes();
    }

    public override void PowerUp()
    {
        base.PowerUp();
        attackRange.OnInit();
        float rate = (this.Range - Constant.RANGE_DEFAULT) / (Constant.RANGE_MAX - Constant.RANGE_DEFAULT);
        CameraFollower.Instance.SetRateOffset(rate);
        SoundManager.Instance.Play(AudioType.SFX_SizeUp);
    }

    public override void OnDeath()
    {
        base.OnDeath();
        SoundManager.Instance.Play(AudioType.SFX_PlayerDie);
    }

    public void OnRevive()
    {
        reviveVFX.Play();
        IsDying = false;
        indicator.SetAlpha(1);
        ChangeAnim(Constant.ANIM_IDLE);
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
                CheckTargetList();
                if (IsHasTarget)
                {
                    if (IsAttackable && isAttackReadyAfterMove)
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
        isAttackReadyAfterMove = true;

        Vector2 deltaPos = joystick.Direction.normalized * speed * Time.deltaTime;
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
        isAttacking = true;
        ChangeAnim(Constant.ANIM_ATTACK);
        Vector3 pos = GetFirstTargetPos();
        TurnTo(pos);
        StartCoroutine(Throw(pos));
    }
    public void CancelAttack()
    {
        isAttacking = false;
    }
    private IEnumerator Throw(Vector3 target)
    {
        yield return new WaitForSeconds(Constant.TIME_ATTACK_DELAY);
        if (isAttacking)
        {
            curWeapon.Throw(target);
            SoundManager.Instance.Play(AudioType.SFX_ThrowWeapon);
            isAttackReadyAfterMove = false;
        }
        isAttacking = false;

        yield return new WaitForSeconds(0.1f);
        Idle();
    }

    //quay model theo joystick
    private void Turn()
    {
        currentSkin.TF.forward = new Vector3(joystick.Direction.x, 0f, joystick.Direction.y);
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

    #region skin
    public override void EquipedCloth()
    {
        base.EquipedCloth();
        ChangeSkin(UserData.Ins.playerSkin);
        WearEquipedCloth();
    }
    public void TryCloth(UIShop.ShopType shopType, Enum type)
    {
        switch (shopType)
        {
            case UIShop.ShopType.hair:
                currentSkin.DespawnHair();
                currentSkin.ChangeHair((HairType)type);
                break;
            case UIShop.ShopType.pant:
                currentSkin.ChangePant((PantType)type);
                break;
            case UIShop.ShopType.accessory:
                currentSkin.DespawnAccessory();
                currentSkin.ChangeAccessory((AccessoryType)type);
                break;
            case UIShop.ShopType.skin:
                TakeOffCloth();
                ChangeSkin((SkinType)type);
                WearEquipedCloth();
                break;
            case UIShop.ShopType.weapon:
                currentSkin.DespawnWeapon();
                ChangeWeapon((WeaponType)type);
                break;
        }
    }

    private HairType hairType;
    private PantType pantType;
    private AccessoryType accessoryType;
    private SkinType skinType;

    //Mac trang bi duoc luu
    private void GetEquippedData()
    {
        hairType = UserData.Ins.playerHair;
        pantType = UserData.Ins.playerPant;
        accessoryType = UserData.Ins.playerAccessory;
        skinType = UserData.Ins.playerSkin;
    }
    public void WearEquipedCloth()
    {
        GetEquippedData();
        
        ChangeHair(hairType);

        ChangePant(pantType);

        ChangeAccessory(accessoryType);

        ChangeWeapon(UserData.Ins.playerWeapon); 
    }
    //Thao cac trang bi thu(try)
    private void TakeOffTryClothes()
    {
        GetEquippedData();

        if (IsTry(hairType))
        {
            currentSkin.DespawnHair();
            UserData.Ins.SetEnumData(UserData.Key_Player_Hair, ref UserData.Ins.playerHair, HairType.None);
        }
        if (IsTry(pantType))
        {
            ChangePant(PantType.BatMan);
            UserData.Ins.SetEnumData(UserData.Key_Player_Pant, ref UserData.Ins.playerPant, PantType.BatMan);
        }
        if (IsTry(accessoryType))
        {
            currentSkin.DespawnAccessory();
            UserData.Ins.SetEnumData(UserData.Key_Player_Accessory, ref UserData.Ins.playerAccessory, AccessoryType.None);
        }
        if (IsTry(skinType))
        {
            TakeOffCloth();
            UserData.Ins.SetEnumData(UserData.Key_Player_Skin, ref UserData.Ins.playerSkin, SkinType.Normal);
            ChangeSkin(SkinType.Normal);
            WearEquipedCloth();
        }
    }
    private bool IsTry(Enum type)
    {
        ShopItem.State state = UserData.Ins.GetEnumData(type.ToString(), ShopItem.State.Lock);
        return (state == ShopItem.State.Lock);
    }
    #endregion

    #region other
    private void SumCoin()
    {
        int coin = UserData.Ins.coin;
        coin += Score;
        UserData.Ins.SetIntData(UserData.Key_Coin,ref UserData.Ins.coin, coin);
    }

    public void TripleScore()
    {
        score *= 3;
    }
    public void OnGameStart()
    {
        indicator.SetAlpha(1);
    }
    #endregion
}
