using UnityEngine;

[ExecuteAlways]
public class CustomAreaEffector : MonoBehaviour
{
    [SerializeField] private Vector2 _size;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _forceAmount;
    [SerializeField] [Range(0.0f, 1.0f)] private float _multiplier = 1.0f;
    private float _lenght;

    public float Multiplier { get { return _multiplier; } set { _multiplier = value; } }

    void Start()
    {
    
    }

    void Update()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        float angle = rotation.z;
        Vector2 size = new Vector2(_size.x, _size.y * _multiplier);
        float radius = size.y / 2;
        Vector2 overlapCenter = FindPointOnCircle(transform.position, angle, radius);

        Collider2D[] colliders = Physics2D.OverlapBoxAll(overlapCenter, size, angle - 90f , _layerMask);
        //Debug.Log("Euler Angles: " + rotation + " Number of Overlaps: " + colliders.Length);
        
        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D rigidbody2D;
            Transform collision = collider.transform;
            if (collision.TryGetComponent<Rigidbody2D>(out rigidbody2D))
            {
                //Vector3 direction = (collision.position - transform.position).normalized;
                Vector3 direction = transform.right;
                float distance = Mathf.Abs(collision.position.y - transform.position.y);
                float multiplier = 1f - (distance / size.y);
                Vector3 force = direction * _forceAmount * multiplier * _multiplier;
                rigidbody2D.AddForce(force, ForceMode2D.Force);
            } 
        }
    }

    private Vector2 FindPointOnCircle(Vector2 center, float angle, float radius)
    {
        Vector2 point;
        angle *= Mathf.Deg2Rad;
        point.y = center.y + radius * Mathf.Sin(angle);
        point.x = center.x + radius * Mathf.Cos(angle);
        return point;
    }

    void DrawBoxGizmo(Vector2 point, Vector2 size, float angle) 
    {
        var orientation = Quaternion.Euler(0, 0, angle);

        // Basis vectors, half the size in each direction from the center.
        Vector2 right = orientation * Vector2.right * size.x/2f;
        Vector2 up = orientation * Vector2.up * size.y/2f;

        // Four box corners.
        var topLeft = point + up - right;
        var topRight = point + up + right;
        var bottomRight = point - up + right;
        var bottomLeft = point - up - right;

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector2 size = new Vector2(_size.x, _size.y * _multiplier);

        Vector3 rotation = transform.rotation.eulerAngles;
        float angle = rotation.z;
        float radius = size.y / 2;
        Vector2 overlapCenter = FindPointOnCircle(transform.position, angle, radius);

        DrawBoxGizmo(overlapCenter, size, angle - 90f);
    }
}
