using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class TestSnake : MonoBehaviour
{
    [SerializeField] NavMeshAgent _agent;
    void Start()
    {
        
    }

    void Update()
    {
        //print(Mathf.Abs(_agent.velocity.x) + Mathf.Abs(_agent.velocity.y));
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Prueba para que se detenga al chocar con obstaculos en alta velocidad
        /*
        if (col.tag == "UnbreakableObstacle")
        {
            if (Mathf.Abs(_agent.velocity.x) + Mathf.Abs(_agent.velocity.y) >= 6.5f)
            {
                _agent.velocity = new Vector3(0, 0, 0);
                print("Ay choque");
            }
        }*/
    }
}
