using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Singleton

    public static CameraManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        OnAwake();
    }

    #endregion

    [Header("References")]
    [field: SerializeField] private CinemachineCamera _mainVirtualCamera;
    [field: SerializeField] private Transform _defaultTrackingTarget;

    [Header("Settings")]
    [field: SerializeField] private Vector2 _followOffsetMin;
    [field: SerializeField] private Vector2 _followOffsetMax;

    private Camera _mainCamera;
    private Transform _trackedTransform;

    private void OnAwake()
    {
    }

    private void Start()
    {
        SetDefaultTarget();
    }

    private void Update()
    {
        if (_trackedTransform == null)
        {
            SetDefaultTarget();
        }
    }

    private void SetDefaultTarget()
    {
        _trackedTransform = _defaultTrackingTarget;
        _mainVirtualCamera.Target.TrackingTarget = _defaultTrackingTarget;
    }

    public void SetCameraFollowTarget(Transform target)
    {
        if (target == null) return;
        _trackedTransform = target;
        _mainVirtualCamera.Target.TrackingTarget = _trackedTransform;
    }
}
