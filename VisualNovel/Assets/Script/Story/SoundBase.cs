using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundBase : MonoBehaviour
{
    AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MusicStart(AudioClip clip,bool fade=false)
    {
        if (audioSource == null) return;
        if(audioSource.isPlaying) audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
        if(fade)
        {
            audioSource.volume = 0;
            audioSource.DOFade(1, 1);
        }
    }

    public void MusicVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void MusicEnd(bool fade = false)
    {
        if (audioSource.isPlaying)
        {
            if(fade)
            {
                StartCoroutine(MusicEndFade());
            } else
            {
                audioSource.Stop();
            }
        }
    }

    IEnumerator MusicEndFade()
    {
        yield return audioSource.DOFade(0, 1).WaitForCompletion();
        audioSource.Stop();
    }
}
