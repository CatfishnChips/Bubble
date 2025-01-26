using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }

    #endregion

    [SerializeField] private Texture2D _cursorDefaultTexture;
    [SerializeField] private Texture2D _cursorHoverTexture;
    [SerializeField] private Texture2D _cursorDragTexture;
    private CursorState _cursorState = CursorState.Default;
    
    public CursorState CursorState { get { return _cursorState; } }

    private void Start()
    {
        SetCursor(CursorState.Default);
    }

    public void SetCursor(CursorState state)
    {
        Texture2D texture2D = _cursorDefaultTexture;
        switch (state)
        {
            case CursorState.Default:
                if (_cursorDefaultTexture != null)
                    texture2D = _cursorDefaultTexture;
                    _cursorState = CursorState.Default;
            break;

            case CursorState.Hover:
                if (_cursorHoverTexture != null)
                    texture2D = _cursorHoverTexture;
                    _cursorState = CursorState.Hover;
            break;

            case CursorState.Drag:
                if (_cursorDragTexture != null)
                    texture2D = _cursorDragTexture;
                    _cursorState = CursorState.Drag;
            break;
        }
        Cursor.SetCursor(texture2D, Vector2.zero, CursorMode.Auto);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

public enum CursorState
{
    Default,
    Hover,
    Drag
}