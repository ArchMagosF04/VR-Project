using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    public int avgFrameRate;
    public TMP_Text displayText;

    public void Update()
    {
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        avgFrameRate = (int)current;
        displayText.text = avgFrameRate.ToString() + " FPS";
    }
}
