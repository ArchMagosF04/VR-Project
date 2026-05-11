using Oculus.Interaction;
using UnityEngine;

public class StarAnchor : MonoBehaviour
{
    [SerializeField] private StarObject anchoredStar;

    private void OnTriggerEnter(Collider other)
    {
        if (anchoredStar != null && anchoredStar.gameObject == other.gameObject) return;

        if (other.TryGetComponent<StarObject>(out StarObject star))
        {
            star.transform.parent = transform;
            star.anchor = this;
            anchoredStar = star;
        }
    }
}
