using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// タイトル画面で走り回っているキャラクター
/// </summary>
public class TitleCharacter : MonoBehaviour
{
    [SerializeField] Transform[] _targets;
    NavMeshAgent _agent;

    int _currentIndex;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        _agent.SetDestination(_targets[0].position);
    }

    void Update()
    {
        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            _currentIndex++;
            _currentIndex = _currentIndex % _targets.Length;

            _agent.SetDestination(_targets[_currentIndex].position);
        }
    }
}
