using System;
using UnityEngine;

public class GMTestGameplay : MonoBehaviour
{
    // Game Manager Test Gameplay
    public static Action OnGameOver;
    void Start()
    {
        
    }
    void Update()
    {
        PlayerStadistics.OnPyrDeath += StartGameOverSequence;
    }

    void StartGameOverSequence()
    {
        OnGameOver?.Invoke();
    }
}
