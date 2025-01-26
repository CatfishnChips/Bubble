using UnityEngine;

public class TransitionScene : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    public void LoadScene()
    {
        SceneTransitioner.Instance.LoadScene(_sceneName, transitionMode: SceneTransitionMode.Circle);
    }
}
