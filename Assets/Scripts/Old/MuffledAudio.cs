using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuffledAudio : MonoBehaviour
{
    private AudioLowPassFilter lPF;

    void Start()
    {
        lPF = GameObject.FindGameObjectWithTag("Game Music").GetComponent<AudioLowPassFilter>();
        lPF.cutoffFrequency = 700f;
        Debug.Log("here");
    }
}