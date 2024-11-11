using System;
using UnityEngine;

[Serializable]
public class TestSubjectInfo
{
    public string Name;
    public string Grade;
    public string ExperimentalDate;

}

[CreateAssetMenu(menuName = "Settings/Config", fileName = "Config")]
public class Config : ScriptableObject
{
    [SerializeField]
    private TestSubjectInfo[] testSubjectInfo;

    public TestSubjectInfo[] GetTestSubjectInfo()
    {
        return testSubjectInfo;
    }
}