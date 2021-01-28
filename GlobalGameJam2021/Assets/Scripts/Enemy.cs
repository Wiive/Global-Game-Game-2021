using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    protected override void Pickup(Relic relic)
    {
        Debug.Log($"{name} Pickups Relic!");
    }
    
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Relic")
        {
            Pickup(other.GetComponent<Relic>());
            
            GameManager.instance.PickedUpObject(this.gameObject, other.gameObject);
        }
    }
}
