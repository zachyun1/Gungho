using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnVolumeChange : MonoBehaviour {

    AudioSource[] comps;

    void Start()
    {
        comps = GameControl.control.GetComponents<AudioSource>();
    }

    public void UpdateVolume()
    {
        comps[1].volume = GetComponent<Slider>().value;
    }
}
