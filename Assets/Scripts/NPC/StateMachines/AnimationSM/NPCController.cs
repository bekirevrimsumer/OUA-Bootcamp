using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
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

    void Start()
    {
        _currentState = _idleState;
        _currentState.EnterState(this);
    }

    void Update()
    {
        _currentState.UpdateState(this);
    }

    public void ChangeState(IState newState)
    {
        _currentState.ExitState(this);
        _currentState = newState;
        _currentState.EnterState(this);
    }

    public void MovePlayer()
    {
        if (Waypoints.Count > 0)
        {
            var waypointIndex = Random.Range(0, Waypoints.Count);
            if (waypointIndex == _currentWaypointIndex)
            {
                waypointIndex = (waypointIndex + 1) % Waypoints.Count;
            }
            _currentWaypointIndex = waypointIndex;

            Debug.Log("Moving to waypoint: " + _currentWaypointIndex);

            Agent.SetDestination(Waypoints[_currentWaypointIndex]);
        }
    }
}
