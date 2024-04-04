using _Framework;
using _Framework.Pool.Scripts;
using _UI.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : GameUnit
{
    [SerializeField] protected float speed = 5f;
    protected Vector3 targetPos;
    protected Vector3 startPos;
    protected Vector3 direction;
    protected float range;

    protected Character owner;

    public void OnInit(Vector3 target, Character character)
    {
        owner = character;
        range = character.Range;
        TurnTo(target);
        targetPos = target;
        startPos = TF.position;

        direction = (target - startPos).normalized;

        TF.localScale = Vector3.one * (float) owner.Range / Constant.RANGE_DEFAULT;
    }

    public abstract void Move();
    public abstract void TurnTo(Vector3 target);

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    protected void OnTriggerEnter(Collider other)
    {
        Character character = Cache<Character>.GetComponent(other); 
        if (character != owner && !character.IsDying && GameManager.IsState(GameState.GamePlay))
        {
            OnDespawn();
            character.OnDeath();
            owner.IncresingPoint(character.Score);
            character.SetKillerName(owner.ownerName);
            SoundManager.Instance.Play(AudioType.SFX_WeaponHit);
            ParticlePool.Play(ParticleType.BloodExplosion, TF.position);
        }
    }

    protected void OnDespawn()
    {
        owner.WeaponEnable();
        SimplePool.Despawn(this);
    }
}
