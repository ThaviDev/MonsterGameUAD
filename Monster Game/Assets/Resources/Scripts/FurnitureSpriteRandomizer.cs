using UnityEngine;

public class FurnitureSpriteRandomizer : MonoBehaviour
{
    [SerializeField] Sprite[] _posibleSprites;
    [SerializeField] SpriteRenderer _renderer;
    void Start()
    {
        _renderer.sprite = _posibleSprites[Random.Range(0,_posibleSprites.Length)];
    }
}
