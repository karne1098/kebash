using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class AudioManager : MonoBehaviour
{
  public static AudioManager Instance;

  private      Dictionary<string, Sound>  _generalSoundDict = new      Dictionary<string, Sound> ();
  private List<Dictionary<string, Sound>> _playerSoundDicts = new List<Dictionary<string, Sound>>();

  // ================== Accessors
  
  public List<Sound> sounds;

  public List<Sound> charaSounds;

  // ================== Methods

  void Awake ()
  {
    Instance = this;

    foreach (Sound s in sounds)
    { 
      _generalSoundDict.Add(s.name, s);
    }

    foreach (KeyValuePair<string, Sound> s in _generalSoundDict)
    {
      s.Value.source        = gameObject.AddComponent<AudioSource>();
      s.Value.source.clip   = s.Value.clip;
      s.Value.source.volume = s.Value.volume;
      s.Value.source.pitch  = s.Value.pitch;
      s.Value.source.loop   = s.Value.loop;
    }
    
    for (int i = 0; i < MultiplayerManager.Instance.MaxPlayerCount; ++i)
    {
      _playerSoundDicts.Add(makePlayerSoundDict());
    }
  }
  
  public void Play(string name, int playerIndex = -1)
  {
    Sound s = getSound(name, playerIndex);
    if (!s.source.isPlaying) { s.source.Play(); }
  }

  public void Pause(string name, int playerIndex = -1)
  {
    Sound s = getSound(name, playerIndex);
    s.source.Pause();
  }

  public void Stop(string name, int playerIndex = -1)
  {
    Sound s = getSound(name, playerIndex);
    s.source.Stop();
  }

  // ================== Helpers

  private Sound getSound(string name, int playerIndex)
  {
    return playerIndex < 0 ? _generalSoundDict[name] : _playerSoundDicts[playerIndex][name];
  }

  private Dictionary<string, Sound> makePlayerSoundDict()
  {
    Dictionary<string, Sound> p = new Dictionary<string, Sound>();
    foreach (Sound s in charaSounds)
    { 
      Sound newS  = new Sound();
      newS.name   = s.name;
      newS.clip   = Instantiate(s.clip);
      newS.volume = s.volume;
      newS.pitch  = s.pitch;
      newS.loop   = s.loop;
      p.Add(s.name, newS);
    }

    foreach (KeyValuePair<string, Sound> s in p)
    {
      s.Value.source = gameObject.AddComponent<AudioSource>();
      s.Value.source.clip = s.Value.clip;

      s.Value.source.volume = s.Value.volume;
      s.Value.source.pitch  = s.Value.pitch;
      s.Value.source.loop   = s.Value.loop;
    }

    return p;
  }
}


