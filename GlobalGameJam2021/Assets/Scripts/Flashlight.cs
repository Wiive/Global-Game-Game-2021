using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

public class Flashlight : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private const int Offset = 8;

    private Light2D flashlight;
    
    public float minIntensity = 0.2f;
    public float maxIntensity = 0.4f;
    public float intensityStepModifier = 0.9f;

    public float maxRadius;
    public float beforeTurnRadius;
    public float radiusStepModifier;
    
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
    }

    public void UpdateDirection(Vector2 direction)
    {
        if (lastDirection != direction)
        {
            StartCoroutine(ChangeRadius());
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

    IEnumerator ChangeRadius()
    {
        bool reduce = true;
        
        while (reduce == true)
        {
            if (flashlight.pointLightOuterRadius > beforeTurnRadius)
            {
                flashlight.pointLightOuterRadius -= radiusStepModifier;
            }
            else
            {
                reduce = false;
            }

            yield return new WaitForSeconds(0.05f);
        }
        
        while (reduce == false)
        {
            if (flashlight.pointLightOuterRadius < maxRadius)
            {
                flashlight.pointLightOuterRadius += radiusStepModifier;
            }
            else
            {
                reduce = true;
            }
            
            yield return new WaitForSeconds(0.05f);
        }

        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Debug.Log($"{name} Found and wants to kill player!");
    }
}