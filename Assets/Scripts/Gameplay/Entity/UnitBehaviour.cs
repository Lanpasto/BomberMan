using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBehaviour : EntityBehaviour
{
    public RuntimeStatAttributeContainer RuntimeStatAttributeContainer;
    
    public UnitControlBehaviour unitControlBehaviour;
    
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

    public override void TakeDamage()
    {
        if (GetStat("Health").Value <= 0)
        {
            base.TakeDamage();
        }
    }

    private void Update()
    {
        FindPositionOnMap();
    }
    
    private void FindPositionOnMap()
    {
        var map = MapManager.Instance.GetMap();
        float minDistance = Mathf.Infinity;
        
        MapUnit closestPointObject = new MapUnit();
        foreach (var obj in map)
        {
            
            float distance = 0f;
            if (!obj.Contains(this.entityDescription))
            {
                distance = Vector2.Distance(this.transform.position, obj.GetTransform());
            }
            else
            {
                distance = Vector2.Distance(this.transform.position, obj.GetCoordinates());
            }
           
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPointObject = obj;
            }
        }
        
        if (!closestPointObject.Contains(this.entityDescription))
        {
            MapManager.Instance.UnRegisterBlock(this);
            this.coordinates = closestPointObject.GetCoordinates();
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