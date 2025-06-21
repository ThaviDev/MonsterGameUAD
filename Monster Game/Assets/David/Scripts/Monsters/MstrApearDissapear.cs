using System;
using System.Collections;
using UnityEngine;

public class MstrApearDissapear : MonoBehaviour
{
    public static Action OnMonsterReady;
    [SerializeField] float _aliveTime;
    [SerializeField] float _fadingTime;

    [SerializeField] private SpriteRenderer _sprtRen;
    void Start()
    {
        StartCoroutine(Fading(false));
    }
    void Update()
    {
        _aliveTime -= Time.deltaTime;
        if (_aliveTime <= 0)
        {
            StartCoroutine(Fading(true));
            _aliveTime = 999; // evitar repeticiones
        }
    }

    public void SetAliveTime(float newAliveTime)
    {
        _aliveTime = newAliveTime;
    }

    IEnumerator Fading(bool isDisapear)
    {
        Color spriteColor = _sprtRen.color;
        float startAlpha = 0f; // Comienza transparente
        float targetAlpha = 1f; // Termina completamente visible
        if (isDisapear)
        {
            startAlpha = 1f; // Comienza visible
            targetAlpha = 0f; // Termina completamente transparente
        }

        float elapsedTime = 0f;

        // Configurar el alpha inicial
        spriteColor.a = startAlpha;
        _sprtRen.color = spriteColor;

        // Ejecutar el fade progresivo
        while (elapsedTime < _fadingTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / _fadingTime);
            spriteColor.a = newAlpha;
            _sprtRen.color = spriteColor;
            yield return null; // Esperar al siguiente frame
        }

        // Asegurarse de que el alpha sea exactamente 1 al final
        if (isDisapear) {
            Destroy(gameObject);
        } else
        {
            spriteColor.a = targetAlpha;
            _sprtRen.color = spriteColor;
            OnMonsterReady?.Invoke();
        }
    }
}
