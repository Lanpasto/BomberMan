using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBehaviour : MonoBehaviour
{
    public RuntimeStatAttributeContainer RuntimeStatAttributeContainer;
    
    public UnitControlBehaviour unitControlBehaviour;

    private Vector2 unitPositionOnMap;
    public Vector2 GeUnitPositionOnMap(){
        return unitPositionOnMap;
    }
    

    public StatAttribute GetStat(string id)
    {
        return this.RuntimeStatAttributeContainer.Stats.GetStartAttributeById(id, this);
    }

    public void AddStat(StatAttribute statAttribute)
    {
        this.RuntimeStatAttributeContainer.Stats.StatAttributes.Add(statAttribute);
    }

    private void Update()
    {
        FindPositionOnMap();
    }

    void FindPositionOnMap()
    {
        var map = MapManager.Instance.GetMap();
        float minDistance = Mathf.Infinity;

        foreach (var obj in map)
        {
            float distance = Vector2.Distance(this.transform.position, obj.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                unitPositionOnMap = obj.coordinates;
            }
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