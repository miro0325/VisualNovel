using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;


public class SheetImporter : MonoBehaviour
{
    public static SheetImporter Instance { get; private set; }

    public bool IsLoad => isLoad;

    private readonly string SHEET_ADDRESS = "https://docs.google.com/spreadsheets/d/1-NP8GVR_9VYLyoQrqjREOahXRpfndTx5G3DEfUBNDug/export?format=tsv&range=A1:G";

    private bool isLoad = false;

    private List<Dictionary<string, object>> sheetData = new();

    public List<Dictionary<string, object>> GetData()
    {
        if (!isLoad || sheetData.Count == 0)
        {
            Debug.LogError("Data is loading or not loaded");
            return null;
        }
        else
        {
            return sheetData;
        }
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
        StartCoroutine(Import());
    }

    private IEnumerator Import()
    {
        using(UnityWebRequest www = UnityWebRequest.Get(SHEET_ADDRESS))
        {
            yield return www.SendWebRequest();
            if(www.isDone)
            {
                ImportData(www.downloadHandler.text);
                
            }
                
            
        }
        
    }
    
    private void ImportData(string data)
    {
        if (string.IsNullOrEmpty(data)) return;
        string[] lines = data.Trim().Split('\n');

        string[] headers = lines[0].Trim().Split('\t');
        for(int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Trim().Split('\t');
            var datas = new Dictionary<string, object>(values.Length);
            for(int j = 0; j < values.Length; j++)
            {
                string value = values[j].Trim();
                if (string.IsNullOrEmpty(value)) continue;
                value = value.TrimStart().TrimEnd();
                object final = value;
                Debug.Log(final);
                int n;
                float f;

                if (int.TryParse(value, out n))
                {
                    final = n;
                }
                else if (float.TryParse(value, out f))
                {
                    final = f;
                }
                Debug.Log(headers[j]);
                datas[headers[j]] = final;
            }
            sheetData.Add(datas);
        }

        isLoad = true;
    }
    
}
