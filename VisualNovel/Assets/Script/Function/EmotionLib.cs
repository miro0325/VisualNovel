using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EmoticonInfo
{
    public ParticleSystem particle;
    public EmotionType type = EmotionType.NONE;
}

public enum EmotionType
{
    NONE = -1, SWEAT = 0, HAPPY = 1, NOTE = 2, QUESTION = 3, POINT = 4
}

public class EmotionLib : MonoBehaviour
{
    public static EmotionLib Instance { get; private set; }
    
    public List<EmoticonInfo> list = new List<EmoticonInfo>();
    public Dictionary<EmotionType, ParticleSystem> dicEmotionPar = new Dictionary<EmotionType, ParticleSystem>();

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
        foreach (var item in list)
        {
            dicEmotionPar.Add(item.type, item.particle);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
