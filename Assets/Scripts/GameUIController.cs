using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private Image _currentView;
    [SerializeField] private PlayerMovement _player;
    [SerializeField] private StathemMovement _stathem;

    private void Awake()
    {
        SpritesDatabase.LoadAllSprites();
    }

    private void OnEnable()
    {
        _player.Moved += OnPlayerMoved;
        _player.Rotated += OnPlayerRotate;
        _stathem.Moved += OnStathemMoved;
    }

    private void OnDisable()
    {
        _player.Moved -= OnPlayerMoved;
        _player.Rotated -= OnPlayerRotate;
        _stathem.Moved -= OnStathemMoved;
    }

    public void OnExitButtonClick()
    {
    #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
    #else
            Application.Quit();
    #endif
    }

    private void UpdateView()
    {
        if (SpritesDatabase.TryGetSprite(_player.Position, _player.Angle, out Sprite newSprite))
        {
            StartCoroutine(SmoothTransition(newSprite));
        }
        else
        {
            Debug.LogWarning($"Sprite not found for position: {_player.Position} {_player.Angle}");
        }
    }

    IEnumerator SmoothTransition(Sprite newSprite)
    {
        yield return new WaitForSeconds(0.5f);

        _currentView.sprite = newSprite;
        _currentView.preserveAspect = true;
        UpdateStathemUI();

        yield return new WaitForSeconds(0.5f);
    }

    private void OnPlayerMoved(Vector2 deltaPosition)
    {
        UpdateView();
    }

    private void OnPlayerRotate(float deltaRotation)
    {
        UpdateView();
    }

    private void OnStathemMoved(Vector2 vector)
    {
        UpdateStathemUI();
    }

    private void UpdateStathemUI()
    {
        float distance = PlayerStathemDistance();
        _stathem.UIComponent.UpdateSprite(distance);
    }

    private int PlayerStathemDistance()
    {
        return Mathf.RoundToInt((_player.Position - _stathem.Position).magnitude);
    }
}
