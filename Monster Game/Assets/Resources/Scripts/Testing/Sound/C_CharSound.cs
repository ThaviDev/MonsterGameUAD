using System;
using System.Collections.Generic;
using UnityEngine;

public class C_CharSound : MonoBehaviour
{
    public Action<String> OnPlaySoundEvent;
    public Action<String> OnStopSoundEvent;
    [System.Serializable]
    public class SoundEntry
    {
        public string m_Name;
        public AudioClip m_Clip;
        public bool m_IsLoop = false;
    }

    [SerializeField]private List<SoundEntry> m_Entries = new List<SoundEntry>();
    private Dictionary<string, SoundEntry> m_Sounds = new Dictionary<string, SoundEntry>();
    [SerializeField]private AudioSource[] m_AudioSources;
    // Para checar que sonido esta sonando en cada audio source
    private Dictionary<AudioSource, string>  m_CurrentSounds = new Dictionary<AudioSource, string>();
    private void Awake()
    {
        m_Sounds.Clear();
        foreach (var entry in m_Entries)
        {
            if (!string.IsNullOrEmpty(entry.m_Name) && entry.m_Clip != null)
            {
                m_Sounds[entry.m_Name] = entry;
            }
        }
        if (m_AudioSources == null || m_AudioSources.Length == 0)
        {
            m_AudioSources = GetComponents<AudioSource>();
        }

        // Inicializar el registro de sonidos actuales
        m_CurrentSounds.Clear();
        foreach (var src in m_AudioSources)
        {
            m_CurrentSounds[src] = null; // Ningún sonido asignado al inicio
        }
    }
    void Start()
    {
        OnPlaySoundEvent += PlaySound;
        OnStopSoundEvent += StopSound;
    }
    private void PlaySound(string soundName)
    {
        if (string.IsNullOrEmpty(soundName))
        {
            Debug.LogWarning("Nombre de sonido vacío.");
            return;
        }
        // Buscar el clip en el diccionario
        if (!m_Sounds.TryGetValue(soundName, out SoundEntry entry) || entry == null)
        {
            Debug.LogWarning($"Sonido '{soundName}' no encontrado o su clip es nulo.");
            return;
        }
        if (m_AudioSources == null || m_AudioSources.Length == 0)
        {
            Debug.LogWarning("No hay AudioSources asignados.");
            return;
        }

        AudioSource availableSource = null;
        foreach (var src in m_AudioSources)
        {
            if (!src.isPlaying)
            {
                availableSource = src;
                break;
            }
        }
        if (availableSource == null)
        {
            Debug.LogWarning("No hay AudioSource disponible para reproducir el sonido.");
            return;
        }
        availableSource.clip = entry.m_Clip;
        availableSource.loop = entry.m_IsLoop;
        availableSource.Play();
        m_CurrentSounds[availableSource] = soundName;
    }

    private void StopSound(string soundName)
    {
        if (string.IsNullOrEmpty(soundName))
        {
            Debug.LogWarning("Nombre de sonido vacío para detener.");
            return;
        }

        bool stoppedAny = false;
        foreach (var src in m_AudioSources)
        {
            // Si esta fuente está reproduciendo el sonido que queremos detener
            if (m_CurrentSounds.TryGetValue(src, out string current) && current == soundName)
            {
                if (src.isPlaying)
                {
                    src.Stop();
                    stoppedAny = true;
                    Debug.Log($"Detenido '{soundName}' en fuente {Array.IndexOf(m_AudioSources, src)}");
                }
                // Limpiar el registro (incluso si ya no estaba reproduciendo)
                m_CurrentSounds[src] = null;
            }
        }

        if (!stoppedAny)
        {
            Debug.LogWarning($"No se encontró ninguna fuente reproduciendo '{soundName}'.");
        }
    }
    private void Update()
    {
        // Cada cierto tiempo, limpiar las entradas de fuentes que ya no están reproduciendo
        // (así mantenemos el registro sincronizado)
        foreach (var src in m_AudioSources)
        {
            if (!src.isPlaying && m_CurrentSounds.ContainsKey(src) && m_CurrentSounds[src] != null)
            {
                // El sonido ha terminado (o fue detenido externamente)
                m_CurrentSounds[src] = null;
            }
        }
    }
    private void OnDestroy()
    {
        OnPlaySoundEvent -= PlaySound;
        OnStopSoundEvent -= StopSound;
    }
}
