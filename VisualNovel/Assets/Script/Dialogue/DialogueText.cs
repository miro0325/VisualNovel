using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DialogueText : MonoBehaviour
{
    public static DialogueText Instance { get; private set; }
    
    TextMeshProUGUI tempText;
    [SerializeField] float timeForChar;
    [SerializeField] float timeForChar_Fast;

    float charTime;

    float timer;

    string saves;

    bool isDialogEnd = false;
    bool isTypingEnd = false;

    int dialogNumber = 0;

    Coroutine coroutine = null;

    public bool IsTypingEnd()
    {
        return isTypingEnd;
    }


    public void Typing(string dialogs, TextMeshProUGUI text)
    {
        isDialogEnd = false;
        saves = dialogs;
        tempText = text;
        coroutine = StartCoroutine(Typer(dialogs.ToCharArray(), text));
        
    }

    IEnumerator Typer(char[] chars, TextMeshProUGUI text)
    {
        int currentChar = 0;
        charTime = timeForChar;
        int charLength = chars.Length;
        isTypingEnd = false;
        text.text = "";
        while(currentChar < charLength)
        {
            if(timer >= 0)
            {
                yield return null;
                timer-=Time.deltaTime;
            }else
            {
                text.text += chars[currentChar].ToString();
                currentChar++;
                timer = charTime;
            }
        }
        if(currentChar >= charLength)
        {
            isTypingEnd = true;
            dialogNumber++;
            coroutine = null;   
            yield break;
        }

    }

    public void EndTyping()
    {
        
        if (!isTypingEnd)
        {
            charTime = timeForChar_Fast;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndTyping();
        }
    }
}
