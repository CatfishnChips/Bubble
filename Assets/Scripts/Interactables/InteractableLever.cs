using UnityEngine;
using UnityEngine.Events;

public class InteractableLever : MonoBehaviour
{
    [SerializeField] private HingeJoint2D _hingeJoint2D;
    [SerializeField] [ReadOnly] private float _referenceAngle;
    [SerializeField] [ReadOnly] private float _jointAngle;
    [SerializeField] [ReadOnly] private float _progress;
    private JointAngleLimits2D _angleLimits;

    public float ReferenceAngle { get { return _referenceAngle; } }
    public float JointAngle { get { return _jointAngle; } }

    public UnityEvent<float> OnUpdate;

    void Start()
    {
        _referenceAngle = _hingeJoint2D.referenceAngle;
        _angleLimits = _hingeJoint2D.limits;
    }

    void FixedUpdate()
    {
        _jointAngle = _hingeJoint2D.jointAngle;

        float offset = 0 - _angleLimits.max;
        float offsetAngle = _jointAngle + offset;
        
        _progress = offsetAngle / (_angleLimits.min + offset);
        OnUpdate?.Invoke(_progress);
    }
}
