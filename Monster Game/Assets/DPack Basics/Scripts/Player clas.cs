using UnityEngine;

public class Playerclas : MonoBehaviour
{
    [Header("Ritmo Cardiaco")]
    public float HeartRate = 70f; //Default heart rate
    public float MaxHeartRate = 130f;
    public float MinHeartRate = 50f;
    public float HeartRateChangeSpeed = 20f;

    [Header("Detección de Objetos")]
    public string TargetTag = "Peligro"; // Tag for the enemy or danger objects
    public float DetectionRange = 5f; // Min distance to detect objects

    void Update()
    {
        ChangeHeartBeat();
    }

    void ChangeHeartBeat()
    {

        GameObject[] EnemyObject = GameObject.FindGameObjectsWithTag(TargetTag);
        bool isNear = false;

        foreach (GameObject obj in EnemyObject)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < DetectionRange)
            {
                isNear = true;
                break;
            }
        }

        if (isNear)
        {
            // Aumenta el ritmo cardiaco poco a poco
            HeartRate = Mathf.MoveTowards(HeartRate, MaxHeartRate, HeartRateChangeSpeed * Time.deltaTime);
        }
        else
        {
            // Disminuye el ritmo cardiaco poco a poco
            HeartRate = Mathf.MoveTowards(HeartRate, MinHeartRate, HeartRateChangeSpeed * Time.deltaTime);
        }
    }

    void Interact()
    {}
}
