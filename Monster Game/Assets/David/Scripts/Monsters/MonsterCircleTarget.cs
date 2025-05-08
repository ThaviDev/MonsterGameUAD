using Pathfinding;
using UnityEngine;

public class MonsterCircleTarget : MonoBehaviour
{
    [SerializeField] float _radius = 5;
    [SerializeField] float _offset = 2;
    IAstarAI ai;

    void OnEnable()
    {
        ai = GetComponent<IAstarAI>();
    }

    void Update()
    {

    }

    public void CircleTarget(Transform target)
    {
        var normal = (ai.position - target.position).normalized;
        var tangent = Vector3.Cross(normal, target.forward);

        ai.destination = target.position + normal * _radius + tangent * _offset;
    }

    /*
    public void DrawGizmos()
    {
        if (target) Draw.Circle(target.position, target.up, radius, Color.white);
    } */
}
