using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBehaviour : MonoBehaviour
{
    public UnitDescriptionSO playerTestDescription;

    private void Start()
    {
        var unit = Instantiate(playerTestDescription.UnitBehaviour);
        
        foreach (var statAttribute in playerTestDescription.UnitStatsAttributes.StatAttributes)
        {
            unit.AddStat(statAttribute);
        }
    }
}