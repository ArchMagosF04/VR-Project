using Oculus.Interaction;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class StarObject : MonoBehaviour
{
    public StarAnchor AnchorPoint;

    public List<StarConnection> connections = new List<StarConnection>();

    public void OnStarFirstSelection()
    {
        GameManager.Instance.SelectStar(this);
    }

    public void AddNewConnection(StarConnection newConnection)
    {
        connections.Add(newConnection);
    }

    public void OnStarDislodged()
    {
        for (int i = connections.Count - 1; i > -1; i--)
        {
            GameManager.Instance.DeleteConnection(connections[i]);
        }

        if (GameManager.Instance.SelectedStar == this) GameManager.Instance.SelectedStar = null;

        AnchorPoint.AnchoredStar = null;
        AnchorPoint = null;
    }
}
