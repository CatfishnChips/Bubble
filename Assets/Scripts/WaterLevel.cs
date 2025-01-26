using UnityEngine;

public class WaterLevel : MonoBehaviour
{
    [SerializeField] [ReadOnly] [Range(0.0f, 1.0f)] private float _waterLevel;
    [SerializeField] [Range(0.0f, 1.0f)] private float _initialWaterLevel;
    [SerializeField] private Transform _waterTransform; 
    [SerializeField] private Vector2 _waterHeightMinMax;

    public void Start()
    {
        _waterLevel = _initialWaterLevel;
    }

    public void SetWaterLevel(float value)
    {
        _waterLevel = value;
        float height = Mathf.Lerp(_waterHeightMinMax.x, _waterHeightMinMax.y, _waterLevel);
        _waterTransform.position = new Vector3(_waterTransform.position.x, height, _waterTransform.position.z);
    }
}
