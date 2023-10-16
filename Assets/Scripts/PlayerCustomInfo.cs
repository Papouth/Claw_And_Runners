using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomInfo : MonoBehaviour
{
    #region Variables
    public List<GameObject> itemsValidate = new List<GameObject>(8);
    [SerializeField] private float rotSpeed;
    [SerializeField] private float friction;
    private float xDeg;
    #endregion



    #region Built In Methods
    private void Update()
    {
        RotatePlayerMouse();
    }
    #endregion


    #region Customs Methods
    private void RotatePlayerMouse()
    {
        if (Input.GetMouseButton(0))
        {
            xDeg -= Input.GetAxis("Mouse X") * rotSpeed * friction;
            transform.rotation = Quaternion.Euler(0, xDeg, 0);
        }
    }
    #endregion
}