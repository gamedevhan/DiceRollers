using UnityEngine;

public static class DebugUtility
{
	public static void Log(string text, string color = "green")
    {
        Debug.Log("<b><color=" + color + ">" + text + "</color></b>");
    }

	public static void Log(int value, string color = "green")
	{
		Debug.Log("<b><color=" + color + ">" + value + "</color></b>");
	}
}
