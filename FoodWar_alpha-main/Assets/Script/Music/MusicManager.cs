using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public AudioMixer musicM;

    public static MusicManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


}
