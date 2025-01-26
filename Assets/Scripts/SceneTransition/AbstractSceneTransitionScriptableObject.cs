using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractSceneTransitionScriptableObject : ScriptableObject
{
    public float _animationTime = 0.25f;
    
    public abstract IEnumerator Enter(Canvas canvas);
    public abstract IEnumerator Exit(Canvas canvas);
}
