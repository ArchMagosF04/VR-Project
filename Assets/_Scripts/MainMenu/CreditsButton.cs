using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CreditsButton : MonoBehaviour
{
    [SerializeField] Animator creditsAnimation;

    public bool isActive = false;

    public void OnCreditsButtonPressed()
    {
        if (isActive == true)
        {
            isActive = false;
        }

        else {isActive = true; }

        creditsAnimation.SetBool("Start", isActive);
    }
}
