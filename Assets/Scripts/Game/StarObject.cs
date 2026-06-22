using Oculus.Interaction;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class StarObject : MonoBehaviour
{
    public StarAnchor AnchorPoint;

    public List<StarConnection> connections = new List<StarConnection>();

    [SerializeField,Range(1f, 5f)] private int maxNumberOfConnections;

    public void OnStarFirstSelection()
    {
        Debug.Log("Star Touched");
        GameManager.Instance.SelectStar(this);
    }

    public void AddNewConnection(StarConnection newConnection)
    {
        if (HasMaxConnections()) return;

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

    public bool HasMaxConnections()
    {
        if (connections.Count >= maxNumberOfConnections) return true;
        else return false;
    }
}
