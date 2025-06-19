using UnityEngine;
using System.Collections;

public class MstrRandomPosSpawner : MonoBehaviour
{
    [SerializeField] Vector2 areaSize = new Vector2(30f, 30f);
    [SerializeField] GameObject spawnerPrefab;
    [SerializeField] int maxAttempts = 15;
    [SerializeField] float _curRespawnRate;
    [SerializeField] float _startRespawnRate;
    [SerializeField] float _respawnRateMult;
    [SerializeField] AstarPath _myAIGraph;

    [SerializeField] bool _isAHordeOn;
    [SerializeField] float _maxHordeTime;
    float _currentHordeTime;
    
    [Header("Debug")]
    [SerializeField] private int _curAttempts;
    [SerializeField] private bool _mnstrSpawned;

    private void Start()
    {
        _isAHordeOn = false;
        _curRespawnRate = _startRespawnRate;
        //StartCoroutine(SpawnRoutine());
    }

    private void Update()
    {
        if (_currentHordeTime > 0)
        {
            _currentHordeTime -= Time.deltaTime;
            HordeManager();
        }
    }

    public void ActivateHorde()
    {
        _currentHordeTime = _maxHordeTime;
    }

    private void HordeManager()
    {
        _curRespawnRate -= Time.deltaTime;
        if (_curRespawnRate <= 0)
        {
            StartCoroutine(SpawnRoutine());
            _startRespawnRate *= _respawnRateMult;
            if (_startRespawnRate <= 0.1)
            {
                _startRespawnRate = 0.1f;
            }
            _curRespawnRate = _startRespawnRate;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        _curAttempts = 0;
        _mnstrSpawned = false;

        while (_curAttempts < maxAttempts && !_mnstrSpawned)
        {
            _curAttempts++;
            
            Vector2 spawnPosition = GetRandomPositionInArea();
            var _mySpanwer = Instantiate(spawnerPrefab, spawnPosition, Quaternion.identity, transform);
            // El spawner necesita el AstarPath para ver si el punto esta dentro del Graph
            // Quiza se pueda hacer esto de una manera mas eficiente
            _mySpanwer.GetComponent<MstrSpawner>()._myGraphPath = _myAIGraph;

            // Esperar un frame para permitir que el spawner complete su lógica
            yield return new WaitForEndOfFrame();

            _mnstrSpawned = CheckTriangleStatus(_mySpanwer);
        }

        if (!_mnstrSpawned)
        {
            Debug.LogWarning($"No se pudo generar el triángulo después de {maxAttempts} intentos");
        }
    }

    private Vector2 GetRandomPositionInArea()
    {
        Vector2 center = transform.position;
        float x = Random.Range(center.x - areaSize.x / 2, center.x + areaSize.x / 2);
        float y = Random.Range(center.y - areaSize.y / 2, center.y + areaSize.y / 2);
        return new Vector2(x, y);
    }

    private bool CheckTriangleStatus(GameObject obj)
    {
        if (obj.GetComponent<MstrSpawner>().spawnedMonster == true)
        {
            print("Sí apareció un monstruo");
            return true;
        } else
        {
            print("No apareció nada");
            return false;
        }
        // Buscar cualquier triángulo activo en la escena
        //triangleSpawned = FindObjectOfType<MstrApearDissapear>() != null;
    }

}
