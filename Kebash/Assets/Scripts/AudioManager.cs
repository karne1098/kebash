using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public List<Sound> sounds;

    public Dictionary<string, Sound> soundDict = new Dictionary<string, Sound>();

    // Start is called before the first frame update
    void Awake ()
    {
        Instance = this;
        foreach (Sound s in sounds)
        {
            soundDict.Add(s.name, s);
        }

        foreach (KeyValuePair<string, Sound> s in soundDict)
        {
           s.Value.source = gameObject.AddComponent<AudioSource>();
           s.Value.source.clip = s.Value.clip;

           s.Value.source.volume = s.Value.volume;
           s.Value.source.pitch = s.Value.pitch;
            s.Value.source.loop = s.Value.loop;
        }
    }

    public void Play (string name)
    {
        Sound s = soundDict[name];
        s.source.Play();

    }
}
