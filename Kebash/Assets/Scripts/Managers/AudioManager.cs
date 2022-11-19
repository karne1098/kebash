using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class AudioManager : MonoBehaviour
{
  public static AudioManager Instance;

  // ================== Accessors
  
  public List<Sound> sounds;

  public List<Sound> charaSounds;


  public Dictionary<string, Sound> soundDict = new Dictionary<string, Sound>();
   
  public Dictionary<string, Sound> p = new Dictionary<string, Sound>();

  public Dictionary<string, Sound>[] soundDicts = new Dictionary<string, Sound>[5];


  // ================== Methods

  void Awake ()
  {
    Instance = this;

    foreach (Sound s in sounds)
    {
      soundDict.Add(s.name, s);
    }

    foreach (Sound s in charaSounds)
    {
      p.Add(s.name, s);
    }
    foreach (KeyValuePair<string, Sound> s in soundDict)
      {
        s.Value.source = gameObject.AddComponent<AudioSource>();
        s.Value.source.clip = s.Value.clip;

        s.Value.source.volume = s.Value.volume;
        s.Value.source.pitch = s.Value.pitch;
        s.Value.source.loop = s.Value.loop;
      }
    foreach (KeyValuePair<string, Sound> s in p)
      {
        s.Value.source = gameObject.AddComponent<AudioSource>();
        s.Value.source.clip = s.Value.clip;

        s.Value.source.volume = s.Value.volume;
        s.Value.source.pitch = s.Value.pitch;
        s.Value.source.loop = s.Value.loop;
      }
    soundDicts[0] = soundDict;
    for(int i = 1; i<soundDicts.Length; i++){
      soundDicts[i] = p;
    }
  
  }

    public void Play(string name, int playID)
    {
        Sound s = soundDicts[playID][name];
        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
    }
    public void Pause(string name, int playID)
  {
    Sound s = soundDicts[playID][name];
    s.source.Pause();
  }

  public void Stop(string name, int playID)
  {
    Sound s = soundDicts[playID][name];
    s.source.Stop();
  }
}


