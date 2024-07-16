using DG.Tweening;
using Photon.Pun;
using UnityEngine;

public class TreeChanger : MonoBehaviourPunCallbacks, IEventListener<SectionEvent>
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

    public void OnEvent(SectionEvent eventType)
    {
        switch(eventType.SectionEventType)
        {
            case SectionEventType.SectionCompleted:
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
        tree.transform.DOShakePosition(1f, new Vector3(0.1f, 0.1f, 0.1f), 10, 90, false, true);
        tree.transform.DOShakeRotation(1f, new Vector3(5f, 5f, 0), 10, 90, true);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        this.StartListeningEvent<SectionEvent>();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        this.StopListeningEvent<SectionEvent>();
    }
}
