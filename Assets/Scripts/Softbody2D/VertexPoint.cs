using UnityEngine;

public class VertexPoint : MonoBehaviour
{
    private Softbody2D _softbody2D;
    public Softbody2D Softbody2D { get { return _softbody2D; } set { _softbody2D = value; } }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("OnCollisionEnter2D: " + col.collider.name);
        if (_softbody2D == null) return;
        if (col.collider == null) return;
        if (col.otherCollider == col.collider) return;
        if (col.collider.isTrigger) return;
        if (col.otherCollider.gameObject.layer == col.collider.gameObject.layer) return;

        _softbody2D.HandleCollision(col);
    }

    private void OnDestroy()
    {
        _softbody2D = null;
    }
}
