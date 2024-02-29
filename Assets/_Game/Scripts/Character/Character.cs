using _Framework.Event.Scripts;
using _Framework.Pool.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Character : GameUnit
{
    [SerializeField] private Animator anim;
    [SerializeField] protected Transform model;
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected Transform rightHand;
    [SerializeField]protected Skin currentSkin;

    #region components
    [SerializeField] private float range;
    private int point = 0;
    public Vector3 throwPoint => rightHand.position;

    public float Range
    {
        get => range;
        set => SetRange(value);
    }

    public void SetRange(float value)
    {
        range = value;
    }

    public void SetRotationDefault()
    {
        model.localRotation = Quaternion.identity;
    }

    [SerializeField] private List<Character> targetList = new List<Character>();

    private string currentAnimName;

    public int Point => point;
    public bool IsHasTarget => targetList.Count > 0;
    public bool IsAttackable = true;
    public bool IsDying = false;

    protected Weapon curWeapon => currentSkin.CurWeapon;
    #endregion

    #region basic
    public virtual void OnInit()
    {
        targetList.Clear();
        model.transform.localScale = Vector3.one;
        point = 0;
        range = Constant.RANGE_DEFAULT;
        TryCloth(UIShop.ShopType.weapon, WeaponType.Kinfe);
        IsDying = false;
        ChangeAnim(Constant.ANIM_IDLE);

        this.RegisterListener(EventID.OnCharacterDie, (param) => RemoveTarget((Character)param));
    }
    public virtual void OnDeath()
    {
        this.PostEvent(EventID.OnCharacterDie, this);
        ChangeAnim(Constant.ANIM_DEAD);
        IsDying = true;
    }
    public virtual void OnDespawn()
    {
    }
    public void ChangeAnim(string animName)
    {
        if (currentAnimName != animName && !IsDying)
        {
            anim.ResetTrigger(animName);

            if (currentAnimName != null)
            {
                anim.ResetTrigger(currentAnimName);
            }

            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }
    #endregion

    #region target
    public void AddTarget(Character character)
    {
        if(!targetList.Contains(character))
        {
            targetList.Add(character);
        }
    }

    public void RemoveTarget(Character character)
    {
        targetList.Remove(character);
    }

    public Vector3 GetFirstTargetPos()
    {
        return targetList[0].TF.position + Vector3.up;
    }
    #endregion

    #region skin
    public void WeaponEnable()
    {
        curWeapon.gameObject.SetActive(true);
    }
    public void WeaponDisable()
    {
        curWeapon.gameObject.SetActive(false);
    }

    public void TryCloth(UIShop.ShopType shopType,Enum type) 
    {
        switch (shopType)
        {
            case UIShop.ShopType.hair:
                currentSkin.ChangeHair((HairType)type);
                break;
            case UIShop.ShopType.pant:
                currentSkin.ChangePant((PantType)type);
                break;
            case UIShop.ShopType.accessory:
                currentSkin.ChangeAccessory((AccessoryType)type);
                break;
            case UIShop.ShopType.skin:

                break;
            case UIShop.ShopType.weapon:
                currentSkin.ChangeWeapon((WeaponType)type);
                curWeapon.OnInit(this);
                break;
        }
    }

    public virtual void EquipedCloth()
    {
        
    }
    #endregion



    #region powerUp
    public void IncresingPoint(int enemyPoint)
    {
        enemyPoint = enemyPoint < 1 ? 1 : enemyPoint;
        point += enemyPoint;
        PowerUp();

        if (point % 5 == 0)
        {
            PowerUp();
        }
    }
    public virtual void PowerUp()
    {
        range += 1;
        model.transform.localScale = Vector3.one * range / Constant.RANGE_DEFAULT;
    }
    #endregion

}
