using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Circle Transition", menuName = "Scene Transitions/Circle")]
public class CircleTransitionScriptableObject : AbstractSceneTransitionScriptableObject
{
    private static readonly int _transitionCircleSizeID = Shader.PropertyToID("_TransitionCircleSize");
    private Material _material;
    private WaitForSecondsRealtime _wait = new WaitForSecondsRealtime(0.25f);

    public override IEnumerator Exit(Canvas canvas)
    {
        float time = 0;
        float initialSize = 1;
        _material = canvas.GetComponentInChildren<Image>().materialForRendering;

        while (time < 1){
            _material.SetFloat(_transitionCircleSizeID, Mathf.Lerp(initialSize, 0f, time));
            yield return null;
            time += Time.deltaTime / _animationTime;
        }
        _material.SetFloat(_transitionCircleSizeID, 0f);
        yield return _wait;
    }

    public override IEnumerator Enter(Canvas canvas)
    {
        float time = 0;
        float targetSize = 1;
        _material = canvas.GetComponentInChildren<Image>().materialForRendering;
        yield return _wait;
        
        while (time < 1){
            _material.SetFloat(_transitionCircleSizeID, Mathf.Lerp(0f, targetSize, time));
            yield return null;
            time += Time.deltaTime / _animationTime;
        }
        _material.SetFloat(_transitionCircleSizeID, targetSize);
    }
}
