using UnityEngine;

public class MstrSpawner : MonoBehaviour
{
    [SerializeField] float _checkRadius = 1f;
    [SerializeField] GameObject[] _monsterPrefab;
    [SerializeField] LayerMask _checkLayers;
    public bool spawnedMonster;
    public AstarPath _myGraphPath;

    private void Start()
    {
        // Realizar el chequeo de colisiones circular
        //bool hasCollision = Physics2D.OverlapCircle(transform.position, _checkRadius, _checkLayers);

        if (_myGraphPath.IsPointOnNavmesh(this.gameObject.transform.position))
        {
            // Si no hay colisiones y es un punto del navmesh, instanciar el triángulospawnerPrefab[Random.Range(0, 2)]
            Instantiate(_monsterPrefab[Random.Range(0, _monsterPrefab.Length)], transform.position, Quaternion.identity);
            spawnedMonster = true;
            Destroy(gameObject, 1);
        }
        else
        {
            // Si hay colisiones o no es un punto del navmesh, destruir inmediatamente
            spawnedMonster = false;
            Destroy(gameObject, 1);
        }
    }

    // Opcional: Dibujar gizmo para visualizar el radio en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _checkRadius);
    }
}
