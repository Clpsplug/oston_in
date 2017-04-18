using UnityEngine;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

    public List<AudioEvent> audioEvents;
    List<AudioSource> sources = new List<AudioSource>();

    static AudioManager instance;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.spatialBlend = 0f;
            sources.Add(source);
        }
    }

    public static void PlaySoundEvent(string eventKey)
    {
        if (instance == null) return;
        instance.PlayEvent(eventKey);
    }

    void PlayEvent(string eventKey)
    {
        bool eventFound = false;
        foreach (AudioEvent ae in audioEvents)
        {
            if(ae.key == eventKey)
            {
                foreach (AudioClip ac in ae.samples)
                {
                    bool foundSource = false;
                    foreach (AudioSource source in sources)
                    {
                        if (source.isPlaying) continue;

                        source.clip = ac;
                        source.time = 0f;
                        source.volume = ae.volume;
                        source.pitch = ae.pitch + UnityEngine.Random.Range(-ae.pitchModulation, ae.pitchModulation);
                        source.Play();
                        foundSource = true;
                        break;
                    }

                    if (!foundSource)
                    {
                        AudioSource source = gameObject.AddComponent<AudioSource>();
                        source.spatialBlend = 0f;
                        sources.Add(source);

                        source.clip = ac;
                        source.time = 0f;
                        source.volume = ae.volume;
                        source.pitch = ae.pitch + UnityEngine.Random.Range(-ae.pitchModulation, ae.pitchModulation);
                        source.Play();

                    }
                }

                eventFound = true;
                break;
            }
        }
        if(!eventFound)
        {
            Debug.LogWarning("No audio event found for " + eventKey);
        }
        else {
          Debug.Log(String.Format("CUED {0}", eventKey));
        }

    }


    [System.Serializable]
    public class AudioEvent
    {
        public string key;
        public List<AudioClip> samples;
        public float volume = 1f;
        public float pitch = 1f;
        public float pitchModulation = 0f;
    }
}
