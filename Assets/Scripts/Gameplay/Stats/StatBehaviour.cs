using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class StatAttribute
{
    public StatDescription statDescription;
    public float Value;
            
}
[Serializable]
public class Stats
{
    public List<StatAttribute> StatAttributes;

    public float GetStatValue(StatDescription statDescription)
    {
        foreach (var stat in StatAttributes)
        {
            if (statDescription == stat.statDescription)
            {
                return stat.Value;
            }
        }
        
        return StatAttributeContainer.Instance.GetDefaultStat(statDescription);
    }

    public float GetStatValue(string statName)
    {
        foreach (var stat in StatAttributes)
        {
            if (statName == stat.statDescription.Name)
            {
                return stat.Value;
            }
        }
        
        return StatAttributeContainer.Instance.GetDefaultStat(statName);
    }

    public StatAttribute GetStartAttributeById(string nameId, UnitBehaviour unit = null)
    {
        foreach (var stat in StatAttributes)
        {
            if (nameId == stat.statDescription.Name)
            {
                return stat;
            }
        }

        var statAttributeContainer = StatAttributeContainer.Instance;
        
        StatAttribute newsStatAttribute = new();
        newsStatAttribute.statDescription = statAttributeContainer.GetStatDescription(nameId);
        newsStatAttribute.Value += statAttributeContainer.GetDefaultStat(nameId);

        if (newsStatAttribute.statDescription == null)
        {
            return null;
        }
        else
        {
            if (unit != null && newsStatAttribute.Value > 0) unit.AddStat(newsStatAttribute);
            return newsStatAttribute;
        }
    }
}
