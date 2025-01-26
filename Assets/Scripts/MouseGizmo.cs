using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseGizmo : MonoBehaviour
{
    #region Singleton

    public static MouseGizmo Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    #endregion

    private Image _image;
    private RectTransform _rectTransform;
    private Canvas _canvas;   
    [SerializeField] private Vector2 _offset = Vector2.zero;
    [SerializeField] private List<Sprite> _sprites;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _canvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        Vector2 mouseScreenPosition = GetMouseScreenPosition();
        SetScreenPosition(mouseScreenPosition + _offset);
    }

    private Vector3 GetMouseScreenPosition()
    {
        Vector3 mouseScreenPosition;
        mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = 10f;
        return mouseScreenPosition;
    }

    public void UpdateGizmoImage(int state)
    {
        if (_image != null)
        {
            _image.sprite = _sprites[state];
        }
    }

    public void SetScreenPosition(Vector2 screenPosition){
        //Vector position (percentage from 0 to 1) considering camera size.
        //For example (0,0) is lower left, middle is (0.5,0.5)
        Vector2 viewportPoint = Camera.main.ScreenToViewportPoint(screenPosition);

        var rootCanvasTransform = (_canvas.isRootCanvas ? _canvas.transform : _canvas.rootCanvas.transform) as RectTransform;
        var rootCanvasSize = rootCanvasTransform!.rect.size;
        //Calculate position considering our percentage, using our canvas size
        //So if canvas size is (1100,500), and percentage is (0.5,0.5), current value will be (550,250)

        var rootCoord = (viewportPoint - rootCanvasTransform.pivot) * rootCanvasSize;
        if (_canvas.isRootCanvas)       
            _rectTransform.anchoredPosition = rootCoord;
    }

    public Vector2 ScreenToCanvasPosition(Vector2 screenPosition){
        //Vector position (percentage from 0 to 1) considering camera size.
        //For example (0,0) is lower left, middle is (0.5,0.5)
        Vector2 viewportPoint = Camera.main.ScreenToViewportPoint(screenPosition);

        var rootCanvasTransform = (_canvas.isRootCanvas ? _canvas.transform : _canvas.rootCanvas.transform) as RectTransform;
        var rootCanvasSize = rootCanvasTransform!.rect.size;
        //Calculate position considering our percentage, using our canvas size
        //So if canvas size is (1100,500), and percentage is (0.5,0.5), current value will be (550,250)

        var rootCoord = (viewportPoint - rootCanvasTransform.pivot) * rootCanvasSize;     
        return rootCoord;
    }
}
