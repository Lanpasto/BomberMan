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
        
        MapManager.Instance.SetMap(mapGenerator.GenerateMap(LevelPropetries.width, LevelPropetries.height,LevelPropetries.countOfPlayer));
        
        SpawnPlayer(playerTestDescription);
    }

    private void SpawnPlayer(UnitDescriptionSO unitDescription)
    {
        var unit = Instantiate(unitDescription.UnitBehaviour);
        unit.UnitDescription = unitDescription;
        unit.Initialize(unitDescription.EntityDescription, Vector2.zero);
       
    }

    
}