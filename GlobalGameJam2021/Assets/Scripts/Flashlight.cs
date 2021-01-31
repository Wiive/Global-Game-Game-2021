using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private Animator taserAnimator;
    private SpriteRenderer spriteRenderer;
    private const int Offset = 2;

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
    private float startIntensity;

    private Vector2 lastDirection;

    private bool flashLightActive;

    PolygonCollider2D polyCollider;
    
    private void Awake()
    {
        GetAllComponents();
    }

    private void Start()
    {
        flashlight = GetComponentInChildren<Light2D>();
        polyCollider = GetComponentInChildren<PolygonCollider2D>();

        currentIntensity = Random.Range(minIntensity, maxIntensity);
        startIntensity = currentIntensity;
        
        ResetFlashLight();
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

        if (flashLightActive)
            flashlight.intensity = currentIntensity;
        else
            flashlight.intensity = 0f;
        
        UpdateRadius();
    }

    public void UpdateDirection(Vector2 direction, Vector2 targetDirection)
    {
        if (lastDirection != direction)
        {
            increaseRadius = true;
        }

        // if (direction == Vector2.down)
        // {
        //     transform.localPosition = direction * (Offset - 6f);
        // }
        // else
        // {
        //     transform.localPosition = direction * Offset;
        // }

        if (targetDirection == Vector2.down)
            transform.localPosition = direction * Offset + Vector2.up * 2f;
        else
            transform.localPosition = direction * Offset;
        
        // transform.up = direction;
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f);

        lastDirection = direction;
    }

    public void EnemyStopped()
    {
        increaseRadius = false;
    }

    public void TurnOfLight()
    {
        polyCollider.enabled = false;
        flashLightActive = false;
    }

    public void ResetFlashLight()
    {
        polyCollider.enabled = true;
        flashLightActive = true;
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