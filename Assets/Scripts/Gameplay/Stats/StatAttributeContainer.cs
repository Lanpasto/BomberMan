using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class StatAttributeContainer : MonoBehaviour
{
    public static StatAttributeContainer Instance;
    public List<StatDescription> DefaultStatAttributes;
    private void Awake()
    {
       Instance = this;
    }

    public float GetDefaultStat(StatDescription statDescription)
    {
        foreach (var statAttribute in DefaultStatAttributes)
        {
            if (statAttribute == statDescription)
            {
                return statAttribute.DefaultValue;
            }
        }
        Debug.LogError("No StatAttribute in container named " + statDescription.Name);
        return 0;
    }
    
    public float GetDefaultStat(string statName)
    {
    
        foreach (var statAttribute in DefaultStatAttributes)
        {
            if (statAttribute.Name == statName)
            {
                return statAttribute.DefaultValue;
            }
        }
        Debug.LogError("No StatAttribute in container named " + statName);
        return 0;
    }

    public StatDescription GetStatDescription(string statName)
    {
        foreach (var statAttribute in DefaultStatAttributes)
        {
            if (statAttribute.Name == statName)
            {
                return statAttribute;
            }
        }
        Debug.LogError("No StatAttribute in container named " + statName);
        return null;
    }


}
