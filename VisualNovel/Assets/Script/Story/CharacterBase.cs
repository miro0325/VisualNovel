using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CharacterBase : MonoBehaviour
{
    public string id;
    public string Name;
    public string Nickname;

    [SerializeField] Transform emotionPos;

    [SerializeField] SpriteRenderer[] faces;
    [SerializeField] SpriteRenderer halo;
    [SerializeField] SpriteRenderer weapon;
    [SerializeField] SpriteRenderer body;


    int curIndex = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Bounce(float power)
    {
        StartCoroutine(MotionBounce(power));

    }

    IEnumerator MotionBounce(float power)
    {
        float originY = transform.position.y;
        yield return transform.DOMoveY(transform.position.y + power, 0.2f).WaitForCompletion();
        yield return transform.DOMoveY(originY, 0.2f).WaitForCompletion();
        yield return transform.DOMoveY(transform.position.y + power * 0.6f, 0.2f).WaitForCompletion();
        yield return transform.DOMoveY(originY, 0.2f).WaitForCompletion();
    }

    public void Fear(float power)
    {
        StartCoroutine(MotionFear(power));
    }

    IEnumerator MotionFear(float power)
    {
        int flipX = 1;
        float originX = transform.position.x;
        for (int i = 0; i < 10; i++)
        {
            yield return transform.DOMoveX(originX + (power * 0.3f * flipX), 0.05f).WaitForCompletion();
            flipX *= -1;

        }
        yield return transform.DOMoveX(originX, 0.05f).WaitForCompletion();
    }

    public void Pick(float power)
    {
        StartCoroutine(MotionPick(power));
        
    }

    IEnumerator MotionPick(float power)
    {
        float originY = transform.position.y;
        //Debug.Log(power);
        yield return transform.DOMoveY(transform.position.y - power, 0.2f).WaitForCompletion();
        yield return transform.DOMoveY(originY, 0.2f).WaitForCompletion();
    }

    public void Flip(int flipX)
    {
        gameObject.transform.localScale = new Vector3(flipX, 1, 1);
    }

    public void Face(int index, bool isAlpha = false)
    {
        if (index >= faces.Length)
        {
            Debug.LogError("Out of Index Size in Character Face");
            return;
        }
        if(isAlpha)faces[index].DOColor(new Color(1, 1, 1), 0);
        faces[curIndex].gameObject.SetActive(false);
        faces[index].gameObject.SetActive(true);
        curIndex = index; 


    }

    public void Move(float time, Vector2 pos)
    {
        transform.DOMove(pos, time).SetEase(Ease.OutSine);
    }

    public void Grey(bool isGrey)
    {
        if(isGrey)
        {
            foreach (var face in faces)
            {
                face.DOColor(new Color(0.5f, 0.5f, 0.5f), 0);
            }
            if (halo != null)
                halo.DOColor(new Color(0.5f, 0.5f, 0.5f), 0);
            if (body != null)
                body.DOColor(new Color(0.5f, 0.5f, 0.5f), 0);
        }
        else
        {
            foreach (var face in faces)
            {
                face.DOColor(new Color(1, 1, 1), 0);
            }
            if (halo != null)
                halo.DOColor(new Color(1, 1, 1), 0);
            if (body != null)
                body.DOColor(new Color(1, 1, 1), 0);
        }
    }


    public void FadeOn(float time)
    {
        //faces[curIndex].DOColor(new Color(1, 1, 1), time);
        foreach (var face in faces)
        {
            face.DOColor(new Color(1, 1, 1), time);
        }
        if (halo !=null)
            halo.DOColor(new Color(1, 1, 1), time);
        if (body != null)
            body.DOColor(new Color(1, 1, 1), time);
    }

    public void FadeOff(float time)
    {
        //faces[curIndex].DOColor(new Color(0, 0, 0), time);
        foreach(var face in faces)
        {
            face.DOColor(new Color(0, 0, 0), time); 
        }
        if (halo != null)
            halo.DOColor(new Color(0,0,0), time);
        if (body != null)
            body.DOColor(new Color(0,0,0), time);
    }

    public void Emotion(string key)
    {
        EmotionType e = ParseEmotionType(key);
        var p = Instantiate(EmotionLib.Instance.dicEmotionPar[e], emotionPos.position, Quaternion.identity);

    }

    EmotionType ParseEmotionType(string key)
    {
        switch(key)
        {
            case "SWEAT":
                return EmotionType.SWEAT;
            case "HAPPY":
                return EmotionType.HAPPY;
            case "QUESTION":
                return EmotionType.QUESTION;
            case "NOTE":
                return EmotionType.NOTE;
            case "POINT":
                return EmotionType.POINT;
            default : return EmotionType.NOTE;
        }
    }
}
