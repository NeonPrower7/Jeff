using UnityEngine;
using Cinemachine;

public class ResetConfiner : MonoBehaviour
{
    private float timer;
    void Update()
    {
        if (timer < 0)
        {
            GetComponent<CinemachineConfiner2D>().InvalidateCache();
            timer = 1;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}