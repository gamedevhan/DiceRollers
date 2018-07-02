using UnityEngine;

public static class DebugUtility
{
	public static void Log(string text, string color = "green")
    {
        Debug.Log("<b><color=" + color + ">" + text + "</color></b>");
    }
}
