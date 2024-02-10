using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitDescription", menuName = "ScriptableObjects/")]
public class UnitDescriptionSO : ScriptableObject
{
    public Stats UnitStatsAttributes;
    public UnitBehaviour UnitBehaviour;
}
