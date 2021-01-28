using System;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private const int Offset = 8;

    private void Awake()
    {
        GetAllComponents();
    }

    private void GetAllComponents()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    
    public void UpdateDirection(Vector2 direction)
    {
        transform.localPosition = direction * Offset;

        transform.up = direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Debug.Log($"{name} Found and wants to kill player!");
    }
}