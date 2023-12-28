using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class KnifeBullet : Bullet
{
    public override void Move()
    {
        TF.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(TF.position, startPos) > range)
        {
            OnDespawn();
        }
    }

    public override void TurnTo(Vector3 target)
    {
        Vector3 lookDirection = target - transform.position;
        //float angle = Mathf.Atan2(lookDirection.z, lookDirection.x) * Mathf.Rad2Deg;

        //// Chỉ xoay theo trục Z
        //TF.Rotate(0f, 0f, angle);
        Quaternion quaternion = Quaternion.LookRotation(lookDirection);
        TF.rotation = quaternion;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(targetPos, Vector3.one/5);
    }
}
