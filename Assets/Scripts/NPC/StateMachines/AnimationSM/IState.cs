using UnityEngine;

public interface IState
{
    void EnterState(NPCController npcController);
    void UpdateState(NPCController npcController);
    void ExitState(NPCController npcController);
}
