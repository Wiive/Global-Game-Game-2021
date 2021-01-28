using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private bool moveConstant = true;
    protected override void Update()
    {
        base.Update();
        
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (moveConstant)
        {
            if (input.x != input.y && input != Vector2.zero)
                direction = input;
        }
        else
        {
            if (input.x != 0f && input.y != 0f)
                direction = Vector2.zero;
            else
                direction = input;
        }
        
        if (direction != Vector2.zero)
            moveController.SetTargetPosition(direction);
    }
    
    protected override void Pickup(Relic relic)
    {
        Debug.Log($"{name} Pickups Relic!");
        relic.ReturnToStartPosition();
    }
}
