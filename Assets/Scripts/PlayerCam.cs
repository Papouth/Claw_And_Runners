using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    #region Variables
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    [SerializeField] private float minClamp = 90f;
    [SerializeField] private float maxClamp = 90f;

    [SerializeField] private Transform orientation;

    private float xRotation;
    private float yRotation;
    [SerializeField] private Transform model;
    #endregion


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -minClamp, maxClamp);

        // rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        model.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}