using UnityEngine;

public class EndlessScrollingBackground : MonoBehaviour
{
	[SerializeField] private float _width = 0.0f;
    [SerializeField] private float _height = 0.0f;
    [SerializeField] private Vector2 _scrollSpeed = Vector2.zero;
    private Vector2 _startPosition;
    private Vector2 _currentPosition;
    private RectTransform _rectTransform;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _startPosition = _rectTransform.anchoredPosition;
        _currentPosition = _startPosition;
    }

    private void Update()
    {
        _currentPosition = _rectTransform.anchoredPosition;
        _currentPosition = new Vector3(_currentPosition.x + _scrollSpeed.x * Time.deltaTime, _currentPosition.y + _scrollSpeed.y * Time.deltaTime, 0.0f);

        if (_currentPosition.x > _startPosition.x + _width) _currentPosition.x = _startPosition.x -_width;
        else if (_currentPosition.x < _startPosition.x - _width) _currentPosition.x = _startPosition.x + _width;

        if (_currentPosition.y > _startPosition.y + _height) _currentPosition.y = _startPosition.y - _height;
        else if (_currentPosition.y < _startPosition.y - _height) _currentPosition.y = _startPosition.y + _height;

        _rectTransform.anchoredPosition = _currentPosition;
    }
}
