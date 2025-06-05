using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CorridorSimulator : MonoBehaviour
{
    [SerializeField]
    private Vector2 _position = new();
    [SerializeField]
    private int _angle = 0;
    [SerializeField]
    private int _moveStep = 1;
    [SerializeField]
    private int _rotateStep = 90;
    [SerializeField]
    private Image _currentView;
    [SerializeField]
    private Vector2 _stathemPointA = new(4, 0);
    [SerializeField]
    private Vector2 _stathemPointB = new(0, 0);
    [SerializeField]
    private float _stathemStepDelay = 2f;
    [SerializeField]
    private Image _stathemImage;
    [SerializeField]
    private Sprite _stathemFaceSprite;
    [SerializeField]
    private Sprite _stathemBackSprite;


    private Vector2 _stathemCoords;
    private Vector2 _stathemTargetPoint;
    private Dictionary<string, Sprite> _spritesDatabase;
    private Vector2 _currentDirection = Vector2.right;

    void Start()
    {
        _stathemCoords = _stathemPointA;
        _stathemTargetPoint = _stathemPointB;
        _spritesDatabase = new Dictionary<string, Sprite>();
        LoadAllSprites();
        UpdateView();
        SetStathem();
        StartCoroutine(StathemMovement());
    }

    void Update()
    {
        // Движение вперед/назад
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            if (CanMove(1))
            {
                Move(1);
            }
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (CanMove(-1))
            {
                Move(-1); 
            }
        }

        // Поворот
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            Rotate(-1);
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            Rotate(1);
        }

        UpdateStathem();
    }

    private void LoadAllSprites()
    {
        _spritesDatabase = new Dictionary<string, Sprite>();

        string[] positionFolders = System.IO.Directory.GetDirectories(Application.dataPath + "/Resources/Sprites/");

        foreach (string folderPath in positionFolders)
        {
            string folderName = System.IO.Path.GetFileName(folderPath);

            Sprite[] angleSprites = Resources.LoadAll<Sprite>("Sprites/" + folderName);

            foreach (Sprite sprite in angleSprites)
            {
                string angleStr = sprite.name.Replace($"{folderName}_angle", "");

                if (int.TryParse(angleStr, out int angleValue))
                {
                    // Ключ в словаре: "posX0_Y0_45"
                    string key = $"{folderName}_{angleValue}";
                    _spritesDatabase[key] = sprite;
                }
            }
        }
    }

    private void Move(int direction)
    {
        _position.x += Mathf.RoundToInt(_currentDirection.x) * _moveStep * direction;
        _position.y += Mathf.RoundToInt(_currentDirection.y) * _moveStep * direction;

        UpdateView();
    }

    private void Rotate(int direction)
    {
        _angle += _rotateStep * direction;

        if (_angle >= 360)
        {
            _angle -= 360;
        }

        if (_angle < 0)
        {
            _angle += 360;
        }

        float radians = _angle * Mathf.Deg2Rad;
        _currentDirection = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

        UpdateView();
    }

    private void UpdateView()
    {
        string key = $"posX{_position.x}_Y{_position.y}_{_angle}";

        if (_spritesDatabase.TryGetValue(key, out Sprite newSprite))
        {
            StartCoroutine(SmoothTransition(newSprite));
        }
        else
        {
            Debug.LogWarning($"Sprite not found for position: {key}");
        }
    }

    IEnumerator SmoothTransition(Sprite newSprite)
    {
        yield return new WaitForSeconds(0.5f);
        _currentView.sprite = newSprite;
        _currentView.preserveAspect = true;
        yield return new WaitForSeconds(0.5f);
    }

    private bool CanMove(int direction)
    {
        int newX = Convert.ToInt32(_position.x) + Mathf.RoundToInt(_currentDirection.x) * _moveStep * direction;
        int newY = Convert.ToInt32(_position.y) + Mathf.RoundToInt(_currentDirection.y) * _moveStep * direction;

        return _spritesDatabase.ContainsKey($"posX{newX}_Y{newY}_{_angle}");
    }

    public void OnExitButtonClick()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    private IEnumerator StathemMovement()
    {
        while (true)
        {
            yield return new WaitForSeconds(_stathemStepDelay);

            Vector2 direction = (_stathemTargetPoint - _stathemCoords).normalized;
            _stathemCoords += direction;

            if (_stathemCoords == _stathemPointA)
            {
                _stathemTargetPoint = _stathemPointB;
            }
            else if (_stathemCoords == _stathemPointB)
            {
                _stathemTargetPoint = _stathemPointA;
            }

            Debug.Log($"Текущая конечная точка: {_stathemTargetPoint}\n Координаты: {_stathemCoords}\n Направление: {direction}");
        }
    }

    private int StathemDistance()
    {
        return Mathf.RoundToInt((_stathemCoords - _position).magnitude);
    }

    private float StathemSpriteScale()
    {
        int distance = StathemDistance();
        return distance switch
        {
            0 => 20,
            1 => 4.5f,
            2 => 2f,
            3 => 1.4f,
            4 => 1f,
            _ => throw new Exception("Куда-то ушел"),
        };
    }

    private void SetStathem()
    {
        _stathemImage.preserveAspect = true;
        _stathemImage.rectTransform.anchoredPosition = Vector2.zero;
        _stathemImage.rectTransform.sizeDelta = new(100f, 100f);
        _stathemImage.sprite = _stathemFaceSprite;
        _stathemImage.enabled = true;

    }

    private void UpdateStathem()
    {
        float koeff = StathemSpriteScale();
        _stathemImage.rectTransform.localScale = new Vector2(1f * koeff, 1f * koeff);
    }
}