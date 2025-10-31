using UnityEngine;

public class ScrapPickup : MonoBehaviour
{
    [SerializeField] IntSCOB _SCOB_ScrapAmount;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AddScrapAmmount();
        }
    }

    private void AddScrapAmmount()
    {
        _SCOB_ScrapAmount.SCOB_Value += 1;
        Destroy(this.gameObject);
    }
}
