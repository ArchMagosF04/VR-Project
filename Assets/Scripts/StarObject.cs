using UnityEngine;

public class StarObject : MonoBehaviour
{
    public Rigidbody rb;
    public StarAnchor anchor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (anchor != null) transform.position = anchor.transform.position;
    }
}
