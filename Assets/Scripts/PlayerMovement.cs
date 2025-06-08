using System;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector2 _position = Vector2.zero;
    [SerializeField] private float _angle = 0f;
    [SerializeField] private float _moveStep = 1f;
    [SerializeField] private float _rotateStep = 90f;

    private PlayerInput _playerInput;
    private Vector2 _currentForwardDirection;
    private Vector2 _currentBackwardDirection;

    public event Action<Vector2> Moved;
    public event Action<float> Rotated;

    public Vector2 Position => _position;
    public float Angle => _angle;
    public Vector2 CurrentForwardDirection => _currentForwardDirection;
    public Vector2 CurrentBackwardDirection => _currentBackwardDirection;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        _playerInput.MoveForwardPressed += () => Move(_currentForwardDirection);
        _playerInput.MoveBackwardPressed += () => Move(_currentBackwardDirection);
        _playerInput.RotateLeftPressed += () => Rotate(false);
        _playerInput.RotateRightPressed += () => Rotate(true);
    }

    private void OnDisable()
    {
        _playerInput.MoveForwardPressed -= () => Move(_currentForwardDirection);
        _playerInput.MoveBackwardPressed -= () => Move(_currentBackwardDirection);
        _playerInput.RotateLeftPressed -= () => Rotate(false);
        _playerInput.RotateRightPressed -= () => Rotate(true);
    }

    private void Start()
    {
        UpdateDirectionVectors();
    }

    private void Move(Vector2 direction)
    {
        if (CanMove(direction))
        {
            Vector2 delta = direction * _moveStep;

            _position += delta;

            Moved?.Invoke(delta);
        }
    }

    private bool CanMove(Vector2 direction)
    {
        Vector2 step = direction * _moveStep;
        Vector2 nextPosition = _position + step;

        return SpritesDatabase.ContainsSprite(nextPosition, _angle);
    }

    private void Rotate(bool isClockwise)
    {
        int direction = isClockwise ? 1 : -1;
        float deltaAngle = _rotateStep * direction;
        float newAngle = Mathf.Repeat(_angle + deltaAngle, 360f);

        if (MathF.Abs(newAngle - _angle) > float.Epsilon)
        {
            _angle = newAngle;
            UpdateDirectionVectors();
            Rotated?.Invoke(deltaAngle);
        }
    }

    private void UpdateDirectionVectors()
    {
        float radians = _angle * Mathf.Deg2Rad;
        _currentForwardDirection = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        _currentBackwardDirection = -_currentForwardDirection;
    }
}
