using System.Collections.Generic;
using UnityEngine;

namespace SteeringBehaviours
{
    public class C_Boid : MonoBehaviour
    {
        //[SerializeField] t_BoidTypes m_CurBoidType;

        [Header("Data")]
        [SerializeField] float m_Mass;
        [SerializeField] float m_Speed;
        public float BoidSpeed { get { return m_Speed; } }
        [SerializeField] float m_MaxSpeed;
        public float BoidMaxSpeed { set { m_MaxSpeed = value; } }
        [SerializeField] bool m_UseArriveSeek;

        [Header("Seek")]
        [SerializeField] Transform m_SeekTarget;
        public Transform SeekTarget { get { return m_SeekTarget; } set { m_SeekTarget = value; } }
        [SerializeField] float m_SeekImpetu;
        public float SeekImpetu { get { return m_SeekImpetu; } set { m_SeekImpetu = value; } }
        //public Color m_SeekColor = new Color(1,1,1,1);
        [SerializeField] Color m_SeekColor = Color.red;

        [Header("Flee")]
        [SerializeField] Transform m_FleeTarget;
        public Transform FleeTarget { get { return m_FleeTarget; } set { m_FleeTarget = value; } }
        [SerializeField] float m_FleeImpetu;
        public float FleeImpetu { get {return m_FleeImpetu; } set { m_FleeImpetu = value; } }
        [SerializeField] Color m_FleeColor = Color.yellow;

        [Header("SeekRatio")] // Seguir hasta cierto rango
        [SerializeField] Transform m_SeekRatioTarget;
        public Transform SeekRatioTarget { get { return m_SeekRatioTarget; } set { m_SeekRatioTarget = value; } }
        [SerializeField] float m_SeekRatioImpetu;
        public float SeekRatioImpetu { get { return m_SeekRatioImpetu; } set { m_SeekRatioImpetu = value; } }
        [SerializeField] float m_SeekRatioRatio;
        public float SeekRatioValue { get { return m_SeekRatioRatio; } set { m_SeekRatioRatio = value; } }
        [SerializeField] Color m_SeekRatioColor = Color.red;

        [Header("FleeRatio")] // Huir solo al estar en el rango
        [SerializeField] Transform m_FleeRatioTarget;
        public Transform FleeRatioTarget { get { return m_FleeRatioTarget; } set { m_FleeRatioTarget = value; } }
        [SerializeField] float m_FleeRatioImpetu;
        [SerializeField] float m_FleeRatio;
        [SerializeField] Color m_FleeRatioColor = Color.yellow;

        [Header("WanderTime")]
        [SerializeField] float m_WanderTimeCount;
        [SerializeField] float m_WanderTimeImpetu;
        [SerializeField] Color m_WanderTimeColor = Color.yellow;
        Vector3 m_WanderTimeRandPos;
        bool m_WanderTimeCanGetNewDir;
        Vector3 m_WanderTimeDir;

        [Header("WanderPos")]
        [SerializeField] float m_WanderPosImpetu;
        [SerializeField] Color m_WanderPosColor = Color.yellow;
        Vector3 m_WanderPosRandPos;

        [Header("WanderWait")]
        [SerializeField] float m_WanderWaitTime;
        [SerializeField] float m_WanderWaitImpetu;
        [SerializeField] Color m_WanderWaitColor = Color.yellow;
        private float m_WanderWaitCurTime;
        private Vector3 m_WanderWaitRandPos;

        [Header("Pursue")]
        [SerializeField] C_Boid m_PursueOtherBoid;
        public C_Boid PursueOtherBoid { get { return m_PursueOtherBoid; } set { m_PursueOtherBoid = value; } }
        [SerializeField] float m_PursueTimeToArrive;
        [SerializeField] float m_PursueImpetu;
        [SerializeField] Color m_PursueColor = Color.magenta;

        [Header("Evade")] // Usado para Huir de un perseguidor
        [SerializeField] C_Boid m_EvadeOtherBoid;
        public C_Boid EvadeOtherBoid { get { return m_EvadeOtherBoid; } set { m_EvadeOtherBoid = value; } }
        [SerializeField] float m_EvadeTimeAsEscape;
        [SerializeField] float m_EvadeImpetu;
        [SerializeField] Color m_EvadeColor = Color.cyan;

        [Header("PathFollower")]
        [SerializeField] List<Vector3> m_Path = new List<Vector3>();
        public List<Vector3> Path { get { return m_Path; } set { m_Path = value; } }
        [SerializeField] float m_PathPosArriveRatio;
        [SerializeField] float m_PathImpetu;
        [SerializeField] Color m_PathColor = Color.green;
        private List<Vector3> m_CurrentPath = null;
        private int m_PathIndex = 0;

        //[Header("Avoid")] // Usado para evitar obstaculos

        //[SerializeField] List<Transform> m_Targets = new List<Transform>();
        //private Dictionary<t_BoidTypes,Transform> m_BoidTargets = new Dictionary<t_BoidTypes, Transform> ();
        //[SerializeField] List<> m_Impetus;
        //[SerializeField] private float[] m_Impetus;
        //private Dictionary<t_BoidTypes, float> m_BoidImpetus = new Dictionary<t_BoidTypes, float>();
        //[SerializeField] float m_GetToPlayerImpetu;
        //[SerializeField] float m_ObstacleImpetu;

        [Header ("Arrive")]
        [SerializeField] float m_ArriveRatio;
        [Header ("Momentum")]
        [SerializeField] Vector3 m_PastForce;
        [SerializeField] Vector3 m_NewForce;
        private void Start()
        {

        }
        void Update()
        {
            BoidMovement();
        }
        void BoidMovement()
        {
            var Forces = Vector3.zero;

            /*
            switch (m_CurBoidType)
            {
                case t_BoidTypes.Seek:
                    Forces += Is_Seek(m_SeekTarget.position, m_GetToPlayerImpetu);
                    break;

            } */

            if (m_SeekTarget != null)
            {
                var seekForce = Seek(m_SeekTarget.position, m_SeekImpetu);
                Debug.DrawLine(transform.position, transform.position + seekForce, m_SeekColor);
                Forces += seekForce;
            }
            if (m_FleeTarget != null)
            {
                var fleeForce = Flee(m_FleeTarget.position, m_FleeImpetu);
                Debug.DrawLine(transform.position, transform.position + fleeForce, m_FleeColor);
                Forces += fleeForce;
            }
            if (m_SeekRatioTarget != null)
            {
                var seekForce = SeekRatio(m_SeekRatioRatio, m_SeekRatioTarget.position, m_SeekRatioImpetu);
                Debug.DrawLine(transform.position, transform.position + seekForce, m_SeekRatioColor);
                Forces += seekForce;
            }
            if (m_FleeRatioTarget != null)
            {
                var fleeForce = FleeRatio(m_FleeRatio, m_FleeRatioTarget.position, m_FleeRatioImpetu);
                Debug.DrawLine(transform.position, transform.position + fleeForce, m_FleeRatioColor);
                Forces += fleeForce;
            }
            if (m_Path != null && m_Path.Count > 0)
            {
                var pathForce = FollowPath(m_Path, m_PathPosArriveRatio);
                Debug.DrawLine(transform.position, transform.position + pathForce, m_PathColor);
                Forces += pathForce;
            }

            // -- Arrive -- 
            if (m_UseArriveSeek)
            {
                m_Speed = Arrive(m_Speed, m_SeekTarget.position, m_ArriveRatio);
            }
            // -- Calculate PastForce --
            m_PastForce = (m_NewForce * m_Mass) + (Forces * (1 - m_Mass));
            // -- Move GameObject With PastForce Aplied --
            transform.position += m_PastForce.normalized * m_Speed * Time.deltaTime;
            m_NewForce = m_PastForce;
        }

        public float Arrive(float speed, Vector3 target, float Arrive_radius)
        {
            // -- Find Distance with Magnitude --
            var distance = (target - transform.position).magnitude;
            // -- Keep max speed when not in radius --
            if (distance > Arrive_radius)
            {
                if (speed < m_MaxSpeed)
                {
                    speed = m_MaxSpeed;
                }
                return speed;
            }
            // -- Otherwise reduce the speed by dividing the distance from the radius --
            // -- This will reduce the speed algoritmically until its basically 0 --
            else
            {
                var newSpeed = distance / Arrive_radius;
                speed = m_MaxSpeed * newSpeed;
                return speed;
            }
        }
        public Vector3 Seek(Vector3 target, float impetu)
        {
            // -- Find Direction Tip-To-Tail Rule --
            var Dir = (target - transform.position).normalized;
            // -- Get Force Multiplyng Impetu --
            var Force = Dir * impetu;
            return Force;
        }
        public Vector3 Flee(Vector3 target, float impetu)
        {
            return Seek(target, impetu) * -1;
        }
        public Vector3 SeekRatio(float ratio, Vector3 target, float impetu)
        {
            var dist = (target - transform.position).magnitude;
            if (dist > ratio)
            {
                return Vector3.zero;
            }
            else
            {
                return Seek(target, impetu);
            }
        }
        public Vector3 FleeRatio(float ratio, Vector3 target, float impetu)
        {
            var dist = (target - transform.position).magnitude;
            if (dist > ratio)
            {
                return Vector3.zero;
            } else
            {
                return Flee(target, impetu);
            }
        }
        public Vector3 WanderBasic(float minX, float maxX, float minY, float maxY)
        {
            // -- Every call gets a new position in an area --
            var r = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY));
            return r;
        }
        public Vector3 WanderTime(float minX, float maxX, float minY, float maxY, float myTime)
        {
            m_WanderWaitCurTime -= Time.deltaTime;
            if (m_WanderWaitCurTime <= 0)
            {
                m_WanderTimeCanGetNewDir = true;
                m_WanderWaitCurTime = myTime;
            }
            if (m_WanderTimeCanGetNewDir)
            {
                m_WanderTimeRandPos = WanderBasic(minX, maxX, minY, maxY);
                m_WanderTimeDir = (m_WanderTimeRandPos - transform.position).normalized;
                m_WanderTimeCanGetNewDir = false;
            }
            return m_WanderTimeDir;
        }
        public Vector3 WanderPosition(float minX, float maxX, float minY, float maxY)
        {
            if (m_WanderPosRandPos == Vector3.zero)
            {
                m_WanderPosRandPos = WanderBasic(minX, maxX, minY, maxY);
            }
            var distance = (m_WanderPosRandPos - transform.position).magnitude;
            if (distance < 0.1f)
            {
                m_WanderPosRandPos = WanderBasic(minX, maxX, minY, maxY);
            }
            return m_WanderPosRandPos;
        }
        public Vector3 WanderWaitInPosition(float minX, float maxX, float minY, float maxY, float waitTime)
        {
            if (m_WanderWaitRandPos == Vector3.zero)
            {
                m_WanderWaitCurTime  = waitTime;
                m_WanderWaitRandPos = WanderBasic(minX, maxX, maxY, waitTime);
            }
            var distance = (m_WanderWaitRandPos - transform.position).magnitude;
            if (distance < 0.1f)
            {
                m_WanderWaitCurTime -= Time.deltaTime;
                if (m_WanderWaitCurTime <= 0f)
                {
                    m_WanderPosRandPos = WanderBasic(minX, maxX, maxY, waitTime);
                    m_WanderWaitCurTime = waitTime;
                }
            }
            return m_WanderWaitRandPos;
        }
        public Vector3 Pursue(C_Boid other, float arriveTime, float impetu)
        {
            C_Boid myOtherBoid = other;
            var predPos = other.transform.position +
                ((other.transform.position - transform.position)
                * other.BoidSpeed * arriveTime);
            var predRadious = other.BoidSpeed * arriveTime;

            if(predRadious > (other.transform.position - transform.position).magnitude)
            {
                return predPos;
            }
            else
            {
                return other.transform.position - predPos + ((other.transform.position - transform.position).normalized
                    * (other.transform.position - transform.position).magnitude);
            }
        }
        public Vector3 Evade(C_Boid other, float arriveTime, float impetu)
        {
            return Pursue(other, arriveTime, impetu) * -1;
        }
        public Vector3 FollowPath(List<Vector3> Path, float posArriveRatio)
        {
            print("Sigo Camino");
            // Declarar camino y reiniciar indice si el camino es diferente al actual
            if (m_CurrentPath != Path)
            {
                m_CurrentPath = Path;
                m_PathIndex = 0;
            }

            if (Path == null 
                || Path.Count == 0 
                || m_PathIndex >= m_CurrentPath.Count)
            {
                return Vector3.zero;
            }

            var target = m_CurrentPath[m_PathIndex];
            var toTarget = target - transform.position;
            var dist = toTarget.magnitude;

            if (dist <= posArriveRatio)
            {
                m_PathIndex++;
                if (m_PathIndex >= m_CurrentPath.Count)
                {
                    return Vector3.zero;
                }
                target = m_CurrentPath[m_PathIndex];
            }

            return Seek(target, m_PathImpetu);
        }
        private void OnDrawGizmos()
        {
            if (m_SeekRatioTarget != null)
            {
                Gizmos.color = m_SeekRatioColor;
                Gizmos.DrawWireSphere(m_SeekRatioTarget.position, m_SeekRatioRatio);
            }
            if (m_FleeRatioTarget != null)
            {
                Gizmos.color = m_FleeRatioColor;
                Gizmos.DrawWireSphere(m_FleeRatioTarget.position, m_FleeRatio);
            }
        }
    }
}

