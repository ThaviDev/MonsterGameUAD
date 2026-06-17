using UnityEngine;

public class C_DistanceBNW : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    [SerializeField] float distance;
    public C_PProcessingManager ppManager;
    void Update()
    {
        distance = Vector2.Distance(pointA.position, pointB.position);
        if (distance >= 10)
        {
            ppManager.PostProcessingIntensity = 0;
        } else
        {
            ppManager.PostProcessingIntensity = 1 - (distance / 10f);
        }
    }
}
