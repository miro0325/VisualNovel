using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BackGround : MonoBehaviour
{
    private Image bg;
    [SerializeField] private Image fade;
    
    // Start is called before the first frame update
    void Start()
    {
        bg = GetComponent<Image>();   
    }

    public void FadeOn(float time)
    {
        fade.DOFade(1, time);
        
    }

    public void FadeOff(float time)
    {
        fade.DOFade(0, time);

    }

    public void ChangeBG(Sprite spr)
    {
        bg.sprite = spr;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
