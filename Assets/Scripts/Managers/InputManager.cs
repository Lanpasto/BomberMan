using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static Action<KeyCode> InputAction = delegate { };
    public List<PlayerInputSO> PlayerInputSos; 

    public enum PlayerAction
    {
        up,
        down,
        left,
        right,
        bomb,
    }

    private void Update()
    {
        foreach (var playerInput in PlayerInputSos)
        {
            foreach (var bind in playerInput.PlayerBinds)
            {
                if (Input.GetKey(bind.key))
                {
                    InputAction?.Invoke(bind.key); 
                }
            }
        }
    }
}
