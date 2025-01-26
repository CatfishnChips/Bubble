using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Transition
{
    public SceneTransitionMode Mode;
    public AbstractSceneTransitionScriptableObject TransitionSO;
}

[System.Serializable]
public enum SceneTransitionMode
{
    Circle
}
