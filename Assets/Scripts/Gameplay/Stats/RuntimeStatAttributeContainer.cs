using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RuntimeStatAttributeContainer
{
    public Stats Stats = new Stats();

    public void SetDefaultStats(Stats stats)
    {
        this.Stats = stats;
    }

    public void AddStat(StatAttribute statAttribute)
    {
        Stats.StatAttributes.Add(statAttribute);
    }
}
