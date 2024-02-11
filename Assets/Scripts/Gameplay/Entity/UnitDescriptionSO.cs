using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "UnitDescription", menuName = "ScriptableObjects/")]
public class UnitDescriptionSO : ScriptableObject
{
    public Stats UnitStatsAttributes;
    public UnitBehaviour UnitBehaviour;
    public PlayerInputSO PlayerInput;
    public EntityDescription EntityDescription;

}