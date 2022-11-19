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


  public List<Dictionary<string, Sound>> soundDicts = new List<Dictionary<string, Sound>>();


  // ================== Methods

  void Awake ()
  {
    Instance = this;

    foreach (Sound s in sounds) {soundDict.Add(s.name, s); }

    

    foreach (KeyValuePair<string, Sound> s in soundDict)
    {
      s.Value.source = gameObject.AddComponent<AudioSource>();
      s.Value.source.clip = s.Value.clip;

      s.Value.source.volume = s.Value.volume;
      s.Value.source.pitch = s.Value.pitch;
      s.Value.source.loop = s.Value.loop;
    }
    
    soundDicts.Add(soundDict);
    for(int i = 0; i<4; i++){
      soundDicts.Add(makePlayer());
    }
  }

  public Dictionary<string, Sound> makePlayer(){
    Dictionary<string, Sound> p = new Dictionary<string, Sound>();
    foreach (Sound s in charaSounds) { 
      Sound newS = new Sound();
      newS.name = s.name;
      newS.clip = Instantiate(s.clip);
      newS.volume = s.volume;
      newS.pitch = s.pitch;
      newS.loop = s.loop;
      p.Add(s.name, newS); }
    foreach (KeyValuePair<string, Sound> s in p)
    {
     
      s.Value.source = gameObject.AddComponent<AudioSource>();
      s.Value.source.clip = s.Value.clip;

      s.Value.source.volume = s.Value.volume;
      s.Value.source.pitch = s.Value.pitch;
      s.Value.source.loop = s.Value.loop;
    }
    return p;
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
    Debug.Log(name);
    Debug.Log(playID);
    Sound s = soundDicts[playID][name];

    s.source.Stop();
  }
}


