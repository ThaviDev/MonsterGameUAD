using UnityEngine;

public class MstrSpawner : MonoBehaviour
{
    [SerializeField] float _checkRadius = 1f;
    [SerializeField] GameObject _monsterPrefab;
    [SerializeField] LayerMask _checkLayers;
    public bool spawnedMonster;

    private void Start()
    {
        // Realizar el chequeo de colisiones circular
        bool hasCollision = Physics2D.OverlapCircle(transform.position, _checkRadius, _checkLayers);

        if (hasCollision)
        {
            // Si hay colisiones, destruir inmediatamente
            spawnedMonster = false;
            Destroy(gameObject,1);
        }
        else
        {
            // Si no hay colisiones, instanciar el triángulo
            Instantiate(_monsterPrefab, transform.position, Quaternion.identity);
            spawnedMonster = true;
            Destroy(gameObject,1);
        }
    }

    // Opcional: Dibujar gizmo para visualizar el radio en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _checkRadius);
    }
}
