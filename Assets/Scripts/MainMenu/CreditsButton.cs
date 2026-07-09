using UnityEngine;

public class CreditsButton : MonoBehaviour
{
    [SerializeField] Animator creditsAnimation;
    [SerializeField] private Canvas creditsCanvas;


    public bool isActive = false;

    private void Awake()
    {
        if (creditsCanvas == null) creditsCanvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        isActive = false;
        creditsCanvas.enabled = isActive;
    }

    public void OnCreditsButtonPressed()
    {
        if (isActive == true)
        {
            isActive = false;
        }
        else {isActive = true; }


        Debug.Log(isActive);

        creditsCanvas.enabled = isActive;
        creditsAnimation.SetBool("Start", isActive);
    }
}
