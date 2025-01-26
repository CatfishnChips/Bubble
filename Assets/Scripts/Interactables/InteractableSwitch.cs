using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableSwitch : MonoBehaviour
{
    [SerializeField] private bool _initialState;
    [SerializeField] [ReadOnly] private bool _state;

    public bool State { get { return _state; } }

    void Start()
    {
        _state = _initialState;
    }

    private void OnMouseDown()
    {
        _state =! _state;
    }
}
