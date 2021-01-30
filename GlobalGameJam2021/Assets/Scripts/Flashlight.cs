using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private Animator taserAnimator;
    private SpriteRenderer spriteRenderer;
    private const int Offset = 8;

    private Light2D flashlight;
    
    public float minIntensity = 0.2f;
    public float maxIntensity = 0.4f;
    public float intensityStepModifier = 0.9f;

    public float maxRadius;
    public float minRadius;
    public float radiusStepModifier;
    public float reduceTime;
    public float increaseTime;
    [SerializeField] bool increaseRadius;
    
    private float currentIntensity;
    private bool intensityDirection;

    private Vector2 lastDirection;
    
    private void Awake()
    {
        GetAllComponents();
    }

    private void Start()
    {
        flashlight = GetComponent<Light2D>();
        
        currentIntensity = Random.Range(minIntensity, maxIntensity);
    }

    private void GetAllComponents()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (currentIntensity < minIntensity)
        {
            intensityDirection = true;
        }
        if (currentIntensity > maxIntensity)
        {
            intensityDirection = false;
        }
        
        if (intensityDirection)
        {
            currentIntensity += intensityStepModifier * Time.deltaTime;
        }
        else
        {
            currentIntensity -= intensityStepModifier * Time.deltaTime;
        }
        
        flashlight.intensity = currentIntensity;

        UpdateRadius();
    }

    public void UpdateDirection(Vector2 direction)
    {
        if (lastDirection != direction)
        {
            increaseRadius = true;
        }

        if (direction == Vector2.down)
        {
            transform.localPosition = direction * (Offset - 6f);
        }
        else
        {
            transform.localPosition = direction * Offset;
        }
        
        
        transform.up = direction;

        lastDirection = direction;
    }

    public void EnemyStopped()
    {
        increaseRadius = false;
    }


    
    private void UpdateRadius()
    {
        if (increaseRadius)
        {
            if (flashlight.pointLightOuterRadius < maxRadius)
            {
                flashlight.pointLightOuterRadius += increaseTime * Time.deltaTime;
            }
        }
        else
        {
            if (flashlight.pointLightOuterRadius > minRadius)
            {
                flashlight.pointLightOuterRadius -= reduceTime * Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            taserAnimator.SetTrigger("Attack");
            Debug.Log($"{name} Found and wants to kill player!");
        }
    }
}