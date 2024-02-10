using UnityEngine;


[CreateAssetMenu(fileName = "Stat", menuName = "ScriptableObjects/Stat")]
public class StatDescription : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public float DefaultValue;
}
