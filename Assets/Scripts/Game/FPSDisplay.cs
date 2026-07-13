using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    //public int avgFrameRate;
    public TMP_Text displayText;

    //public void Update()
    //{
    //    float current = 0;
    //    current = (int)(1f / Time.unscaledDeltaTime);
    //    avgFrameRate = (int)current;
    //    displayText.text = avgFrameRate.ToString() + " FPS";
    //}

    [SerializeField] private float updateInterval = 0.5f;

    private float _accumulatedTime = 0f;
    private int _frameCount = 0;

    void Update()
    {
        // Track the real, unscaled time and frame count
        _accumulatedTime += Time.unscaledDeltaTime;
        _frameCount++;

        // Update the display text when the target interval is met
        if (_accumulatedTime >= updateInterval)
        {
            int fps = Mathf.RoundToInt(_frameCount / _accumulatedTime);
            displayText.text = $"FPS: {fps}";

            // Reset buffers
            _accumulatedTime = 0f;
            _frameCount = 0;
        }
    }

}
