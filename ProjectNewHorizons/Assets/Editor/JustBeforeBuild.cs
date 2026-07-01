using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class JustBeforeBuild : IPreprocessBuildWithReport
{
    private DevFunctions dev = new();
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        Debug.Log("Running BEFORE build starts!");

        dev.ResetFullSaveFile();
    }
}
