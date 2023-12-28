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
    [SerializeField] protected Weapon curWeapon;
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected WeaponData weaponData;
    [SerializeField] protected HairData hairData;
    [SerializeField] protected PantData pantData;
    [SerializeField] protected Transform rightHand;
    [SerializeField] protected Transform head;
    [SerializeField] protected Renderer pant;
    private GameObject curHair;

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
    #endregion

    #region basic
    public virtual void OnInit()
    {
        ChangeAnim(Constant.ANIM_IDLE);
        targetList.Clear();
        model.transform.localScale = Vector3.one;
        point = 0;
        range = Constant.RANGE_DEFAULT;
        curWeapon.OnInit(this);
        IsDying = false;

        this.RegisterListener(EventID.OnCharacterDie, (param) => RemoveTarget((Character)param));
    }
    public virtual void OnDeath()
    {
        this.PostEvent(EventID.OnCharacterDie, this);
        IsDying = true;
    }
    public virtual void OnDespawn()
    {
    }
    public void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
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

    #region weapon
    public void WeaponEnable()
    {
        curWeapon.gameObject.SetActive(true);
    }
    public void WeaponDisable()
    {
        curWeapon.gameObject.SetActive(false);
    }

    public void ChangeWeapon(WeaponType type)
    {
        Destroy(curWeapon.gameObject);
        Weapon prefab = weaponData.GetWeaponPrefab(type);
        curWeapon = Instantiate(prefab, rightHand);
        curWeapon.OnInit(this);
    }
    #endregion

    #region skin
    public void ChangeHair(HairType hairType)
    {
        if (curHair != null)
        {
            Destroy(curHair.gameObject);
        }
        GameObject prefab = hairData.GetPrefab(hairType);
        curHair = Instantiate(prefab, head);
    }
    public void ChangePant(PantType pantType)
    {
        pant.material = pantData.GetPantMaterial(pantType);
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
