using DG.Tweening;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private Vector3 destinationWorldSpace;
    [SerializeField] private float timeToReach;

    private Tween moveTween;

    private void Start()
    {
        moveTween = transform.DOMove(destination ? destination.position : destinationWorldSpace, timeToReach).SetEase(Ease.Linear).OnComplete(OnEndReached);
    }

    public void StopBlackHole()
    {
        if (moveTween != null) moveTween.Kill(false);
    }

    public void OnEndReached()
    {
        Debug.Log("All Consumed");
        GameManager.Instance.OnGameLost();
    }
}
