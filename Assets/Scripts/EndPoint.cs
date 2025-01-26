using UnityEngine;

public class EndPoint : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private StageManager _stageManager;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((_layerMask & (1 << col.gameObject.layer)) != 0)
        {
            if (_stageManager != null)
            {
                _stageManager.EndStage();
            }
        }
    }
}
