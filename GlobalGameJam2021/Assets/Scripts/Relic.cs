using UnityEngine;

public class Relic : MonoBehaviour
{
    public RelicData data;

    [SerializeField] Enemy carrier;

    SpriteRenderer spriteRenderer;
    [SerializeField] MazeNode spawnPoint;
    private Transform originalParent;
    public MazeNode SpawnPoint
    {
        get { return spawnPoint; }
        set { spawnPoint = value; }
    }

    BoxCollider2D boxCollider2D;
    public bool isPickedUp;

    private Vector2 startPosition;

    [SerializeField] float scoreIncreaseTimer = 0.5f;
    [SerializeField] int scoreValue = 1;

    float currentTime;
    
    private void Awake()
    {
        originalParent = transform.parent;
        startPosition = transform.position;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if (carrier != null)
            transform.position = carrier.transform.position;
        else if (isPickedUp)
        {
            ReturnToStartPosition();
        }

        if(!isPickedUp && GameStateManager.instance.CurrentGameState == GameStateManager.GameState.GameLoop)
        {
            currentTime += Time.deltaTime;
            if (currentTime > scoreIncreaseTimer)
            {
                // Debug.Log("increasing score");
                currentTime = 0;
                GameManager.instance.AddToScore(scoreValue);
            }
        }

    }

    public void ReturnToStartPosition()
    {
        transform.parent = originalParent;
        transform.position = spawnPoint.transform.position;
        carrier = null;
        spawnPoint.hasRelic = true;
        spriteRenderer.sprite = data.sprite;
        spriteRenderer.sortingOrder = -1;
        spriteRenderer.enabled = true;
        boxCollider2D.enabled = true;
        isPickedUp = false;
        currentTime = 0;
    }

    public void SetData(RelicData data)
    {
        this.data = data;
        spriteRenderer.sprite = data.sprite;
    }

    public void GetPickedUp(Enemy carrier)
    {
        transform.parent = carrier.transform;
        boxCollider2D.enabled = false;
        spriteRenderer.sprite = data.relicIcon;
        spriteRenderer.sortingOrder = 100;
        // spriteRenderer.enabled = false;
        this.carrier = carrier;
        isPickedUp = true;
    }
}
