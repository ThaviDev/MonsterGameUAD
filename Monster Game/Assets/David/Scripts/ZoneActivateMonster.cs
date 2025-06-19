using UnityEngine;

public class ZoneActivateMonster : MonoBehaviour
{
    [SerializeField] private GameObject _monster;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var myMonster = Instantiate(_monster,transform.position,transform.rotation);
            var monsterTime = myMonster.GetComponent<MstrApearDissapear>();
            monsterTime._aliveTime = 180;
            Destroy(this.gameObject);
        }
    }
}
