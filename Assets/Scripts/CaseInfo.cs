using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseInfo : MonoBehaviour
{
    public int caseId;

    [SerializeField] private UIManager UImanager;

    public void SetUIVariable()
    {
        UImanager.caseInfo = this;
    }
}