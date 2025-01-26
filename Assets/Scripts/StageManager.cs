using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [SerializeField] private GameObject _bubblePrefab;
    [SerializeField] [ReadOnly] private GameObject _bubbleObject;
    [SerializeField] private Transform _spawnPoint;

    private float _currentResetCooldown;
    [SerializeField] private float _resetCooldown = 2.0f;

    [Header("Force Settings")]
    [SerializeField] private LayerMask _forceLayerMask;
    [SerializeField] private float _forceRadius = 1.0f;
    [SerializeField] private Vector2 _force = new Vector2(1.0f, 1.0f);
    [SerializeField] private float _forceDelay = 0.25f;

    private void Start()
    {
        _currentResetCooldown = 0.0f;
        _bubbleObject = null;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetCursor(CursorState.Default);
        }
      
        if (MouseGizmo.Instance != null)
        {
            MouseGizmo.Instance.UpdateGizmoImage(0);
        }
    }

    private void Update()
    {   
        HandleResetCooldown();

        // Reload Scene
        if (Input.GetKeyDown(KeyCode.T))
        {
            Scene activeScene = SceneManager.GetActiveScene();
            SceneTransitioner.Instance.LoadScene(activeScene.name, transitionMode: SceneTransitionMode.Circle, LoadSceneMode.Single);
        }

        if (_bubbleObject != null)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (_currentResetCooldown <= 0.0f)
                {
                    _currentResetCooldown = _resetCooldown;
                    Reset();
                }
            }
        }
        else 
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnBubble();
            }
        }
    }

    private void HandleResetCooldown()
    {
        if (_currentResetCooldown > 0.0f)
        {
            _currentResetCooldown -= Time.deltaTime;
        }
    }

    private void Reset()
    {
        SpawnBubble();
    }

    private void SpawnBubble()
    {
        if (_bubbleObject != null)
        {
            _bubbleObject.GetComponent<Softbody2D>().PopBubble();
        }

        _bubbleObject = Instantiate(_bubblePrefab);
        _bubbleObject.transform.SetPositionAndRotation(_spawnPoint.position, Quaternion.identity);

        if (CameraManager.Instance != null)
        {
            CameraManager.Instance.SetCameraFollowTarget( _bubbleObject.transform.GetChild(0));
        }
        
        Invoke("ApplyForce", _forceDelay);
    }

    private void ApplyForce()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_spawnPoint.position, _forceRadius, _forceLayerMask);
        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D rigidbody2D;
            Transform transform = collider.transform;
            if (transform.TryGetComponent<Rigidbody2D>(out rigidbody2D))
            {
                rigidbody2D.AddForceAtPosition(_force, _spawnPoint.position, ForceMode2D.Impulse);
            } 
        }
    }

    public void EndStage()
    {
        Debug.Log("End Stage");
        Application.Quit();
    }
}
