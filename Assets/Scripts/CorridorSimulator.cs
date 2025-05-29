using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorridorSimulator : MonoBehaviour
{
    [SerializeField]
    private int _posX = 0;
    [SerializeField]
    private int _posY = 0;
    [SerializeField]
    private int _angle = 0;
    [SerializeField]
    private int _moveStep = 100;
    [SerializeField]
    private int _rotateStep = 45;
    [SerializeField]
    private Image _currentView;

    private Dictionary<string, Sprite> _spritesDatabase;
    private Vector2 _currentDirection = Vector2.up;

    void Start()
    {
        _spritesDatabase = new Dictionary<string, Sprite>();
        LoadAllSprites();
        UpdateView();
    }

    void Update()
    {
        // Движение вперед/назад
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            Move(1);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            Move(-1);
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
        _posX += Mathf.RoundToInt(_currentDirection.x) * _moveStep * direction;
        _posY += Mathf.RoundToInt(_currentDirection.y) * _moveStep * direction;

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
        _currentDirection = new Vector2(Mathf.Sin(radians), Mathf.Cos(radians));

        UpdateView();
    }

    private void UpdateView()
    {
        string key = $"posX{_posX}_Y{_posY}_{_angle}";

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
        int newX = _posX + Mathf.RoundToInt(_currentDirection.x) * _moveStep * direction;
        int newY = _posY + Mathf.RoundToInt(_currentDirection.y) * _moveStep * direction;

        return _spritesDatabase.ContainsKey($"posX{newX}_Y{newY}_angle{_angle}");
    }
}
