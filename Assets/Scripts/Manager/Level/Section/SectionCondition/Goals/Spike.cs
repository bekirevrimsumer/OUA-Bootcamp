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

    public void SpikeAnimation()
    {
        StartCoroutine(DelaySpike());
    }

    public IEnumerator DelaySpike()
    {
        yield return new WaitForSeconds(Delay);

        transform.DOLocalMoveY(0f, 0.4f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public void StopSpike()
    {
        transform.DOKill();
        StopAllCoroutines();
        transform.DOLocalMoveY(-2f, 0.4f).SetEase(Ease.InOutSine);
        transform.GetComponent<BoxCollider>().enabled = false;
    }
}
