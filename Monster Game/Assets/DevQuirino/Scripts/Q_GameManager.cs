using UnityEngine;

public class Q_GameManager: MonoBehaviour
{
    public static Q_GameManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Hay mas de una instancia de Q_GameManager, cuando es singleton");
        }
    }

    void Start()
    {
        var gamemanager = Q_GameManager.instance;
        var enemyManager = Q_EnemyManager.instance;

        enemyManager.Start();
    }


    void Update()
    {
        var gamemanager = Q_GameManager.instance;
        var enemyManager = Q_EnemyManager.instance;

        enemyManager.Update();


    }
}
