using DG.Tweening;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private Transform holeVisual;
    [SerializeField] private Vector3 destinationWorldSpace;
    [SerializeField] private float timeToReach;
    [SerializeField] private ParticleSystem[] complementaryParticles;

    [SerializeField] private ParticleSystem explosionParticle;

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

    public void StopBlackhole()
    {
        if (moveTween != null) moveTween.Kill(false);
    }

    [ContextMenu("ExplosionTest")]
    public void DestroyBlackHole()
    {
        foreach (ParticleSystem particle in complementaryParticles)
        {
            particle.Stop();
            particle.gameObject.SetActive(false);
        }

        holeVisual.DOScale(0, 3).SetEase(Ease.InBack).OnComplete(()=> explosionParticle.Play());
    }

    public void HideBlackhole()
    {
        gameObject.SetActive(false);
    }
}
