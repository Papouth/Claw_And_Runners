using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandTir : MonoBehaviour
{
    #region Variables
    [Header("Cibles")]
    public Transform[] ciblesPivots;
    [SerializeField] private float speedRot;
    [SerializeField] private Transform ballonCircle;
    [SerializeField] private Transform tripleTarget;
    [SerializeField] private GameObject[] tripleTargetGameObject;
    [SerializeField] private GameObject[] simpleTarget;
    [SerializeField] private Transform[] ballonParents;
    [HideInInspector] public int validTarget;
    [HideInInspector] public bool victory;
    #endregion


    #region Built-In Methods
    private void Start()
    {
        validTarget = ciblesPivots.Length + ballonParents.Length + simpleTarget.Length + tripleTargetGameObject.Length;
    }

    private void Update()
    {
        RotateTargets();

        VictoryActivity();
    }
    #endregion


    #region Customs Methods
    private void RotateTargets()
    {
        ballonCircle.Rotate(Vector3.right * speedRot * Time.deltaTime);
        tripleTarget.Rotate(Vector3.right * speedRot * Time.deltaTime);

        // Rotate target & initialization
        for (int i = 0; i < ballonParents.Length; i++)
        {
            ballonParents[i].Rotate(-Vector3.right * speedRot * Time.deltaTime);
        }
    }

    private void StaticTargetRotation()
    {
        for (int i = 0; i < ciblesPivots.Length; i++)
        {
            ciblesPivots[i].transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    public void ResetActivity()
    {
        StaticTargetRotation();

        validTarget = ciblesPivots.Length + ballonParents.Length + simpleTarget.Length + tripleTargetGameObject.Length;
    }

    public void VictoryActivity()
    {
        if (validTarget <= 0 && !victory)
        {
            victory = true;
            // Musique ou truc dans le genre comme des confettis
            Debug.Log("Bien joué mon boeuf");
        }
    }
    #endregion
}