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
    private Vector2 targetDirection;
    private Flashlight flashlight;
    [SerializeField] private EnemyData data;

    [SerializeField] float respawnTime = 5f;
    [SerializeField] private MazeNode spawnPoint;

    public MazeNode SpawnPoint { set { spawnPoint = value; } }

    [SerializeField] private Material dissolveMaterial;
    private Material baseMaterial;

    private SpawnManager spawnManager;
    
    Vector2Int gridSize = new Vector2Int(0,0);

    bool gettingNewPath = false;
    bool isCarryingRelic = false;
    bool respawning = false;
    private bool turning = false;
    [SerializeField] private float turnSpeed;

    [SerializeField] Relic stolenRelic = null;

    int pathIndexPosition = 0;
    PathFinder pathFinder;
    
    float fade = 1;

    private EnemySound enemySound;
    
    protected override void Start()
    {
        base.Start();
        transform.position = spawnPoint.transform.position;
        baseMaterial = spriteRenderer.material;
        direction = Vector2.down;
        faceDirection = direction;
    }

    protected override void Update()
    {
        if (!isAlive)
        {                      
            spriteRenderer.material.SetFloat("_Fade", fade); //material.SetFloat("Fade",fade);

            fade -= 1f * Time.deltaTime;
            
            if (fade <= 0)
            {
                if (!respawning)
                {
                    respawning = true;
                    StartCoroutine(HandleRespawn());
                }
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
        spawnManager = FindObjectOfType<SpawnManager>();
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
            animator.SetFloat("DirectionX", faceDirection.x);
            animator.SetFloat("DirectionY", faceDirection.y);
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
        GameManager.instance.AddToScore(100);
        flashlight.TurnOfLight();
        stolenRelic?.ReturnToStartPosition();
        stolenRelic = null;
        base.GotKilled();
        spriteRenderer.material = dissolveMaterial;
        enemySound.PlayDeathSound();
    }
    
    private void UpdateFlashlightDirection()
    {
        flashlight.UpdateDirection(direction, targetDirection);
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
        if (moveController.IsMoving || turning)
            return;

        CurrentPos = path[pathIndexPosition].GridPos;

        targetDirection = path[pathIndexPosition + 1].GridPos - CurrentPos;
        Vector2 position = path[pathIndexPosition + 1].transform.position;
        
        if (direction != targetDirection)
            StartCoroutine(TurnCharacter());
        else
        {
            direction = path[pathIndexPosition + 1].GridPos - CurrentPos;
            moveController.SetEnemyTargetPosition(position);
            pathIndexPosition++;
        }
    }

    IEnumerator TurnCharacter()
    {
        turning = true;

        int counter = 0;
        
        float startDiff = Mathf.Abs(Mathf.DeltaAngle(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg,
                                            Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg));

        float turnSpeed = startDiff > 90 ? this.turnSpeed / 2f : this.turnSpeed;

        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        // Debug.Log($"{name}: TURN - startDiff: {startDiff} - dir: {direction} - targetDir: {targetDirection}");

        while (turning)
        {
            counter++;
            
            // direction = Vector2.Lerp(direction, targetDirection, turnSpeed * Time.deltaTime);

            float currentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float newAngle = Mathf.Lerp(currentAngle, targetAngle, turnSpeed * Time.deltaTime);

            direction = new Vector2(Mathf.Cos(newAngle * Mathf.Deg2Rad), Mathf.Sin(newAngle * Mathf.Deg2Rad)).normalized;
            faceDirection = direction;
            
            currentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            float angleDiff = Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle));

            // Debug.Log(  $"{name}[{counter}] - currentAngle: {currentAngle} - targetAngle: {targetAngle} - diff: {angleDiff} - newAngle: {newAngle}\n" +
            //                     $"targetDirection: {targetDirection} - newDirection: {direction}");

            if (angleDiff <= 2f)
            {
                turning = false;
            
                direction = targetDirection;
                faceDirection = direction;
                moveController.SetTargetPosition(direction);
                pathIndexPosition++;
                
                // Debug.Log($"{name}[{counter}] diff: {angleDiff} - Is Done Turning!");
                
                break;
            }

            yield return 0;
        }
    }

    IEnumerator HandleNewPath()
    {
        yield return new WaitForSeconds(WaitTime);
        if (path.Count > 0)
            CurrentPos = path[pathIndexPosition].GridPos;

/*        if (pathFinder.GetWayPoint(CurrentPos).isExit && isCarryingRelic)
        {
            Debug.Log("I Enemy: " + name + " have now exited the labyrinth with a relic");
            Destroy(gameObject);
        }*/

        // float currentAngle = Vector2.Angle(direction, Vector2.up);
        // Debug.Log($"{name} currentAngle: {currentAngle}");
        
        // yield return new WaitForSeconds(WaitTime/2);
        // flashlight.EnemyStopped();
        // yield return new WaitForSeconds(WaitTime/2);
        GetNewPath();
        yield return new WaitForSeconds(WaitTime);
        gettingNewPath = false;
    }

    private void GetNewPath()
    {
        if (!isAlive) return;
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

    IEnumerator HandleRespawn()
    {
        spawnPoint = spawnManager.GetSpawnPos();
        path.Clear();
        yield return new WaitForSeconds(respawnTime);
        spriteRenderer.material = baseMaterial;
        CurrentPos = spawnPoint.GridPos;
        transform.position = spawnPoint.transform.position;
        flashlight.ResetFlashLight();
        Respawn();
        respawning = false;
        enemySound.PlaySpawnSound();
    }
}
