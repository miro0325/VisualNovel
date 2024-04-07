using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public enum TYPE
{
    NONE = 0,SCREEN = 1, CHARACTER = 2, IMAGE = 3,TEXT=4,BG=5,MUSIC=6
}

public enum MOTION
{
    NONE=0,BOUNCE = 1,FEAR =2, PICK =3,MOVE=4,FACE=5,FLIP=6,GREY=7
}

public enum EFFECT
{
    NONE=0,FADEON=1,FADEOFF=2,EMOTION=3
}

public enum BG
{
    NONE=0,FADEON=1,FADEOFF=2,CHANGE=3
}

public enum SCREEN
{
    NONE=0,FADEON=1,FADEOFF=2,BLINK=3
}

public enum MUSIC
{
    NONE = 0, START=1, VOLUME=2, END=3, SFX=4
}

[System.Serializable]
public class CharacterInfo
{
    public CharacterBase character;
    public string key;
}

[System.Serializable]
public class SoundInfo
{
    public AudioClip audio;
    public string key;
}

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance { get; private set; }

    [SerializeField] CharacterInfo[] characterInfos;
    [SerializeField] SoundInfo[] soundInfos;

    [SerializeField] Text nameText;
    [SerializeField] Text circleText;

    [SerializeField] TextMeshProUGUI typingText;

    [SerializeField] GameObject textPanel;

    [SerializeField] BackGround bk;

    [SerializeField] SoundBase audioClip;

    [SerializeField] AudioSource sfx;

    private string filePath;

    int curIndex = 0;
    int curNumber = 0;

    TYPE prevType = TYPE.NONE;
    string prevStr = "NONE";

    bool isDelay = false;
    bool isTextOutput = false;


    List<Dictionary<string, object>> curStory;

    public Dictionary<string, CharacterBase> DicCharacters = new Dictionary<string, CharacterBase>();
    public Dictionary<string, AudioClip> DicAudios = new Dictionary<string, AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);
        //curStory = SheetImporter.Instance.GetData();
        StartCoroutine(WaitLoadData());
        
    }

    private IEnumerator WaitLoadData()
    {
        yield return new WaitUntil(() => SheetImporter.Instance.IsLoad);
        curStory = SheetImporter.Instance.GetData();
        yield return new WaitForSeconds(1f);
        Initialize();
        NextScreen();
        Debug.Log("start");
    }

    private void Initialize()
    {
        for(int i = 0; i < characterInfos.Length; i++)
        {
            DicCharacters.Add(characterInfos[i].key, characterInfos[i].character);
        }
        for (int i = 0; i < soundInfos.Length; i++)
        {
            DicAudios.Add(soundInfos[i].key, soundInfos[i].audio);
        }
    }

    private TYPE ParseType(string value)
    {
        TYPE type = (TYPE)Enum.Parse(typeof(TYPE), value);
        return type;
    }

    private MOTION ParseMotion(string value)
    {
        string txt = value;

        if (value.Contains("/")) txt = value.Split('/')[0]; 

        MOTION motion = (MOTION)Enum.Parse(typeof(MOTION), txt);
        return motion;
    }

    private EFFECT ParseEffect(string value)
    {
        string txt = value;

        if (value.Contains("/")) txt = value.Split('/')[0];
        EFFECT effect = (EFFECT)Enum.Parse(typeof(EFFECT), txt);
        return effect;
    }

    private BG ParseBG(string value)
    {
        string txt = value;

        if (value.Contains("/")) txt = value.Split('/')[0];
        BG bg = (BG)Enum.Parse(typeof(BG), txt);
        return bg;
    }

    private SCREEN ParseScreen(string value)
    {
        string txt = value;

        if (value.Contains("/")) txt = value.Split('/')[0];
        SCREEN screen = (SCREEN)Enum.Parse(typeof(SCREEN), txt);
        return screen;
    }

    private MUSIC ParseMusic(string value)
    {
        string txt = value;

        if (value.Contains("/")) txt = value.Split('/')[0];
        MUSIC music = (MUSIC)Enum.Parse(typeof(MUSIC), txt);
        return music;
    }


    // Update is called once per frame
    void Update()
    {
        EndTyping();
    }

    void EndTyping()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (curIndex >= curStory.Count) return;
            TYPE t = (TYPE)ParseType(curStory[curIndex]["TYPE"].ToString());
            if (t.Equals(TYPE.TEXT) && !DialogueText.Instance.IsTypingEnd())
            {
                DialogueText.Instance.EndTyping();
            }
        }
    }

    void NextScreen()
    {
        if (curStory == null) return;
        StartCoroutine(NextScreenPlay());
    }

    IEnumerator NextScreenPlay()
    {
        for(int i = 0; i < curStory.Count; i++)
        {
            CheckInput();
            curNumber = int.Parse(curStory[curIndex]["NUMBER"].ToString());
            prevType = (TYPE)ParseType(curStory[curIndex]["TYPE"].ToString());
            prevStr = curStory[curIndex]["TARGET"].ToString();
            curIndex++;
            yield return StartCoroutine(WaitScreen());
        }
            //while (curNumber < int.Parse(curStory[curIndex]["NUMBER"].ToString()))
            //{
            //    //Debug.Log(int.Parse(curStory[curIndex]["NUMBER"].ToString()));
                


            //}
        
        
        
    }

    IEnumerator ScreenStart()
    {
        TYPE t = (TYPE)curStory[curIndex]["TYPE"];
        yield return null;
    }

    void CheckInput()
    {
        TYPE t = (TYPE)ParseType(curStory[curIndex]["TYPE"].ToString());
        //Debug.Log(t);
        MOTION m;
        EFFECT e;
        switch(t)
        {
            case TYPE.NONE:
                return;
            case TYPE.SCREEN:
                break;
            case TYPE.CHARACTER:
                PlayCharacter();
                break;
            case TYPE.IMAGE:
                break;
            case TYPE.TEXT:
                PlayText();
                break;
            case TYPE.BG:
                PlayBG();
                break;
            case TYPE.MUSIC:
                PlayMusic();
                break;
            
        }

    }

    void PlayMusic()
    {
        if (curStory == null) return;
        string _m = curStory[curIndex]["MOTION"].ToString();
        string[] m_Index = null;
        string[] e_Index = null;
        string _e = curStory[curIndex]["EFFECT"].ToString();
        Debug.Log(_m);
        if (_m.Contains("/"))
        {
            m_Index = _m.Split('/');
            _m = _m.Split('/')[0];
        }
        if (_e.Contains("/"))
        {
            e_Index = _e.Split('/');
            _e = _e.Split('/')[0];
        }
        var target = curStory[curIndex]["TARGET"].ToString();
        if(_e == "START")
        {
            if (DicAudios.ContainsKey(target))
            {
                
                if(e_Index != null && e_Index.Length > 1 && bool.Parse(e_Index[1]))
                {
                    audioClip.MusicStart(DicAudios[target], bool.Parse(e_Index[1]));

                } else
                {
                    audioClip.MusicStart(DicAudios[target]);
                }
            }
        }
        else if (_e == "END")
        {
            if (e_Index != null && e_Index.Length > 1 && bool.Parse(e_Index[1]))
            {
                audioClip.MusicEnd(bool.Parse(e_Index[1]));

            }
            else
            {
                audioClip.MusicEnd();
            }
            
        }
        else if (_e == "VOLUME")
        {
            if (e_Index.Length > 1)
            {
                audioClip.MusicVolume(float.Parse(e_Index[1]));
            }
        }
        else if(_e == "SFX")
        {
            if (DicAudios.ContainsKey(target))
            {
                var sfx = Instantiate(this.sfx, Vector2.zero,Quaternion.identity);
                sfx.clip = DicAudios[target];
                sfx.GetComponent<SoundFX>().Init();
            }
        }
    }

    void PlayBG()
    {
        if (curStory == null) return;
        string _m = curStory[curIndex]["MOTION"].ToString();
        string[] m_Index = null;
        string[] e_Index = null;
        string _e = curStory[curIndex]["EFFECT"].ToString();
        Debug.Log(_m);
        if (_m.Contains("/"))
        {
            m_Index = _m.Split('/');
            _m = _m.Split('/')[0];
        }
        if (_e.Contains("/"))
        {
            e_Index = _e.Split('/');
            _e = _e.Split('/')[0];
        }
        var target = curStory[curIndex]["TARGET"].ToString();
        Debug.Log(target);
        if (target == "NONE")
        {
            BG bg = (BG)ParseBG(_e);
            if(bg.Equals(BG.FADEON))
            {
                bk.FadeOn(float.Parse(e_Index[1]));
            } else if(bg.Equals(BG.FADEOFF))
            {
                bk.FadeOff(float.Parse(e_Index[1]));
            }
        }
        else
        {
            var newBG = Resources.Load<Sprite>("Background/" + target);
            Debug.Log(newBG);
            if (newBG != null)
            {
                BG bg = (BG)ParseBG(_e);
                if(bg.Equals(BG.CHANGE))
                {
                    bk.ChangeBG(newBG);
                }
            }
        }
    }

    void PlayText()
    {
        if(curStory == null) return;
        var target = curStory[curIndex]["TARGET"].ToString();
        if (target == "NONE") textPanel.SetActive(false);
        else
        {
            CharacterBase c = DicCharacters[target];
            if(c == null || curStory[curIndex]["TEXT"].ToString() == "") return;
            textPanel.SetActive(true);
            nameText.text = c.Name;
            circleText.text = c.Nickname;
            Debug.Log(curStory[curIndex]["TEXT"].ToString());
            DialogueText.Instance.Typing(curStory[curIndex]["TEXT"].ToString(), typingText);
        }
    }

    void PlayCharacter()
    {
        string _m = curStory[curIndex]["MOTION"].ToString();
        string[] m_Index = null;
        string[] e_Index = null;
        string _e = curStory[curIndex]["EFFECT"].ToString();
        //Debug.Log(_m);
        if (_m.Contains("/"))
        {
            m_Index = _m.Split('/');
            _m = _m.Split('/')[0];
        }
        if(_e.Contains("/"))
        {
            e_Index = _e.Split('/');
            _e = _e.Split('/')[0];
        }
        
        MOTION m = (MOTION)ParseMotion(_m);
        EFFECT e = (EFFECT)ParseEffect(_e);
        string id = curStory[curIndex]["TARGET"].ToString();
        if(!m.Equals(null)) PlayMotion(id,m,m_Index);
        if (!e.Equals(null)) PlayEffect(id, e, e_Index);

    }

    void PlayMotion(string key, MOTION m, string[] index = null)
    {
        CharacterBase character = DicCharacters[key];
        
        switch(m)
        {
            case MOTION.NONE:
                return;
            case MOTION.PICK:
                character.Pick(float.Parse(index[1]));
                return;
            case MOTION.MOVE:
                Debug.Log("move");
                character.Move(float.Parse(index[2]), new Vector2(float.Parse(index[1].Split(',')[0]), float.Parse(index[1].Split(',')[1])));
                return;
            case MOTION.FACE:
                if(index.Length >= 3 && index[2]=="true")
                    character.Face(int.Parse(index[1]), bool.Parse(index[2]));
                else
                    character.Face(int.Parse(index[1]));
                return;
            case MOTION.BOUNCE:
                character.Bounce(float.Parse(index[1]));
                return;
            case MOTION.FEAR:
                character.Fear(float.Parse(index[1]));
                return;
            case MOTION.FLIP:
                character.Flip(int.Parse(index[1]));
                return;
        }
    }

    void PlayEffect(string key, EFFECT e, string[] index = null)
    {
        CharacterBase character = DicCharacters[key];
        switch(e)
        {
            case EFFECT.NONE:
                return;
            case EFFECT.FADEON:
                character.FadeOn(int.Parse(index[1]));
                return;
            case EFFECT.FADEOFF:
                character.FadeOff(int.Parse(index[1]));
                return;
            case EFFECT.EMOTION:
                character.Emotion(index[1]);
                return;
        }
    }

    

    IEnumerator WaitScreen()
    {
        if(curIndex >= curStory.Count) yield break;
        isDelay = true;
        Debug.Log("delay");
        if(prevType == TYPE.TEXT && prevStr != "NONE" && curStory.Count >= curIndex)
        {
            //Debug.Log("s");
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) && DialogueText.Instance.IsTypingEnd());
        } else
        {
            //Debug.Log(curStory[curIndex]["DELAY"].ToString());
            yield return new WaitForSeconds(float.Parse(curStory[curIndex]["DELAY"].ToString()));
            if (prevType == TYPE.BG) Debug.Log("Yes");
        }
        isDelay = false;
    }

    
}
