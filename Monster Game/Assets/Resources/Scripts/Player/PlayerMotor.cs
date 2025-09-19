using System;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public static Action<Collider2D> OnPyrHit;

    Collider2D _yCol;
    void Start()
    {
        
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.V))
        {
            OnPyrHit?.Invoke();
        }*/
    }

    private void OnTriggerEnter2D(Collider2D otherCol)
    {
        if (otherCol.gameObject.layer == 6) // Monster Layer
        {
            OnPyrHit.Invoke(otherCol);
        }
    }
}
