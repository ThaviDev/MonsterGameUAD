using UnityEngine;

public class Q_EnemyManager : MonoBehaviour
{
    public static Q_EnemyManager instance { get; private set; }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Hay mas de una instancia de Q_EnemyManager, cuando es singleton");
        }
    }

    public void Start()
    {
        
    }


    public void Update()
    {
        var enemyManager = Q_EnemyManager.instance;
    }
}
