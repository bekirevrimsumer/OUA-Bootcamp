using Cinemachine;
using UnityEngine;

public class PlayerSetup : MonoBehaviour, IEventListener<MultiplayerEvent>
{
    // [SerializeField] private MoveObjWithRay moveObjWithRay;
    public CinemachineVirtualCamera Camera;
    public Transform CameraFollowTransform;

    public bool IsLocal { get; private set; } = false;

	public void IsLocalPlayer()
	{
        IsLocal = true;
        Camera.Follow = CameraFollowTransform;
	}

    public void OnEvent(MultiplayerEvent eventType)
    {
        switch(eventType.MultiplayerEventEventType)
        {
            case MultiplayerEventType.JoinGame:
            IsLocalPlayer();
            break;
        }
    }

    protected virtual void OnEnable()
    {
        this.StartListeningEvent<MultiplayerEvent>();
    }

    protected virtual void OnDisable()
    {
        this.StopListeningEvent<MultiplayerEvent>();
    }
}
