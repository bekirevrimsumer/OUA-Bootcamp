using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TreeChanger : MonoBehaviour, IEventListener<LightReflectionEvent>
{
    public GameObject DeadTree;
    public GameObject OriginalTree;
    public float Delay;

    public void ChangeTree()
    {
        DOVirtual.DelayedCall(Delay, () => {
            TransitionTree();
        });
    }

    public void OnEvent(LightReflectionEvent eventType)
    {
        switch(eventType.LightReflectionEventType)
        {
            case LightReflectionEventType.HitTarget:
                ChangeTree();
                break;
        }
    }

    private void TransitionTree()
    {
        ShakeTree(DeadTree);
        DeadTree.transform.DOScale(Vector3.zero, 2f)
            .OnComplete(() => {
                DeadTree.SetActive(false);
                OriginalTree.SetActive(true);

                OriginalTree.transform.localScale = Vector3.zero;

                OriginalTree.transform.DOScale(Vector3.one, 2f);

                ShakeTree(OriginalTree);
            });
    }

        private void ShakeTree(GameObject tree)
    {
        // Apply a small shaking effect to the tree
        tree.transform.DOShakePosition(1f, new Vector3(0.1f, 0.1f, 0.1f), 10, 90, false, true);
        tree.transform.DOShakeRotation(1f, new Vector3(5f, 5f, 0), 10, 90, true);
    }

    protected virtual void OnEnable()
    {
        this.StartListeningEvent<LightReflectionEvent>();
    }

    protected virtual void OnDisable()
    {
        this.StopListeningEvent<LightReflectionEvent>();
    }
}
