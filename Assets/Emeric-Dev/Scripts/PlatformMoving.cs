using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMoving : Platform
{
    enum PathingType { loop, path}
    
    [SerializeField] Transform[] _nodes;
    [Tooltip("The way the platform will interact with its path.\nLoop: When reaching the last node, travel back to the first.\nPath: When reaching the last node, travel back in the opposite direction.")]
    [SerializeField] PathingType pathingType = default;
    [SerializeField] int _startingNodeIndex = 0;
    [SerializeField] float _moveSpeed = 1f;
    [SerializeField] float _distanceToChangeNode = 0.01f;
    int _targetNodeIndex;
    bool _movingForward = true;

    void Start()
    {
        if (_nodes.Length > 0){
            if (_startingNodeIndex >= 0 && _startingNodeIndex < _nodes.Length){
                transform.position = _nodes[_startingNodeIndex].position;
                ChangeTargetNode(_startingNodeIndex);
            }
        }
    }

    void Update()
    {
        Vector3 targetPos = _nodes[_targetNodeIndex].position;

        if (Vector3.Distance(transform.position, targetPos) < _distanceToChangeNode){
            ChangeTargetNode(_targetNodeIndex);
            targetPos = _nodes[_targetNodeIndex].position;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPos, _moveSpeed * Time.deltaTime);
    }

    void ChangeTargetNode(int currentTarget){
        switch (pathingType) {
            case PathingType.loop: _targetNodeIndex = NextNodeLoop(currentTarget); break;
            case PathingType.path: _targetNodeIndex = NextNodePath(currentTarget); break;
        }
    }

    int NextNodePath(int currentTarget){
        if (currentTarget >= (_nodes.Length - 1)){
            _movingForward = false;
        } else if (currentTarget <= 0){
            _movingForward = true;
        }

        if (_movingForward){
            currentTarget++;
        } else {
            currentTarget--;
        }

        return currentTarget;
    }

    int NextNodeLoop(int currentTarget){
        if (currentTarget >= (_nodes.Length - 1)){
            currentTarget = 0;
        } else {
            currentTarget++;
        }
        return currentTarget;
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.green;
        foreach (Transform node in _nodes){
            Gizmos.DrawSphere(node.position, 1f);
        }

        if (pathingType == PathingType.loop){
            for (int i = 0; i < (_nodes.Length); i++){
                if (i == _nodes.Length - 1){
                    Gizmos.DrawLine(_nodes[i].position, _nodes[0].position);
                } else {
                    Gizmos.DrawLine(_nodes[i].position, _nodes[i + 1].position);
                }
            }
        } else {
            for (int i = 0; i < (_nodes.Length - 1); i++){
                Gizmos.DrawLine(_nodes[i].position, _nodes[i + 1].position);
            }
        }
    }
}
