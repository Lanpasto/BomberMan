using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBehaviour : EntityBehaviour
{
    public RuntimeStatAttributeContainer RuntimeStatAttributeContainer;
    
    public UnitControlBehaviour unitControlBehaviour;
    
    private void OnEnable()
    {
        unitControlBehaviour.OnAction += UnitActions;
    }
    
    private void OnDisable()
    {
        unitControlBehaviour.OnAction -= UnitActions;
    }

    public UnitDescriptionSO UnitDescription { get; set; }

    public StatAttribute GetStat(string id)
    {
        return this.RuntimeStatAttributeContainer.Stats.GetStartAttributeById(id, this);
    }

    public void AddStat(StatAttribute statAttribute)
    {
        this.RuntimeStatAttributeContainer.Stats.StatAttributes.Add(statAttribute);
    }

    
    public override void Initialize(EntityDescription description, Vector2 coordinates)
    {
        base.Initialize(description, coordinates);
        transform.position = MapManager.Instance.GetPositionByCoordinates(coordinates);
        unitControlBehaviour.PlayerInputKeys = UnitDescription.PlayerInput;
        foreach (var statAttribute in UnitDescription.UnitStatsAttributes.StatAttributes)
        {
            AddStat(statAttribute);
        }
    }
    
    private void Update()
    {
        FindPositionOnMap();
    }

    private void UnitActions(InputManager.PlayerAction action)
    {
        switch (action)
        {
            case InputManager.PlayerAction.bomb:
                //BOMB
                break;
        }
    }
    
    FindPositionOnMap()
    var map = MapManager.Instance.GetMap();
        float minDistance = Mathf.Infinity;
        
        EntityBehaviour closestPointObject = null;
        foreach (var obj in map)
        {
           //Debug.Log(obj);
            float distance = 0f;
            if (obj.entityDescription != this.entityDescription)
            {
                distance = Vector2.Distance(this.transform.position, obj.transform.position);
            }
            else
            {
                distance = Vector2.Distance(this.transform.position, obj.coordinates);
            }
           
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPointObject = obj;
            }

        }
        
        if (closestPointObject.entityDescription != entityDescription)
        {
            MapManager.Instance.UnRegisterBlock(this.coordinates);
            //Debug.Log("CURRENT CORD " + coordinates + " new cords " + closestCoordinates);
            this.coordinates = closestPointObject.coordinates;
            MapManager.Instance.RegisterNewBlock(this);
        }
       
    }
}

[Serializable]
public class UnitBehaviourWithStats
{
    public UnitBehaviour Unit;
    public List<Stats> UnitStat;

    public UnitBehaviourWithStats(UnitBehaviour unit, List<Stats> stats)
    {
        this.UnitStat = stats;
        this.Unit = unit;
    }
}