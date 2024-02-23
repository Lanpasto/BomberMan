using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBeamBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject Flame;
    
    [SerializeField] private GameObject FlameStartUp;
    [SerializeField] private GameObject FlameStartUpLeft;
    [SerializeField] private GameObject FlameStartUpDown;
    [SerializeField] private GameObject FlameStartUpRight;
    [SerializeField] private GameObject FlameStartUpLeftRight;
    [SerializeField] private GameObject FlameStartUpLeftDown;
    [SerializeField] private GameObject FlameStartDown;
    [SerializeField] private GameObject FlameStartDownLeft;
    [SerializeField] private GameObject FlameStartDownRight;
    [SerializeField] private GameObject FlameStartDownRightLeft;
    [SerializeField] private GameObject FlameStartDownRightUp;
    [SerializeField] private GameObject FlameStartLeft;
    [SerializeField] private GameObject FlameStartRight;
    [SerializeField] private GameObject FlameStartLeftRight;
    [SerializeField] private GameObject FlameStartLeftRightUpDown;

    [SerializeField] private GameObject FlameEnd;

    private List<EntityBehaviour> entityBehaviours = new();
    private Vector2 startCoordinate;
    private float lifespawn;
    
    private Vector2 closestUpCoordinate;
    private Vector2 closestDownCoordinate;
    private Vector2 closestLeftCoordinate;
    private Vector2 closestRightCoordinate;

    private bool right;
    private bool left;
    private bool up;
    private bool down;
    
    public void Initialize(Vector2 startCoordinate ,List<EntityBehaviour> entityBehaviours, float lifespan)
    {
        
        
        this.startCoordinate = startCoordinate;
        Debug.Log(this.startCoordinate + " START COORDS");
        this.lifespawn = lifespan;
        this.entityBehaviours.AddRange(entityBehaviours);
        DeterminateClosestCoordinates();
        SpawnFlame();
    }

    private void DeterminateClosestCoordinates()
    {
        closestUpCoordinate = Vector2.positiveInfinity;
        closestDownCoordinate =  Vector2.positiveInfinity;
        closestLeftCoordinate =  Vector2.positiveInfinity;
        closestRightCoordinate =  Vector2.positiveInfinity;

        foreach (var entity in entityBehaviours)
        {
            var entityCoordinates = entity.coordinates;

            if (entityCoordinates.y > startCoordinate.y && Vector2.Distance(entityCoordinates, startCoordinate) <
                Vector2.Distance(closestUpCoordinate, startCoordinate))
            {
                closestUpCoordinate = entityCoordinates;
                up = true;
            }
            else if (entityCoordinates.y < startCoordinate.y &&
                     Vector2.Distance(entityCoordinates, startCoordinate) <
                     Vector2.Distance(closestDownCoordinate, startCoordinate))
            {
                closestDownCoordinate = entityCoordinates;
                down = true;
            }

            if (entityCoordinates.x < startCoordinate.x && Vector2.Distance(entityCoordinates, startCoordinate) <
                Vector2.Distance(closestLeftCoordinate, startCoordinate))
            {
                closestLeftCoordinate = entityCoordinates;
                left = true;
            }
            else if (entityCoordinates.x > startCoordinate.x &&
                     Vector2.Distance(entityCoordinates, startCoordinate) <
                     Vector2.Distance(closestRightCoordinate, startCoordinate))
            {
                closestRightCoordinate = entityCoordinates;
                right = true;
            }
        }
        
        Debug.Log(closestUpCoordinate + "closestUpCoordinate");
        Debug.Log(closestDownCoordinate + "closestDownCoordinate");
        Debug.Log(closestLeftCoordinate + "closestLeftCoordinate");
        Debug.Log(closestRightCoordinate + "closestRightCoordinate");
    }

    private void SpawnFlame()
    {
        if (up)
        {
            
        }

        if (left)
        {
            
        }

        if (right)
        {
            
        }

        if (down)
        {
            
        }
    }
}
