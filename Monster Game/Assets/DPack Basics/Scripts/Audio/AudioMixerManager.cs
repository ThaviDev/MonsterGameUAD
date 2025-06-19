using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerManager : MonoBehaviour
{
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] string _parameterName_MasterVolume;
    [SerializeField] string _parameterName_SFX_Volume;
    [SerializeField] string _parameterName_MusicVolume;

    // Claves para PlayerPrefs
    public static string MasterVolumeKey => "MasterVolume";
    public static string SFXVolumeKey => "SFXVolume";
    public static string MusicVolumeKey => "MusicVolume";


    private static AudioMixerManager _instance;
    public static AudioMixerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // Buscar una instancia existente en la escena.
                _instance = FindAnyObjectByType<AudioMixerManager>();

                if (_instance == null)
                {
                    // Crear un nuevo GameObject con el script adjunto si no se encuentra ninguna instancia.
                    GameObject singletonObject = new GameObject("SoundMixerManager");
                    _instance = singletonObject.AddComponent<AudioMixerManager>();

                    // Opcional: Evitar que el objeto sea destruido al cambiar de escena.
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Evitar que el objeto sea destruido al cambiar de escena.
        }
        else if (_instance != this)
        {
            Destroy(gameObject); // Destruir instancias adicionales si ya existe una instancia.
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        LoadVolumeSettings();
    }
    private void LoadVolumeSettings()
    {
        float masterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 0);
        float sfxVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 0);
        float musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0);

        // Exposed Parameters
        _audioMixer.SetFloat(_parameterName_MasterVolume, masterVolume);
        _audioMixer.SetFloat(_parameterName_SFX_Volume, sfxVolume);
        _audioMixer.SetFloat(_parameterName_MusicVolume, musicVolume);
    }

    private float ConvertLinearToDecibels(float linearValue)
    {
        if (linearValue <= 0.0001f) // Evita log(0) = -infinito
            return -80f;            // Valor mínimo en Unity
        else
            return 20f * Mathf.Log10(linearValue);
    }

    public void SetMasterVolume(float linearVolume)
    {
        float volumeDB = ConvertLinearToDecibels(linearVolume);
        PlayerPrefs.SetFloat(MasterVolumeKey, volumeDB);
        PlayerPrefs.Save();
        _audioMixer.SetFloat(_parameterName_MasterVolume, volumeDB);
    }

    public void SetSFXVolume(float linearVolume)
    {
        float volumeDB = ConvertLinearToDecibels(linearVolume);
        PlayerPrefs.SetFloat(SFXVolumeKey, volumeDB);
        PlayerPrefs.Save();
        _audioMixer.SetFloat(_parameterName_SFX_Volume, volumeDB);
    }

    public void SetMusicVolume(float linearVolume)
    {
        float volumeDB = ConvertLinearToDecibels(linearVolume);
        PlayerPrefs.SetFloat(MusicVolumeKey, volumeDB);
        PlayerPrefs.Save();
        _audioMixer.SetFloat(_parameterName_MusicVolume, volumeDB);
    }
    /*
    public void SetMasterVolume(float _volume)
    {
        _audioMixer.SetFloat("MasterVolume", _volume);
    }
    public void SetSFXVolume(float _volume)
    {
        _audioMixer.SetFloat("SFXVolume", _volume);
    }
    public void SetMusicVolume(float _volume)
    {
        _audioMixer.SetFloat("MusicVolume", _volume);
    }*/
}
