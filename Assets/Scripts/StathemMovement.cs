using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(StathemUI))]
public class StathemMovement : MonoBehaviour
{
    [SerializeField] private Vector2 _pointA = new(4, 0);
    [SerializeField] private Vector2 _pointB = new(0, 0);
    [SerializeField] private float _stepDelay = 2f;

    private Vector2 _currentPosition;
    private Vector2 _currentTargetPoint;

    public event Action<Vector2> Moved;

    public Vector2 Position => _currentPosition;
    public Vector2 Direction => (_currentTargetPoint - _currentPosition).normalized;
    public StathemUI UIComponent { get; private set; }

    private void Awake()
    {
        UIComponent = GetComponent<StathemUI>();
    }

    private void Start()
    {
        _currentPosition = _pointA;
        _currentTargetPoint = _pointB;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (true)
        {
            yield return new WaitForSeconds(_stepDelay);

            Vector2 direction = Direction;
            _currentPosition += direction;

            if (_currentPosition == _pointA)
            {
                _currentTargetPoint = _pointB;
            }
            else if (_currentPosition == _pointB)
            {
                _currentTargetPoint = _pointA;
            }

            Moved?.Invoke(Direction);

            //Debug.Log($"Текущая конечная точка: {_currentTargetPoint}\n Координаты: {_currentPosition}\n Направление: {direction}");
        }
    }
}
