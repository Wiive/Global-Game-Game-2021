using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Optimizer : MonoBehaviour
{
    public static Optimizer instance;
    
    [SerializeField]private List<ShadowCaster2D> wallShadow = new List<ShadowCaster2D>();
    
    public bool controlShadows;
    public bool controlFlashlights;
    public float timeStep;
    private float timer;
    public float distanceBorderXOffset;
    public float distanceBorderYOffset;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (timer >= timeStep)
        {
            if (controlShadows)
            {
                foreach (var shadow in wallShadow)
                {
                    //shadow.castsShadows = CheckInsideBorder(shadow.transform.position);
                }
            }

            if (controlFlashlights)
            {
                foreach (var hunter in SpawnManager.instance.SpawnedHunters)
                {
                    bool result = CheckInsideBorder(hunter.CurrentPos, hunter.name);

                    Flashlight flashlight = hunter.GetComponentInChildren<Flashlight>();
                
                    if (result)
                    {
                        flashlight.light2D.color = Color.green; //enabled = true;
                    }
                    else
                    {
                        flashlight.light2D.color = Color.red; //enabled = false;
                    }
                }
            }
            
            timer = 0f;
        }   
        
        timer += Time.deltaTime;
    }

    private bool CheckInsideBorder(Vector2 position, string name)
    {
        //DECLARE OUTSIDE OF FUNCTION
        //public float distanceBorderXOffset = 100;
        //public float distanceBorderYOffset = 80;
        
        bool checkX = true;
        bool checkY = true;
        
        
        float distanceX = Mathf.Abs( position.x - transform.position.x);
        float distanceY = Mathf.Abs(position.y - transform.position.y);
        
        Debug.Log(name + " x:" + distanceX + " y:" + distanceY + " pos:" + position);

        if (distanceX > distanceBorderXOffset)
        {
            checkX = false;
        }
        
        if (distanceY > distanceBorderYOffset)
        {
            checkY = false;
        }

        if (!checkX || !checkY)
        {
            return false;
        }

        return true;
    }
    
    public void CollectWalls()
    {
        foreach (var mazeNode in GameObject.FindObjectsOfType<MazeNode>())
        {
            if (mazeNode.isWall)
            {
                mazeNode.gameObject.AddComponent<ShadowCaster2D>();
                wallShadow.Add(mazeNode.GetComponent<ShadowCaster2D>());
            }
        }
    }
}
