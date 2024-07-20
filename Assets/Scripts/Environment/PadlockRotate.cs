using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class PadlockRotate : MonoBehaviourPunCallbacks
{
    public static event Action<string, int> Rotated = delegate { };

    private bool _coroutineAllowed;
    private int _numberShown;

    private void Start()
    {
        _coroutineAllowed = true;
        _numberShown = 0;
    }

    private void OnMouseDown()
    {
        if (_coroutineAllowed)
        {
            photonView.RPC("StartRotateWheel", RpcTarget.All);
        }
    }

    [PunRPC]
    private void StartRotateWheel()
    {
        if (_coroutineAllowed)
        {
            StartCoroutine(RotateWheel());
        }
    }

    private IEnumerator RotateWheel()
    {
        _coroutineAllowed = false;

        for (int i = 0; i <= 11; i++)
        {
            transform.Rotate(0f, 3f, 0f);
            yield return new WaitForSeconds(0.01f);
        }

        _coroutineAllowed = true;

        _numberShown += 1;

        if (_numberShown > 9)
        {
            _numberShown = 0;
        }

        Rotated(name, _numberShown);
    }
}
