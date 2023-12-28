using _Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAttackRange : MonoBehaviour
{
    private Character owner;

    public void OnInit(Character owner)
    {
        this.transform.localScale = Vector3.one * owner.Range;
        this.owner = owner;
    }

    private void OnTriggerEnter(Collider other)
    {
        Character character = Cache<Character>.GetComponent(other);
        if (character != null &&  character != owner && !character.IsDying)
        {
            owner.AddTarget(character);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        Character character = Cache<Character>.GetComponent(other);
        if (character != null && character != owner)
        {
            owner.RemoveTarget(character);
        }
    }
}
