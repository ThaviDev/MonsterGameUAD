using System.Collections;
using UnityEngine;

public class MonsterAtkProximity : MonoBehaviour
{
    public bool _pyrIsInProximity
    {
        set { _pyrIsInProximity = value; } 
    }
    [SerializeField] float _srtUpTime = 1f;
    [SerializeField] float _recoveryTime = 1f;
    [SerializeField] float _distance = 1f;

    [SerializeField] private float _atkCurrentCooldown = 0f;
    [SerializeField] private float _atkCooldown = 4f;
    [SerializeField] private Collider2D _atkCollider;
    void Start()
    {

    }

    void Update()
    {
        if (_atkCurrentCooldown > 0)
        {
            _atkCollider.enabled = false;
            _atkCurrentCooldown -= Time.deltaTime;
        } else
        {
            _atkCollider.enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.U) && _atkCurrentCooldown <= 0)
        {
            StartCoroutine(AttackingSequence());
        }


    }

    IEnumerator AttackingSequence()
    {
        print("Empiezo a atacar");
        yield return new WaitForSeconds(_srtUpTime);

        print("Empiezo a atacar");
        ExecuteAttackContact();

        print("Ya ataque");
        yield return new WaitForSeconds(_recoveryTime);
        print("Termino de atacar");
    }

    private void ExecuteAttackContact()
    {
        print("Ataqueee!!");

    }
    private void OnTriggerEnter2D(Collider2D otherCol)
    {
        if (otherCol.gameObject.layer == 7) // Player Layer
        {
            _atkCollider.enabled = false;
            _atkCurrentCooldown = _atkCooldown;
        }
    }
}
