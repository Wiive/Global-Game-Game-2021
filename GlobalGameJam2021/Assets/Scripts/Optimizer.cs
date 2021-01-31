using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Debug = UnityEngine.Debug;

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
    
    void Update()
    {
        if (timer >= timeStep)
        {
            if (controlShadows)
            {
                foreach (var shadow in wallShadow)
                {
                    shadow.castsShadows = IsInsideBorder(shadow.transform.position, shadow.name);
                }
            }

            if (controlFlashlights)
            {
                foreach (var hunter in SpawnManager.instance.SpawnedHunters)
                {
                    Flashlight flashlight = hunter.GetComponentInChildren<Flashlight>();
                    if (flashlight.light2D == null)
                        continue;

                    flashlight.light2D.enabled = IsInsideBorder(hunter.CurrentPos, hunter.name);
                }
            }
            
            timer = 0f;
        }   
        
        timer += Time.deltaTime;
    }

    private bool IsInsideBorder(Vector2 position, string name)
    {
        position *= 16f;
        
        if (Mathf.Abs(position.x - transform.position.x) > 160 + distanceBorderXOffset ||
            Mathf.Abs(position.y - transform.position.y) > 90 + distanceBorderYOffset)
            return false;

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
