using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioClip _ambientMusic;
    [SerializeField] AudioClip _hordeMusic;
    AudioSource _source;
    void Start()
    {
        _source = GetComponent<AudioSource>();
        SetMusic(0);
    }
    public void SetMusic(int musicID)
    {
        if (_source.clip != null) {
            _source.Stop();
        }
        switch (musicID) { 
            case 0:
                _source.clip = _ambientMusic;
                break;
            case 1:
                _source.clip = _hordeMusic;
                break;
        }
        _source.Play();
    }
}
