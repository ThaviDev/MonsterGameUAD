using UnityEngine;

public class C_DetectMonsterAiming : MonoBehaviour
{
    [SerializeField] C_FlashLightMotor m_LightMotor;
    [SerializeField] LayerMask m_MonsterLayerMask;
    [SerializeField] LayerMask m_ObstacleLayerMask;
    Vector2 m_ParentFrontDirection;

    GameObject m_TestSaveMonster;

    void Start()
    {
        m_LightMotor = GetComponent<C_FlashLightMotor>();
    }
    void Update()
    {
        m_ParentFrontDirection = transform.parent.transform.up;
        Debug.DrawRay(transform.position, m_ParentFrontDirection, Color.magenta);
        if (m_LightMotor.IsFlashLightOn)
        {
            FindMonstersInLight();
        }
    }
    private void FindMonstersInLight()
    {
        Collider2D[] MonstInLight = Physics2D.OverlapCircleAll
            (transform.position, 
            m_LightMotor.LightRange, 
            m_MonsterLayerMask);

        for (int i = 0; i < MonstInLight.Length; i++)
        {
            m_TestSaveMonster = MonstInLight[i].gameObject;
            Vector2 _directionToTarget = (MonstInLight[i].gameObject.transform.position - transform.position).normalized;
            float angleToTarget = Vector2.Angle(m_ParentFrontDirection, _directionToTarget);

            if (angleToTarget > m_LightMotor.LightAngle / 2)
                continue;

            RaycastHit2D ray = Physics2D.Raycast
                (transform.position,
                _directionToTarget,
                m_LightMotor.LightRange);

            Debug.DrawRay(transform.position, _directionToTarget * m_LightMotor.LightRange, Color.green);

            if (ray.collider == null)
            {
                Debug.Log("El monstruo esta fuera del alacance del raycast");
                break;
            }

            if (ray.collider == MonstInLight[i])
            {
                MonstInLight[i].GetComponent<C_MonsterMotor>().HasBeenFoundByPlayer = true;
                Debug.Log("Monstruo detectado: " 
                    + MonstInLight[i].gameObject.name);
            }
            else
            {
                Debug.Log("Monstruo detectado " 
                    + MonstInLight[i].gameObject.name 
                    + " pero esta detras de muro " 
                    + ray.collider.name);
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, m_LightMotor.LightRange);
        /*
        if (m_TestSaveMonster != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, m_TestSaveMonster.transform.position);
        }
        */
    }
}
