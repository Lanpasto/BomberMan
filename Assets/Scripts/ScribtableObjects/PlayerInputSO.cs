using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInputSO", menuName = "ScriptableObjects/PlayerInputSet")]
public class PlayerInputSO : ScriptableObject
{
    public List<KeyBind> PlayerBinds;
}
[Serializable]
public class KeyBind
{
    public InputManager.PlayerAction action;
    public KeyCode key;

}
