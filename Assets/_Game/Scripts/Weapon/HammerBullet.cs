using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerBullet : Bullet
{
    private float rotationSpeed = 1000f;

    public override void Move()
    {
        TF.position += direction * speed * Time.deltaTime;

        // Xoay tròn theo trục Z
        TF.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);

        if (Vector3.Distance(TF.position, startPos) > range)
        {
            OnDespawn();
        }
    }

    public override void TurnTo(Vector3 target)
    {
    }
}
