using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Transform[] _pointsToMove;
    [SerializeField] private float _displacementSpeed;
    [SerializeField] private float _timeToResponse;
    [SerializeField] private bool _canMove;

    private void Start()
    {
        if (_canMove && _pointsToMove != null) 
            StartCoroutine(MovingIntoPoints());
    }

    private IEnumerator MovingIntoPoints()
    {
        int actualPosition = 0;
        
        while (_canMove)
        {
            if (Vector2.Distance(transform.position, _pointsToMove[actualPosition].position) >= 0.5f)
            {
                transform.position = Vector2.Lerp(transform.position, _pointsToMove[actualPosition].position,
                    _displacementSpeed * Time.deltaTime);
            }
            else
            {
                // Se vuelve a ubicar en el primer punto, si alcanza el máximo de ubicaciones
                actualPosition = actualPosition < _pointsToMove.Length - 1 ? actualPosition + 1 : 0;
                yield return new WaitForSeconds(_timeToResponse);
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
