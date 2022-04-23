using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    FMOD.Studio.Bus bus;

    [SerializeField]
    [Range(0f, 1f)]
    private float volume = .5f;

    void Start()
    {
        bus = FMODUnity.RuntimeManager.GetBus("bus:/Music");
    }

    void Update()
    {
        bus.setVolume(volume);
    }
}
