using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBehaviour : MonoBehaviour
{
    public UnitDescriptionSO playerTestDescription;
    public LevelPropetries LevelPropetries;

    private SpawnMap mapGenerator;
   
    
    private void Start()
    {
        mapGenerator = GetComponentInChildren<SpawnMap>();
        
        MapManager.Instance.SetMap(mapGenerator.GenerateMap(LevelPropetries.width, LevelPropetries.height));
        
        
        SpawnPlayer(playerTestDescription);
    }

    private void SpawnPlayer(UnitDescriptionSO unitDescription)
    {
        var unit = Instantiate(unitDescription.UnitBehaviour);
        unit.GetComponent<UnitBehaviour>().unitControlBehaviour.PlayerInputKeys = unitDescription.PlayerInput;
        foreach (var statAttribute in unitDescription.UnitStatsAttributes.StatAttributes)
        {
            unit.AddStat(statAttribute);
        }
    }
}