using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    [SerializeField]
    private HMDInfo info;

    [SerializeField]
    private GazeImage gazeImage;

    //[SerializeField]
    private string subjectName;

    private Vector2[] uvData;

    private List<RecordData> recordData;
    private List<RecordData> allRecordData;
    private List<EmoOrNotData> emoOrNotData;

    private float totalTime = 0.0f;
    private float elapsedTime = 0.0f;

    private bool recordEnable = false;

    public bool RecordEnable
    {
        set => recordEnable = value;
    }

    private int displayCount = 0;

    private SubjectData subjectData;

    void Start()
    {
        recordData = new List<RecordData>();
        allRecordData = new List<RecordData>();
        emoOrNotData = new List<EmoOrNotData>();
        subjectData = new SubjectData();
        
        subjectData.Name = File.ReadAllText(FileUtils.SubjectNameFilePath);
        subjectData.ExperimentDate = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");

        var directoryPath = FileUtils.LogFolderPath(subjectData);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
            Debug.Log("フォルダを作成しました: " + directoryPath);
        }
        else
        {
            Debug.Log("このフォルダは既に存在しています: " + directoryPath);
        }
    }

    void Update()
    {
        if (recordEnable)
        {
            RecordData data = new RecordData();
            data.TotalTime = totalTime.ToString("0.0000");
            data.ElapsedTime = elapsedTime.ToString("0.0000");
            data.HitPointX = info.UVCoordinate.x.ToString("0.0000");
            data.HitPointY = info.UVCoordinate.y.ToString("0.0000");

            recordData.Add(data);
            allRecordData.Add(data);

            elapsedTime += Time.deltaTime;
        }

        totalTime += Time.deltaTime;
    }

    private void GenerateCSV<T>(List<T> items, string fileName)
    {
        StringBuilder csvContent = new StringBuilder();

        var properties = typeof(T).GetProperties();
        for (int i = 0; i < properties.Length; i++)
        {
            csvContent.Append(properties[i].Name);
            if (i < properties.Length - 1)
            {
                csvContent.Append(",");
            }
        }
        csvContent.AppendLine();

        foreach (var item in items)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                var value = properties[i].GetValue(item, null);
                csvContent.Append(value);

                if (i < properties.Length - 1)
                {
                    csvContent.Append(",");
                }
            }
            csvContent.AppendLine();
        }

        File.WriteAllText(fileName, csvContent.ToString());

        recordData = new List<RecordData>();
    }

    private void OutputCSV(string imageName)
    {
        var fileName = FileUtils.LogFolderPath(subjectData) + $"\\{displayCount}_{imageName}_record.csv";
        GenerateCSV(recordData, fileName);
        
        Debug.Log(fileName + "is recorded.");
    }

    public void ResetRecordData()
    {
        OutputCSV(gazeImage.TextureName);
        recordData = new List<RecordData>();
        elapsedTime = 0f;
        Debug.Log("RecordData is initialized.");
    }

    public void RecordEmoOtNotData(string determineEmo)
    {
        EmoOrNotData data = new EmoOrNotData();
        data.Count = displayCount;
        data.EmoOrNot = determineEmo;
        displayCount++;

        emoOrNotData.Add(data);
    }

    private void OnDisable()
    {
        GenerateCSV(allRecordData, FileUtils.LogFolderPath(subjectData) + "\\all_records.csv");
        GenerateCSV(emoOrNotData, FileUtils.LogFolderPath(subjectData) + "\\emo_or_not.csv");

        Debug.Log("AllRecordData is recorded.");
    }

    private class RecordData
    {
        public string TotalTime{ get; set; }
        public string ElapsedTime{ get; set; }
        public string HitPointX{ get; set; }
        public string HitPointY{ get; set; }
    }

    private class EmoOrNotData
    {
        public int Count { get; set; }
        public string EmoOrNot { get; set; }
    }
}
