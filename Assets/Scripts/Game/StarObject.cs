using System.Collections.Generic;
using UnityEngine;

public class StarObject : MonoBehaviour
{
    public StarAnchor AnchorPoint;

    public List<StarConnection> connections = new List<StarConnection>();

    [SerializeField,Range(1f, 5f)] private int maxNumberOfConnections;

    private MaterialPropertyBlock propertyBlock;

    private MeshRenderer meshRenderer;

    [Header("Mat Settings")]
    [SerializeField, ColorUsage(true, true)] private Color neonColor;
    [SerializeField, ColorUsage(true, true)] private Color emissionColor;
    [SerializeField] private float pulseSpeed = 1f;
    [SerializeField] private float glowStrength = 10f;
    [SerializeField, ColorUsage(true, true)] private Color baseColor;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        propertyBlock = new MaterialPropertyBlock();
    }

    private void Start()
    {
        propertyBlock.SetColor("_NeonColor", neonColor);
        propertyBlock.SetColor("_EmissionColor", emissionColor);
        propertyBlock.SetFloat("_PulseSpeed", pulseSpeed);
        propertyBlock.SetFloat("_GlowStrength", glowStrength);
        propertyBlock.SetColor("_BaseColor", baseColor);

        meshRenderer.SetPropertyBlock(propertyBlock);
    }

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
