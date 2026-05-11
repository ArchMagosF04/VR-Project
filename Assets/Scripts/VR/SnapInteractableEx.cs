using Oculus.Interaction;
using System.Collections.Generic;
using UnityEngine;

public class SnapInteractableEx : SnapInteractable
{
    public List<SnapInteractor> _snappedInteractors = new();

    public StarAnchor anchor;

    protected override void Awake()
    {
        base.Awake();
        anchor = GetComponentInParent<StarAnchor>();
    }

    protected override void SelectingInteractorAdded(SnapInteractor interactor)
    {
        base.SelectingInteractorAdded(interactor);
        if (!_snappedInteractors.Contains(interactor))
        {
            _snappedInteractors.Add(interactor);
            Debug.Log($"Objeto snapeado por: {interactor.name}");
        }

        StarObject snapStar = interactor.GetComponentInParent<StarObject>();

        if (snapStar != null)
        {
            anchor.AnchoredStar = snapStar;
            snapStar.AnchorPoint = anchor;
        }
    }

    protected override void SelectingInteractorRemoved(SnapInteractor interactor)
    {
        base.SelectingInteractorRemoved(interactor);
        if (_snappedInteractors.Remove(interactor))
        {
            Debug.Log($"Objeto liberado por: {interactor.name}");
        }

        StarObject snapStar = interactor.GetComponentInParent<StarObject>();

        if (snapStar != null)
        {
            snapStar.OnStarDislodged();
        }
    }

    public IReadOnlyList<GameObject> GetSnappedObjects()
    {
        List<GameObject> snappedObjects = new();
        foreach (var interactor in _snappedInteractors)
        {
            var interactable = interactor.SelectedInteractable;
            if (interactable != null)
            {
                snappedObjects.Add(interactable.gameObject);
            }
        }
        return snappedObjects;
    }

    public bool HasAnySnapped() => _snappedInteractors.Count > 0;
}
