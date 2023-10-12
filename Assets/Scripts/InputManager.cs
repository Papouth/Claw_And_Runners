using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region Variables
    private Vector2 moveInput;
    private Vector2 mousePosition;
    private bool canJump;
    private bool canInteract;
    private bool canSelect;

    private float scrollMouse;
    #endregion


    #region Bool Functions
    public Vector2 Move => moveInput;
    public Vector2 Look => mousePosition;

    public float ScrollMouse => scrollMouse;

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

    private void OnScroll(InputValue value)
    {
        if (value.Get<float>() >= 1)
        {
            scrollMouse = 1;
        }
        else if (value.Get<float>() <= -1)
        {
            scrollMouse = -1;
        }
        else
        {
            scrollMouse = 0;
        }
    }
    #endregion
}