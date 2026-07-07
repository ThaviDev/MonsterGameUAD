using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MusicManager : MonoBehaviour
{
    private static MusicManager _instance;
    public static MusicManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // Buscar una instancia existente en la escena.
                _instance = FindAnyObjectByType<MusicManager>();

                if (_instance == null)
                {
                    // Crear un nuevo GameObject con el script adjunto si no se encuentra ninguna instancia.
                    GameObject singletonObject = new GameObject("Music Manager");
                    _instance = singletonObject.AddComponent<MusicManager>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    // Music
    [SerializeField] AudioClip m_AmbientMusic;
    [SerializeField] AudioClip m_HordeMusic;
    [SerializeField] AudioClip m_ChaseMusic;
    [SerializeField] AudioClip m_MainMenu;

    // Sound Cues
    [SerializeField] AudioClip m_MonsterReveal;
    [SerializeField] AudioClip m_MonsterNearCue;
    [SerializeField] AudioClip m_Screamer;
    [SerializeField] int m_lastMusicID = 0;
    AudioSource _source;

    void Awake()
    {
        _source = GetComponent<AudioSource>();
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        SetMusic(0);
    }
    public void PlaySoundCue(int musicID)
    {
        StartCoroutine(PlaySoundCueCoroutine(musicID));
    }
    IEnumerator PlaySoundCueCoroutine(int musicID)
    {
        SoundCuePlay(musicID);
        yield return new WaitForSeconds(_source.clip.length);
        PlayMusic();
    }
    public void SetMusic(int musicID)
    {
        m_lastMusicID = musicID;
        PlayMusic();
    }
    public void PlayMusic()
    {
        if (_source == null)
        {
            Debug.LogWarning("AudioSource is null. Please ensure that the MusicManager has an AudioSource component attached.");
            return;
        }
        if (_source.clip != null)
        {
            _source.Stop();
        }
        switch (m_lastMusicID)
        {
            case 0:
                _source.clip = m_AmbientMusic;
                break;
            case 1:
                _source.clip = m_HordeMusic;
                break;
            case 2:
                _source.clip = m_ChaseMusic;
                break;
            case 3:
                _source.clip = m_MainMenu;
                break;
        }
        _source.Play();
    }
    void SoundCuePlay(int soundID)
    {
        if (_source.clip != null)
        {
            _source.Stop();
        }
        switch (soundID)
        {
            case 0:
                _source.clip = m_MonsterReveal;
                break;
            case 1:
                _source.clip = m_MonsterNearCue;
                break;
            case 2:
                _source.clip = m_Screamer;
                break;
        }
        _source.Play();
    }
}
