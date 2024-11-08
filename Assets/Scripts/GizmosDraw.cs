using UnityEngine;

public class GizmosDraw : MonoBehaviour
{
    private BoxCollider boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
    void OnDrawGizmos()
    {
        if (boxCollider != null)
        {
            Gizmos.color = Color.red; // Set the color of the gizmo
            Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.size);
        }
    }
}
