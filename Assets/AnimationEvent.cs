using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    public UnityEvent OnAnimationKey;

    public void CallEvent()
    {
        OnAnimationKey?.Invoke();
    }
}
