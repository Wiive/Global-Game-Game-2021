using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private bool moveConstant = true;
    protected override void Update()
    {
        base.Update();
        
        // TODO: REMOVE LATER (DEBUG ONLY)
        DebugInput();
        
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (moveConstant)
        {
            if ((input.x != 0 && input.y == 0 || input.x == 0 && input.y != 0) && input != Vector2.zero)
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
    
    protected override void Attack(Character character)
    {
        base.Attack(character);
    }
    
    protected override void GotKilled()
    {
        base.GotKilled();
    }
    
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        
        if (other.CompareTag("Enemy"))
            Attack(other.GetComponent<Enemy>());
    }

    // TODO REMOVE LATER (DEBUG ONLY)
    private void DebugInput()
    {
        if (Input.GetKeyDown(KeyCode.C))
            moveConstant = !moveConstant;
    }
}
