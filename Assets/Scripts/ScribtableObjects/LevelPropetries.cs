using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelPropetries", menuName = "ScriptableObjects/LevelPropetries")]
public class LevelPropetries : ScriptableObject
{
    public int width;
    public int height;
    public int countOfPlayer;
    public RandomTypeEnum randomType;
}
