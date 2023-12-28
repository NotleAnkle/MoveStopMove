using _Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    [SerializeField] Player player;

    public void OnInit()
    {
        this.transform.localScale = Vector3.one * player.Range;
    }

    private void OnTriggerEnter(Collider other)
    {
        Bot bot = Cache<Bot>.GetComponent(other);
        if(bot != null && !bot.IsDying)
        {
            bot.TurnOnTargetCircle();
            player.AddTarget(bot);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        Bot bot = Cache<Bot>.GetComponent(other);
        if (bot != null)
        {
            bot.TurnOffTargetCircle();
            player.RemoveTarget(bot);  
        }
    }
}
