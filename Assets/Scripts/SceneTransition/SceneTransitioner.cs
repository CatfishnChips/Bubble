using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
[RequireComponent(typeof(Canvas))]
public class SceneTransitioner : MonoBehaviour
{
    #region Singleton
    private static SceneTransitioner _instance;

    public static SceneTransitioner Instance 
    { 
        get => _instance;
        private set => _instance = value;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.activeSceneChanged += HandleSceneChange;
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _canvas = GetComponent<Canvas>();
        _canvas.enabled = false;
    }

    #endregion

    private Canvas _canvas;

    private AsyncOperation _loadLevelOperation;
    [SerializeField] private List<Transition> _transitions = new();
    private AbstractSceneTransitionScriptableObject _activeTransition;

    private void HandleSceneChange(Scene from, Scene to){
        if (_activeTransition != null){
            StartCoroutine(Enter());
        }
    }

    public void LoadScene(string scene, SceneTransitionMode transitionMode = SceneTransitionMode.Circle, LoadSceneMode mode = LoadSceneMode.Single){
        if (_activeTransition != null) return;
        if (_loadLevelOperation != null) return;

        //_loadLevelOperation = SceneManager.LoadSceneAsync(scene, mode);

        Transition transition = _transitions.Find((transition) => transition.Mode == transitionMode);
        if (transition != null){
            //_loadLevelOperation.allowSceneActivation = false;
            _canvas.enabled = true;
            _activeTransition = transition.TransitionSO;
            StartCoroutine(Exit(scene, transitionMode, mode));
        }
        else{
            Debug.LogWarning($"No transition found for TransitionMode {transitionMode}!");
        }
    }

    private IEnumerator Exit(string scene, SceneTransitionMode transitionMode = SceneTransitionMode.Circle, LoadSceneMode mode = LoadSceneMode.Single){
        yield return StartCoroutine(_activeTransition.Exit(_canvas));
        _loadLevelOperation = SceneManager.LoadSceneAsync(scene, mode);
        //_loadLevelOperation.allowSceneActivation = true;
    }

    private IEnumerator Enter(){
        _canvas.enabled = true;
        yield return StartCoroutine(_activeTransition.Enter(_canvas));
        _canvas.enabled = false;
        _loadLevelOperation = null;
        _activeTransition = null;
    }

    public void EnterIntoTransition(SceneTransitionMode transitionMode = SceneTransitionMode.Circle){
        Transition transition = _transitions.Find((transition) => transition.Mode == transitionMode);
        if (transition != null){
            _canvas.enabled = true;
            _activeTransition = transition.TransitionSO;
            StartCoroutine(EnterIntoTransition());
        }
        else{
            Debug.LogWarning($"No transition found for TransitionMode {transitionMode}!");
        }
    }

    private IEnumerator EnterIntoTransition(){
        yield return StartCoroutine(_activeTransition.Exit(_canvas));
    }

    public void ExitFromTransition(SceneTransitionMode transitionMode = SceneTransitionMode.Circle){
        Transition transition = _transitions.Find((transition) => transition.Mode == transitionMode);
        if (_activeTransition != transition.TransitionSO) return;

        if (_activeTransition != null){
            StartCoroutine(ExitFromTransition());
        }
    }

    private IEnumerator ExitFromTransition(){
        _canvas.enabled = true;
        yield return StartCoroutine(_activeTransition.Enter(_canvas));
        _canvas.enabled = false;
        _activeTransition = null;
    }
}