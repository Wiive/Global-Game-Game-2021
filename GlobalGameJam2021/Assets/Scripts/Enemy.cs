using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] List<MazeNode> path = new List<MazeNode>();
    [SerializeField] Vector2Int destination = new Vector2Int(0, 0);
    [SerializeField] float WaitTime = 2f;    
    [SerializeField]int range = 4;
    private Vector2 faceDirection;
    private Flashlight flashlight;
    [SerializeField] private EnemyData data;

    [SerializeField] private Material dissolveMaterial;
    private Material baseMaterial;
    
    Vector2Int gridSize = new Vector2Int(0,0);

    bool gettingNewPath = false;
    bool isCarryingRelic = false;

    [SerializeField] Relic stolenRelic = null;

    int pathIndexPosition = 0;
    PathFinder pathFinder;
    
    float fade = 1;

    private EnemySound enemySound;
    
    protected override void Start()
    {
        base.Start();
        baseMaterial = spriteRenderer.material;
    }

    protected override void Update()
    {
        if (!isAlive)
        {           
            spriteRenderer.material.SetFloat("_Fade", fade); //material.SetFloat("Fade",fade);

            fade -= 1f * Time.deltaTime;

            if (fade <= 0)
            {
                GameManager.instance.AddToScore(100);
                Destroy(this.gameObject);
            }
            
            return;
        }
        
        base.Update();

        if (GameStateManager.instance.CurrentGameState == GameStateManager.GameState.GameLoop)
        {
            if (pathIndexPosition <= path.Count - 2)
            {
                MoveEnemyAlongPath();
            }
            else if (!gettingNewPath)
            {
                gettingNewPath = true;
                StartCoroutine(HandleNewPath());
            }
            UpdateFlashlightDirection();
        }
     
    }

    protected override void GetAllComponents()
    {
        base.GetAllComponents();
        enemySound = GetComponentInChildren<EnemySound>();
        pathFinder = GetComponent<PathFinder>();
        flashlight = GetComponentInChildren<Flashlight>();
    }

    protected override void Init()
    {
        base.Init();

        gridSize = FindObjectOfType<MazeCreator>().GridSize;
        destination = TryToGetDestination();
        path = pathFinder.SearchForPath(CurrentPos, destination);
    }

    public void SetData(EnemyData data)
    {
        this.data = data;

        animator.runtimeAnimatorController = data.runtimeAnimatorController;
    }
    
    protected override void UpdateAnimations()
    {
        if (direction != Vector2.zero)
        {
            animator.SetFloat("DirectionX", direction.x);
            animator.SetFloat("DirectionY", direction.y);
        }
        animator.SetBool("IsMoving", moveController.IsMoving);
    }

    protected override void Pickup(Relic relic)
    {
        if (stolenRelic != null) return;
        Debug.Log($"{name} Pickups Relic!");
        stolenRelic = relic;
        relic.GetPickedUp(this);
        enemySound.PlayPickUpSound();
    }

    protected override void Attack(Character character)
    {
        base.Attack(character);
        enemySound.PlayAttackSound();
    }
    
    public override void GotKilled()
    {
        stolenRelic = null;
        base.GotKilled();
        spriteRenderer.material = dissolveMaterial;
        enemySound.PlayDeathSound();
    }
    
    private void UpdateFlashlightDirection()
    {
        flashlight.UpdateDirection(direction);
    }
    
    protected override void UpdateTimers()
    {
        base.UpdateTimers();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Relic"))
        {
            Pickup(other.GetComponent<Relic>());
            GameManager.instance.PickedUpObject(this.gameObject, other.gameObject);
        }
    }

    private void MoveEnemyAlongPath()
    {
        if (moveController.IsMoving)
            return;

        CurrentPos = path[pathIndexPosition].GridPos;
        direction = path[pathIndexPosition + 1].GridPos - CurrentPos;
        moveController.SetTargetPosition(direction);
        pathIndexPosition++;
    }

    IEnumerator HandleNewPath()
    {
        if(path.Count > 0)
            CurrentPos = path[pathIndexPosition].GridPos;
        else
            CurrentPos = new Vector2Int(Mathf.RoundToInt(transform.position.x / TileSize),
                                        Mathf.RoundToInt(transform.position.y / TileSize));


        if (pathFinder.GetWayPoint(CurrentPos).isExit && isCarryingRelic)
        {
            Debug.Log("I Enemy: " + name + " have now exited the labyrinth with a relic");
            Destroy(gameObject);
        }
        
        yield return new WaitForSeconds(WaitTime/2);
        flashlight.EnemyStopped();
        yield return new WaitForSeconds(WaitTime/2);
        GetNewPath();
        gettingNewPath = false;
    }

    private void GetNewPath()
    {
        destination = TryToGetDestination();
        pathIndexPosition = 0;
        path = pathFinder.SearchForPath(CurrentPos, destination);
    }

    private Vector2Int TryToGetDestination()
    {
        if (!isCarryingRelic)
        {
            destination = pathFinder.SearchForRelic(range, CurrentPos);
        }
        else if(isCarryingRelic)
        {
            destination = pathFinder.SearchForExit(range, CurrentPos);
        }

        MazeNode wayPointDestination = pathFinder.GetWayPoint(destination);

        if (destination.x == 0 && destination.y == 0)
        {
            do
            {
                int minX = Mathf.Clamp(CurrentPos.x - range, 0, 100);
                int minY = Mathf.Clamp(CurrentPos.y - range, 0, 100);

                int maxX = Mathf.Clamp(CurrentPos.x + range, 0, gridSize.x) + 1;
                int maxY = Mathf.Clamp(CurrentPos.y + range, 0, gridSize.y) + 1;

                int x = Random.Range(minX, maxX);
                int y = Random.Range(minY, maxY);
                Vector2Int wayPointKey = new Vector2Int(x, y);
                wayPointDestination = pathFinder.GetWayPoint(wayPointKey);
            }
            while (wayPointDestination == null || wayPointDestination.isWall || wayPointDestination.GridPos == CurrentPos);
        }
        else
        {
            isCarryingRelic = true;
        }

        return wayPointDestination.GridPos;
    }
}
