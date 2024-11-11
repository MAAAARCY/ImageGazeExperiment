using System;
using UnityEngine;
using Fove.Unity;
using Fove;

public class HMDInfo : MonoBehaviour, IDisposable
{
    [SerializeField]
    private FoveInterface fove;
    
    [SerializeField]
    private GameObject foveHMD;

    [SerializeField]
    private GameObject image;

    [SerializeField]
    private GameObject toastWindow;

    [SerializeField]
    private GameObject parent;

    private Ray combinedGazeRay;
    private RaycastHit hit;
    private float seconds;

    private Vector2 uvCoordinate;

    public Vector2 UVCoordinate
    {
        get => uvCoordinate;
    }

    void Start()
    {
        this.parent.transform.rotation = Quaternion.Euler(0, this.foveHMD.transform.eulerAngles.y, 0);
        seconds = 0.0f;
    }

    void Update()
    {
        var eyeStateL = FoveManager.GetEyeState(Eye.Left).value.ToString();
        var eyeStateR = FoveManager.GetEyeState(Eye.Right).value.ToString();

        if (seconds < 1.0f)
        {
            this.parent.transform.rotation = Quaternion.Euler(0, this.foveHMD.transform.eulerAngles.y, 0);
            seconds += Time.deltaTime;
        }

        if (eyeStateL == "Opened" && eyeStateR == "Opened")
        {
            combinedGazeRay = this.fove.GetCombinedGazeRay();
            Debug.DrawRay(combinedGazeRay.origin, combinedGazeRay.direction * 30, Color.red, 5.0f);

            if (Physics.Raycast(combinedGazeRay, out hit))
            {
                string name = hit.collider.gameObject.name;
                uvCoordinate = hit.textureCoord;
            }
        }
        else
        {
            uvCoordinate = Vector2.zero;
        }

        this.toastWindow.transform.rotation = Quaternion.Euler(this.foveHMD.transform.eulerAngles.x, this.foveHMD.transform.eulerAngles.y, 0);
        // this.toastWindow.transform.rotation = Quaternion.Euler(-90, this.foveHMD.transform.eulerAngles.y, 0);
    }

    public void Dispose()
    {
        Debug.Log("called Dispose()");
    }
}
