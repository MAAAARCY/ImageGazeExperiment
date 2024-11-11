using UnityEngine;
using TMPro;

public class GazeImage : MonoBehaviour
{
    [SerializeField]
    private Material material;

    [SerializeField]
    private Texture2D[] texture;

    [SerializeField]
    private Texture2D[] test;

    [SerializeField]
    private Recorder recorder;

    [SerializeField]
    private TMPro.TMP_Text elapsedTimeText;

    [SerializeField]
    private GameObject imageObject;

    [SerializeField]
    private GameObject question;

    private int currentImageId = 0;

    private float totalTime = 0f;
    private float elapsedTime = 0f;
    private float timeSpan = 5f;

    private bool isRecordCompleted = false;
    private bool isExperimentInitialized = false;
    private bool isExperimentFinished = false;

    public string TextureName
    {
        get
        {
            return texture[currentImageId].name;
        }
    }

    private void Start()
    {
        material.SetTexture("_MainTex", texture[0]);
        imageObject.SetActive(false);
        elapsedTimeText.SetText("Spaceを押して実験を開始します");
        // texture = test;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            imageObject.SetActive(true);
            isExperimentInitialized = true;
            recorder.RecordEnable = true;
        }

        if (elapsedTime > timeSpan)
        {
            elapsedTimeText.SetText("");
            question.SetActive(true);

            if (currentImageId <= texture.Length - 1 && !isRecordCompleted)
            {
                if (currentImageId != texture.Length - 1)
                {
                    recorder.ResetRecordData();
                    currentImageId++;
                    isRecordCompleted = true;
                    recorder.RecordEnable = false;
                }
                else
                {
                    recorder.ResetRecordData();
                    isRecordCompleted = true;
                    recorder.RecordEnable = false;
                    isExperimentFinished = true;
                }

            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                elapsedTime = 0f;
                question.SetActive(false);
                isRecordCompleted = false;
                recorder.RecordEnable = true;

                recorder.RecordEmoOtNotData("emo");

                if (!isExperimentFinished) material.SetTexture("_MainTex", texture[currentImageId]);
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                elapsedTime = 0f;
                question.SetActive(false);
                isRecordCompleted = false;
                recorder.RecordEnable = true;

                recorder.RecordEmoOtNotData("non-emo");

                if (!isExperimentFinished) material.SetTexture("_MainTex", texture[currentImageId]);
            }
        }

        if (!question.activeSelf && isExperimentInitialized)
        {
            if (!isExperimentFinished)
            {
                elapsedTime += Time.deltaTime;
                elapsedTimeText.SetText((currentImageId+1).ToString() + "枚目" + ", 残り：" + (timeSpan - elapsedTime).ToString("0") + "秒");
            }
            else
            {
                imageObject.SetActive(false);
                elapsedTimeText.SetText("実験は終了です、お疲れさまでした。");
            }
            
        }

        totalTime += Time.deltaTime;
    }

    private void OnDisable()
    {
        material.SetTexture("_MainTex", texture[0]);
    }
}
