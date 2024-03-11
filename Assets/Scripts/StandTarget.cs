using UnityEngine;

public class StandTarget : MonoBehaviour
{
    [HideInInspector] public bool targetHit;

    public void TargetHit()
    {
        if (!targetHit)
        {
            targetHit = true;
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }
    }
}