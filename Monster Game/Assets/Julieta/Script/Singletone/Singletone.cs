using UnityEngine;
using UnityEngine.SceneManagement;

public class Singletone<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _instance;
    [SerializeField] protected bool _OnlyOneScene = true;

    public static T Instance
    {
        get { if (_instance == null) 
            { 
            _instance = FindFirstObjectByType<T>();
                if(_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<T>();
                    singletonObject.name = typeof(T).ToString() + "(Singletone)";
                    DontDestroyOnLoad(singletonObject);
                }
            } 
        return _instance;
        }
    }
     

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if (!_OnlyOneScene)
            {
                DontDestroyOnLoad(_instance);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
        else
        {
                Destroy(gameObject);
        }
    }

    protected virtual void Start()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    protected virtual void OnSceneUnloaded(Scene currentScene)
    {
        if(_OnlyOneScene) 
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
