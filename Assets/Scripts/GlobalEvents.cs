using System;
using UnityEngine;

public sealed class GlobalEvents
{
    private static readonly GlobalEvents _instance = new();

    public static GlobalEvents Instance => _instance;

    private GlobalEvents() { }

    public event Action<Vector2> PlayerMoved;
    public event Action<int> PlayerRotated;
    public event Action<Vector2> StathemMoved;

    public void OnPlayerMoved(Vector2 delta)
    {
        PlayerMoved?.Invoke(delta);
    }

    public void OnPlayerRotated(int delta)
    {
        PlayerRotated?.Invoke(delta);
    }

    public void OnStathemMoved(Vector2 delta)
    {
        StathemMoved?.Invoke(delta);
    }
}
