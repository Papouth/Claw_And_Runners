using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandTir : MonoBehaviour
{
    #region Variables
    [Header("Cibles")]
    [SerializeField] private Transform[] ciblesPivots;
    [SerializeField] private float speedRot;
    [SerializeField] private Transform ballonCircle;
    [SerializeField] private Transform tripleTarget;
    [SerializeField] private Transform[] ballonParents;
    #endregion


    #region Built-In Methods
    private void Start()
    {
        for (int i = 0; i < ciblesPivots.Length; i++)
        {
            ciblesPivots[i].transform.rotation = Quaternion.identity;
        }
    }

    private void Update()
    {
        RotateTargets();
    }
    #endregion


    #region Customs Methods
    private void RotateTargets()
    {
        ballonCircle.Rotate(Vector3.right * speedRot * Time.deltaTime);
        tripleTarget.Rotate(Vector3.right * speedRot * Time.deltaTime);

        for (int i = 0; i < ballonParents.Length; i++)
        {
            ballonParents[i].Rotate(-Vector3.right * speedRot * Time.deltaTime);
        }
    }

    #endregion
}