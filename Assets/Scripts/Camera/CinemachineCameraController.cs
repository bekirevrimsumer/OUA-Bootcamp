using Cinemachine;
using UnityEngine;

public class CinemachineCameraController : MonoBehaviour, IEventListener<MultiplayerEvent>
{
    public PlayerController Character;
    public bool FollowsAPlayer = false;

    protected CinemachineVirtualCamera _virtualCamera;

    public virtual void Awake() 
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void OnEvent(MultiplayerEvent eventType)
    {
        switch(eventType.MultiplayerEventType)
        {
            case MultiplayerEventType.JoinGame:
            SetCharacter(eventType.Character);
            break;
        }
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

    private void SetCharacter(GameObject character)
    {
        var characterController = character.FindChildObject("CharacterBase").GetComponent<PlayerController>();
        Character = characterController;
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
