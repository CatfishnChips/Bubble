using UnityEngine;
using UnityEngine.U2D;

public class SoftbodySkin2D : MonoBehaviour
{
    #region Constants
    private const float splineOffset = 0.5f;
    #endregion
    #region Fields
    [SerializeField] private SpriteShapeController _spriteShapeController;
    [SerializeField] private Transform[] _points;

    #endregion

    #region MonoBehaviour Callbacks
    private void Awake()
    {
        UpdateVertices();
    }

    private void Update()
    {
        UpdateVertices();
    }
    #endregion

    #region Private Methods
    private void GetVertices(){}
    private void UpdateVertices()
    {
        for (int i = 0; i < _points.Length - 1; i++)
        {
            Vector2 vertex = _points[i].localPosition;
            Vector2 towardsCenterDir = (Vector2.zero - vertex).normalized;
            float colliderRad = _points[i].gameObject.GetComponent<CircleCollider2D>().radius;
           
            try
            {
                _spriteShapeController.spline.SetPosition(i, vertex - towardsCenterDir * colliderRad);
            }
            catch
            {
                Debug.Log("Spline points are too close to each other.");
                _spriteShapeController.spline.SetPosition(i, vertex - towardsCenterDir * (colliderRad + splineOffset));
            }

            Vector2 leftTangent = _spriteShapeController.spline.GetLeftTangent(i);
            Vector2 newRightTangent = Vector2.Perpendicular(towardsCenterDir) * leftTangent.magnitude;
            Vector2 newLeftTangent = -(newRightTangent);
            _spriteShapeController.spline.SetRightTangent(i, newRightTangent);
            _spriteShapeController.spline.SetLeftTangent(i, newLeftTangent);
        }   
    }
    #endregion
}
