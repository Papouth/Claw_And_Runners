using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathisAnimation : MonoBehaviour
{
    [SerializeField] private PlayerController PC;

    private void Start()
    {
        PC.MathisAnim();    
    }
}
