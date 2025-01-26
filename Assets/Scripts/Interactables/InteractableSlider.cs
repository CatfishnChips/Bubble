using UnityEngine;
using UnityEngine.Events;

public class InteractableSlider : MonoBehaviour
{
    [SerializeField] private SliderJoint2D _sliderJoint2D;
    [SerializeField] [ReadOnly] private float _referenceAngle;
    [SerializeField] [ReadOnly] private float _jointTranslation;
    [SerializeField] [ReadOnly] private float _progress;

    public UnityEvent<float> OnUpdate;

    void Start()
    {
        _referenceAngle = _sliderJoint2D.referenceAngle;
    }

    void Update()
    {
        _jointTranslation = _sliderJoint2D.jointTranslation;

        //_progress = 
        OnUpdate?.Invoke(_progress);
    }
}
