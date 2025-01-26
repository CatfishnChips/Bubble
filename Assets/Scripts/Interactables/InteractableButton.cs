using UnityEngine;
using UnityEngine.Events;

public class InteractableButton : MonoBehaviour
{
    public UnityEvent OnMouse;

    private void OnMouseDown()
    {
        Debug.Log("BUTTON!");
        OnMouse?.Invoke();
    }
}
