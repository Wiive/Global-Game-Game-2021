using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    protected override void Update()
    {
        base.Update();
        
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (direction.x != 0f && direction.y != 0f)
            direction = Vector2.zero;
    }
}
