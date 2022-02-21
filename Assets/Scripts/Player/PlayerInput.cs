using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 GetMovementInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        return new Vector2(horizontalInput, 0);
    }
   
    public bool IsJumpButtonDown()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
    public bool IsJumpButtonHeld()
    {
        return Input.GetKey(KeyCode.Space);
    }
    public bool IsJumpButtonUp()
    {
        return Input.GetKeyUp(KeyCode.Space);
    }
}
