using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviourPunCallbacks
{
    public Animator Animator;
    public NavMeshAgent Agent;
    public List<Vector3> Waypoints;
    private IState _currentState;
    private int _currentWaypointIndex = 0;
    [HideInInspector]
    public IdleState _idleState = new IdleState();
    [HideInInspector]
    public WalkState _walkState = new WalkState();
    private NPCEnemyAction _npcEnemyAction;

    void Start()
    {
        _currentState = _idleState;
        _currentState.EnterState(this);
        _npcEnemyAction = GetComponent<NPCEnemyAction>();
    }

    void Update()
    {
        if(!_npcEnemyAction.IsCompleted)
            _currentState.UpdateState(this);
    }

    [PunRPC]
    public void ChangeState(IState newState)
    {
        _currentState.ExitState(this);
        _currentState = newState;
        _currentState.EnterState(this);
    }

    [PunRPC]
    public void MovePlayer()
    {
        if (Waypoints.Count > 0)
        {
            Agent.SetDestination(Waypoints[_currentWaypointIndex]);
            _currentWaypointIndex += 1;
            if (_currentWaypointIndex >= Waypoints.Count)
            {
                _currentWaypointIndex = 0;
            }
        }
    }
}
