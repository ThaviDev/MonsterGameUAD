using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderController : MonoBehaviour
{
    [Header("Referencias a Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        // Inicializar sliders con valores guardados
        InitializeSliders();

        // Configurar listeners para cambios en los sliders
        SetupSliderListeners();
    }

    private void InitializeSliders()
    {
        // Convertir decibeles guardados a valores lineales (0-1)
        masterSlider.value = ConvertDecibelsToLinear(PlayerPrefs.GetFloat(AudioMixerManager.MasterVolumeKey, 0f));
        sfxSlider.value = ConvertDecibelsToLinear(PlayerPrefs.GetFloat(AudioMixerManager.SFXVolumeKey, 0f));
        musicSlider.value = ConvertDecibelsToLinear(PlayerPrefs.GetFloat(AudioMixerManager.MusicVolumeKey, 0f));
    }

    private void SetupSliderListeners()
    {
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    private float ConvertDecibelsToLinear(float dB)
    {
        // Convertir decibeles a valor lineal (0-1)
        return dB <= -80f ? 0f : Mathf.Pow(10, dB / 20f);
    }

    // Métodos públicos para cambiar volúmenes desde UI
    public void SetMasterVolume(float linearValue)
    {
        AudioMixerManager.Instance.SetMasterVolume(linearValue);
    }

    public void SetSFXVolume(float linearValue)
    {
        AudioMixerManager.Instance.SetSFXVolume(linearValue);
    }

    public void SetMusicVolume(float linearValue)
    {
        AudioMixerManager.Instance.SetMusicVolume(linearValue);
    }
}