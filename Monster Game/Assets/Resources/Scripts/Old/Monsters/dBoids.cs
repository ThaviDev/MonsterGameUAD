using UnityEngine;

namespace daveBoids
{
    public class dBoids : MonoBehaviour
    {
        [Header("Data")]
        Vector3 _curForce; // Fuerza del frame pasado
        Vector3 _newForce; // Fuerza de la nueva dirección
        [SerializeField] float _mass; // masa del objeto para checar movimiento


        void Start()
        {

        }

        void Update()
        {

        }

        void BoidCalculation()
        {
            var Forces = Vector3.zero;

            // -- Arrive for Seek and others --
            //_speed = Arrive(_speed, _seekTarget.position, _seekArriveRatio);
            // -- Calculate Last Force --
            _curForce = (_newForce * _mass) + (Forces * (1 - _mass));
            // -- Move GameObject With Last Force Aplied --
            //transform.position += _lastForce.normalized * _speed * Time.deltaTime;
            _newForce = _curForce;
        }
    }
}