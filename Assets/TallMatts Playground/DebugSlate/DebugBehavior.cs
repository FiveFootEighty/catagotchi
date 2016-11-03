using UnityEngine;
using System.Collections;

public class DebugBehavior : MonoBehaviour {

    public static TextMesh debugText;
    private static int messageCount = 0;
    private static int MAX_MESSAGE_AMOUNT = 13;

	void Start () {
        debugText = transform.FindChild("DebugText").GetComponent<TextMesh>();
	}

    public static void Log(string message)
    {
        Debug.Log(message);
        if (messageCount == MAX_MESSAGE_AMOUNT)
        {
            debugText.text = TrimText(debugText.text);
            messageCount--;
        }
        debugText.text += message + "\n";

        messageCount++;
    }

    private static string TrimText(string text)
    {
        return text.Substring(text.IndexOf("\n")+1);
    }
}
