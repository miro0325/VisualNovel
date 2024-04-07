using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFX : MonoBehaviour
{
    AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init()
    {
        audioSource = GetComponent<AudioSource>();
        Destroy(this.gameObject, audioSource.clip.length + 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
