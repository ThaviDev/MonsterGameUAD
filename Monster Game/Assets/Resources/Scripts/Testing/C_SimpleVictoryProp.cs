using UnityEngine;

public class C_SimpleVictoryProp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out C_PlayerMotor pyrMotor))
        {
            return;
        }
        GMTestGameplay.OnVictory?.Invoke();
    }
}
