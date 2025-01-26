using UnityEngine;
using UnityEngine.InputSystem;

public class ClickManager : MonoBehaviour
{
    private bool _isRunning;
    private Vector3 _mouseScreenPosition = Vector2.zero;
    private Vector3 _mouseWorldPosition = Vector3.zero;

    [Header("Mouse Settings")]
    [SerializeField] private float _mouseClickRadius;
    [SerializeField] private LayerMask _mouseClickLayerMask;

    //private Collider2D[] _colliders = new Collider2D[16];

    [Header("Force Settings")]
    [SerializeField] private float _forceAmount;
    [SerializeField] private float _forceCooldown;
    private float _currentCooldown;
    private bool CanApplyForce { get {return _currentCooldown <= 0;}}

    [Header("Drag Settings")]
    [SerializeField] private LayerMask _dragLayerMask;
    [SerializeField] [Range(0.0f, 1000.0f)] private float _maxForce = 500.0f;
    [SerializeField] [Range(0.0f, 100.0f)] private float _damping = 1.0f;
    [SerializeField] [Range(0.0f, 100.0f)] private float _frequency = 5.0f;
    [SerializeField] private bool _drawDragLine = true;
    [SerializeField] private Color _color = Color.cyan;
    private TargetJoint2D _targetJoint;

    private void Start()
    {
        _isRunning = true;
        _currentCooldown = 0f;
    }

    private void Update()
    {
        if (!_isRunning) return;

        HandleCooldown();

        _mouseScreenPosition = GetMouseScreenPosition();
        _mouseWorldPosition = GetMouseWorldPosition(_mouseScreenPosition);
        //Debug.Log("Mouse Screen Position: " + _mouseScreenPosition + " Mouse World Position: " + _mouseWorldPosition);

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D collider = Physics2D.OverlapPoint(_mouseWorldPosition, _dragLayerMask);
            if (collider != null)
            {
                // Fetch the collider's rigidbody.
                Rigidbody2D rigidbody2D = collider.attachedRigidbody;
                if (rigidbody2D == null) 
                    return;

                // Add a target joint to the Rigidbody2D GameObject.
                _targetJoint = rigidbody2D.gameObject.AddComponent<TargetJoint2D>();
                _targetJoint.maxForce = _maxForce;
                _targetJoint.dampingRatio = _damping;
                _targetJoint.frequency = _frequency;

                // Attach the anchor to the local-point where we clicked.
                _targetJoint.anchor = _targetJoint.transform.InverseTransformPoint(_mouseWorldPosition);    

                GameManager.Instance.SetCursor(CursorState.Drag);
            }
            else{
                if (CanApplyForce)
                {
                    ApplyForceOnClick();
                    SetCooldown();
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            GameManager.Instance.SetCursor(CursorState.Default);

            if (_targetJoint == null) 
                return;

            Destroy(_targetJoint);
            _targetJoint = null;
            return;
        }
        else
        {
            Collider2D collider = Physics2D.OverlapPoint(_mouseWorldPosition, _dragLayerMask);
            if (collider != null)
            {
                Rigidbody2D rigidbody2D = collider.attachedRigidbody;
                if (rigidbody2D == null) 
                    return;
                
                if (GameManager.Instance.CursorState == CursorState.Default)
                    GameManager.Instance.SetCursor(CursorState.Hover);
            }
            else 
            {
                if (GameManager.Instance.CursorState != CursorState.Drag)
                    GameManager.Instance.SetCursor(CursorState.Default);
            }
        }

        // Update the TargetJoint
        if (_targetJoint != null)
        {
            _targetJoint.target = _mouseWorldPosition;

            // Draw a line between the target and the joint anchor.
            if (_drawDragLine)
            {
                Debug.DrawLine (_targetJoint.transform.TransformPoint (_targetJoint.anchor), _mouseWorldPosition, _color);
            }
        }
    }

    private void HandleCooldown()
    {
        if (_currentCooldown > 0)
        {
            _currentCooldown -= Time.deltaTime;

            if (MouseGizmo.Instance != null)
            {
                int state = Mathf.RoundToInt((_currentCooldown / _forceCooldown) * 4);
                MouseGizmo.Instance.UpdateGizmoImage(state);
            }
        }
    }

    private void SetCooldown()
    {
        _currentCooldown = _forceCooldown;
    }

    private Vector3 GetMouseScreenPosition()
    {
        Vector3 mouseScreenPosition;
        mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = 10f;
        return mouseScreenPosition;
    }

    private Vector3 GetMouseWorldPosition(Vector3 screenPosition)
    {
       Vector3 mouseWorldPosition;
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);   
        mouseWorldPosition.z = 0;
        return mouseWorldPosition;
    }

    private void ApplyForceOnClick()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_mouseWorldPosition, _mouseClickRadius, _mouseClickLayerMask);
        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D rigidbody2D;
            Transform transform = collider.transform;
            if (transform.TryGetComponent<Rigidbody2D>(out rigidbody2D))
            {
                Vector3 direction = (transform.position - _mouseWorldPosition).normalized;
                Vector3 force = direction * _forceAmount;
                rigidbody2D.AddForceAtPosition(force, _mouseWorldPosition, ForceMode2D.Impulse);
            } 
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 mouseScreenPosition = GetMouseScreenPosition();
        Vector3 mouseWorldPosition = GetMouseWorldPosition(mouseScreenPosition);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(mouseWorldPosition, _mouseClickRadius);
    }
}
