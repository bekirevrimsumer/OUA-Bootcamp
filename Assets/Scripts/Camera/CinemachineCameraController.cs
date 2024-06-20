using Cinemachine;
using UnityEngine;

public class CinemachineCameraController : MonoBehaviour
{
    public CharacterController Character;
    public bool FollowsAPlayer = false;

    protected CinemachineVirtualCamera _virtualCamera;

    public virtual void Awake() 
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();    
    }

    public virtual void StartFollowing()
    {
        if (Character == null)
        {
            Debug.LogWarning("Karakter atanmamış.");
            return;
        }

        _virtualCamera.Follow = Character.transform;
    }

    public virtual void StopFollowing()
    {
        _virtualCamera.Follow = null;
    }
}
