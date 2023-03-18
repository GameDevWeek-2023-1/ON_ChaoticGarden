using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourceMusic;

    private bool _musicFadeOut;
    private void Update()
    {
        if (!_musicFadeOut) return;

        audioSourceMusic.volume -= Time.deltaTime;
    }
    public void MusicFadeOut()
    {
        _musicFadeOut = true;
    }
}
