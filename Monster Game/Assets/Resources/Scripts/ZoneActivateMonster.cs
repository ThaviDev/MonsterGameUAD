using UnityEngine;

public class ZoneActivateMonster : MonoBehaviour
{
    [SerializeField] private GameObject _monster;
    [SerializeField] private float _monsterAliveTime;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var myMonster = Instantiate(_monster,transform.position,transform.rotation);
            var monsterTime = myMonster.GetComponent<MstrApearDissapear>();
            //monsterTime._aliveTime = 60;
            monsterTime.SetAliveTime(_monsterAliveTime);
            Destroy(this.gameObject);
        }
    }
}
