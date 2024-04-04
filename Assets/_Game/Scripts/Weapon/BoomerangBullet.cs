using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBullet : Bullet
{
    [SerializeField] AnimationCurve curve;
    private float rotationSpeed = 800f;
    private bool IsBack = false;

    public override void Move()
    {
        float step = speed * Time.deltaTime;
        //TF.position = Vector3.Lerp(TF.position, targetPos, curve.Evaluate(step));
        TF.position = Vector3.MoveTowards(TF.position, targetPos, step);
        // Xoay tròn theo trục Z
        TF.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);

        if (IsBack)
        {
            targetPos = owner.throwPoint;
        }

        if(Vector3.Distance(TF.position, targetPos) < 0.5f)
        {
            if (IsBack)
            {
                OnDespawn();
            }
            else
            {
                IsBack = true;
                speed *= 2;
            }
        }
    }

    public override void TurnTo(Vector3 target)
    {
        IsBack = false;
        speed = 10f;
        //Vector3 lookDirection = target - transform.position;
        //float angle = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;

        //// Chỉ xoay theo trục Z
        //TF.Rotate(0f, angle, 0f);
    }
}
