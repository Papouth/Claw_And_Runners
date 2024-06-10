using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Ladder : MonoBehaviour
{
    private CharacterController CC;
    private bool canClimbLadder;
    [SerializeField] private float speedLadder = 1f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>())
        {
            CC = other.GetComponent<CharacterController>();

            canClimbLadder = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterController>())
        {
            canClimbLadder = false;
        }
    }

    private void Update()
    {
        if (canClimbLadder)
        {
            if(Input.GetKey(KeyCode.W))
            {
                CC.Move(Vector3.up * Time.deltaTime * speedLadder);
            }
            if (Input.GetKey(KeyCode.S))
            {
                CC.Move(-Vector3.up * Time.deltaTime * speedLadder);
            }
        }
    }
}