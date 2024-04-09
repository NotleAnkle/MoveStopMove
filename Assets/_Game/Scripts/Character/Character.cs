using _Framework.Event.Scripts;
using _Framework.Pool.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class Character : GameUnit
{
    #region components
    //Target 
    [SerializeField] private List<Character> targetList = new List<Character>();

    // Skin & weapon
    [SerializeField] protected Skin currentSkin;
    public bool IsHasTarget => targetList.Count > 0;
    public bool IsAttackable => curWeapon.IsActive;
    public Vector3 throwPoint => currentSkin.RightHand.position;
    protected Weapon curWeapon => currentSkin.CurWeapon;
    private string currentAnimName;

    //Character stats
    [SerializeField] protected float speed = 5f;
    public float Range => range;
    public int Score => score;
    public bool IsDying { get; protected set; }
    public string KillerName { get; protected set; }
    private float range;
    protected int score = 0;

    //Indicator
    [SerializeField] protected Transform indicatorPoint;
    protected TargetIndicator indicator;
    public string ownerName => indicator.Name;

    #endregion

    #region basic
    public virtual void OnInit()
    {
        targetList.Clear();
        score = 0;
        range = Constant.RANGE_DEFAULT;
        IsDying = false;
        ChangeAnim(Constant.ANIM_IDLE);

        if (!indicator)
        {
            indicator = SimplePool.Spawn<TargetIndicator>(PoolType.TargetIndicator);
        }
        indicator.SetTarget(indicatorPoint);
        indicator.SetAlpha(1);
    }
    public virtual void OnDeath()
    {
        LevelManager.Instance.CheckCharacterDie(this);
        ChangeAnim(Constant.ANIM_DEAD);
        IsDying = true;
        indicator.SetAlpha(0);
    }
    public virtual void OnDespawn()
    {
        TakeOffCloth();
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

    public void CheckTargetList()
    {
        List<Character> removelist = new List<Character>();

        for(int i = 0; i < targetList.Count; i++)
        {
            if (targetList[i].IsDying || !IsTargetInRange(targetList[i]))
            {
                removelist.Add(targetList[i]);
            }
        }

        for(int i = 0; i < removelist.Count; i++)
        {
            RemoveTarget(removelist[i]);
        }

    }

    private bool IsTargetInRange(Character target)
    {
        return Vector3.Distance(TF.position, target.TF.position) < Range/0.94;
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
        currentSkin = SimplePool.Spawn<Skin>((PoolType)skinType, this.TF);
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
        KillerName = name;
    }
    protected void GetRandomScore()
    {
        //random diem tu 0 - 10
        score = UnityEngine.Random.Range(0, 10);
        indicator.SetScore(score);

        //random size tu 1 - 1.5 player
        float playerRange = LevelManager.Instance.Player.Range;
        float addedRange = playerRange - Constant.RANGE_DEFAULT;

        range += Random.Range(addedRange, addedRange*1.5f);
        SetSize(range);
    }
    #endregion
}
