using _Framework;
using _Framework.Pool.Scripts;
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
        if (character != owner && !character.IsDying)
        {
            OnDespawn();
            character.OnDeath();
            owner.IncresingPoint(character.Point);
        }
    }

    protected void OnDespawn()
    {
        owner.WeaponEnable();
        SimplePool.Despawn(this);
    }
}
