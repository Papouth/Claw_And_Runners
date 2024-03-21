using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region Variables
    private Vector2 moveInput;
    private Vector2 mousePosition;
    private Vector2 scrollMouse;

    private bool canJump;
    private bool canInteract;
    private bool canSelect;
    private bool canSlot1;
    private bool canSlot2;
    #endregion


    #region Bool Functions
    public Vector2 Move => moveInput;
    public Vector2 Look => mousePosition;
    public Vector2 ScrollMouse => scrollMouse;

    public bool CanJump
    {
        get { return canJump; }
        set { canJump = value; }
    }
    public bool CanInteract
    {
        get { return canInteract; }
        set { canInteract = value; }
    }

    public bool CanSelect
    {
        get { return canSelect; }
        set { canSelect = value; }
    }

    public bool CanSlot1
    {
        get { return canSlot1; }
        set { canSlot1 = value; }
    }

    public bool CanSlot2
    {
        get { return canSlot2; }
        set { canSlot2 = value; }
    }
    #endregion


    #region Functions
    public void OnMove(InputValue value)
    {
        // On récupère la valeur du mouvement qu'on stock dans un Vector2
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        mousePosition = value.Get<Vector2>();
    }

    public void OnJump()
    {
        // Récupération de l'input
        canJump = true;
    }

    public void OnInteract()
    {
        canInteract = true;
    }

    public void OnSelect()
    {
        canSelect = !canSelect;
    }

    public void OnSlot1()
    {
        canSlot1 = true;
    }

    public void OnSlot2()
    {
        canSlot2 = true;
    }

    public void OnNextSlot(InputValue value)
    {
        scrollMouse = value.Get<Vector2>();

        if (scrollMouse.y >= 1)
        {
            scrollMouse.y = 1;
        }
        else if (scrollMouse.y <= -1)
        {
            scrollMouse.y = -1;
        }
        else
        {
            scrollMouse.y = 0;
        }
    }
    #endregion
}