using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    private void Awake() => instance = this;
  

    public void playSound(string sound)
    {
        string searchPath = "event:/" + sound;
        RuntimeManager.PlayOneShot(searchPath);
    }

}
