using Fove.Unity;
using UnityEngine;

public class FoveControl : MonoBehaviour
{
    void Start()
    {
        Debug.Log(FoveManager.TareOrientation());
    }

    void Update()
    {
        
    }
}
