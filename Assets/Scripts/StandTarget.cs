using UnityEngine;

public class StandTarget : MonoBehaviour
{
    [HideInInspector] public bool targetHit;
    [SerializeField] private bool needFakeDestroy;
    private StandTir standTir;



    private void Start()
    {
        standTir = GetComponentInParent<StandTir>();
    }

    private void Update()
    {
        ResetTarget();
    }

    public void TargetHit()
    {
        if (!targetHit && !needFakeDestroy)
        {
            targetHit = true;
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);

            standTir.validTarget--;
        }
        else if (!targetHit && needFakeDestroy)
        {
            targetHit = true;
            gameObject.SetActive(false);

            standTir.validTarget--;
        }
    }

    private void ResetTarget()
    {
        if (standTir.victory)
        {
            standTir.victory = false;
            targetHit = false;
            gameObject.SetActive(true);
        }
    }
}