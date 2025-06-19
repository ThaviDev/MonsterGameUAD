using UnityEngine;

public class FurnitureBreakingBase : MonoBehaviour
{
    [SerializeField] GameObject _scrapObj;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Choco");
        if (collision.gameObject.tag == "Monster")
        {
            DestructionOfFurniture();
            Debug.Log("Me choca el monstruo : collider");
        }
    }

    private void DestructionOfFurniture()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject newScrap = Instantiate(_scrapObj, transform.position, transform.rotation);
            Rigidbody2D scrapRB = newScrap.GetComponent<Rigidbody2D>();
            if (scrapRB != null)
            {
                print("Tengo Rigid Body");
                float randomSpeed = Random.Range(0.2f, 4f);
                Vector2 randomDir = Random.insideUnitCircle.normalized;
                scrapRB.linearVelocity = randomSpeed * randomDir;
            }
        }
        Destroy(gameObject);
    }
}
