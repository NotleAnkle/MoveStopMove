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
    [SerializeField] protected float speed = 5f;
    [SerializeField]protected Skin currentSkin;
    [SerializeField] protected Transform indicatorPoint;

    #region components
    [SerializeField] private float range;
    private int score = 0;
    public Vector3 throwPoint => currentSkin.RightHand.position;

    public float Range => range;

    [SerializeField] private List<Character> targetList = new List<Character>();

    private string currentAnimName;

    public int Score => score;
    public bool IsHasTarget => targetList.Count > 0;
    public bool IsAttackable = true;
    public bool IsDying = false;

    protected Weapon curWeapon => currentSkin.CurWeapon;

    protected TargetIndicator indicator;

    public string ownerName => indicator.Name;
    public string killerName;
    #endregion

    private void Awake()
    {
        this.RegisterListener(EventID.OnCharacterDie, (param) => RemoveTarget((Character)param));
    }

    #region basic
    public virtual void OnInit()
    {
        targetList.Clear();
        score = 0;
        range = Constant.RANGE_DEFAULT;
        ChangeWeapon(WeaponType.Kinfe);
        IsDying = false;
        ChangeAnim(Constant.ANIM_IDLE);

        if (!indicator)
        {
            indicator = SimplePool.Spawn<TargetIndicator>(PoolType.TargetIndicator);
        }
        indicator.SetTarget(indicatorPoint);
    }
    public virtual void OnDeath()
    {
        this.PostEvent(EventID.OnCharacterDie, this);
        ChangeAnim(Constant.ANIM_DEAD);
        IsDying = true;
        indicator.SetAlpha(0);
    }
    public virtual void OnDespawn()
    {
    }
    public void ChangeAnim(string animName)
    {
        if (currentAnimName != animName && !IsDying)
        {
            currentSkin.Anim.ResetTrigger(animName);

            if (currentAnimName != null)
            {
                currentSkin.Anim.ResetTrigger(currentAnimName);
            }

            currentAnimName = animName;
            currentSkin.Anim.SetTrigger(currentAnimName);
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
    protected void TurnTo(Vector3 targetPos)
    {
        currentSkin.TF.LookAt(targetPos + (TF.position.y - targetPos.y) * Vector3.up);
    }
    #endregion

    #region Skin
    public void WeaponEnable()
    {
        curWeapon.SetActive(true);
    }
    public void WeaponDisable()
    {
        curWeapon.SetActive(false);
    }
    public void SetRotationDefault()
    {
        currentSkin.TF.localRotation = Quaternion.identity;
    }
    public void ChangeWeapon(WeaponType type)
    {
        currentSkin.ChangeWeapon(type);
        curWeapon.OnInit(this);
    }
    public void ChangeSkin(SkinType skinType)
    {
        TakeOffCloth();
        currentSkin = SimplePool.Spawn<Skin>((PoolType)skinType, this.TF);
        EquipedCloth();
    }

    public void ChangeHair(HairType hairType)
    {
        currentSkin.ChangeHair(hairType);
    }
    public void ChangePant(PantType pantType)
    {
        currentSkin.ChangePant(pantType);
    }
    public void ChangeAccessory(AccessoryType accessoryType)
    {
        currentSkin.ChangeAccessory(accessoryType);
    }
    public virtual void EquipedCloth()
    {
        
    }

    public void TakeOffCloth()
    {
        currentSkin?.OnDespawn();
        SimplePool.Despawn(currentSkin);
    }
    #endregion

    #region powerUp
    public void IncresingPoint(int enemyPoint)
    {
        enemyPoint = enemyPoint < 1 ? 1 : enemyPoint;
        score += enemyPoint;
        PowerUp();
        indicator.SetScore(score);
    }
    public virtual void PowerUp()
    {
        ParticlePool.Play(ParticleType.BeamUpBlue, TF.position);
        range += 1;
        range = range > Constant.RANGE_MAX ? Constant.RANGE_MAX : range;
        SetSize(range);
    }
    public void SetSize(float range)
    {
        currentSkin.TF.localScale = Vector3.one * range / Constant.RANGE_DEFAULT;
    }
    #endregion

    #region other
    public void SetKillerName(string name)
    {
        killerName = name;
    }
    protected void GetRandomScore()
    {
        score = UnityEngine.Random.Range(0, 10);
        SetSize(range + score /2);
        indicator.SetScore(score);
    }
    #endregion
}
