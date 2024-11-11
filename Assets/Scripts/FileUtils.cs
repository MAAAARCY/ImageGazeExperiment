using System.IO;

public struct SubjectData
{
    public string Name;
    public string ExperimentDate;
}

internal static class FileUtils
{
    public static string LogFolderPath(SubjectData data)
        => System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + $"\\Unity\\ImageGazeExperiment\\Logs\\{data.Name}_{data.ExperimentDate}";

    public static string SubjectNameFilePath
        => System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + "\\Unity\\ImageGazeExperiment\\SubjectName.txt";
}
