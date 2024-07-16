using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float Delay;

    void Start()
    {
        SpikeAnimation();
    }

    void Update()
    {
    }

    // public IEnumerator SpikeAnimation()
    // {
    //     if (!IsFirstTimeDelay)
    //     {
    //         IsFirstTimeDelay = true;
    //         yield return new WaitForSeconds(0.6f);
    //     }

    //     _isStarted = true;
    //     transform.DOLocalMoveY(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        
    // }

    public void SpikeAnimation()
    {
        StartCoroutine(DelaySpike());
    }

    public IEnumerator DelaySpike()
    {
        yield return new WaitForSeconds(Delay);

        transform.DOLocalMoveY(0f, 0.4f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

}
