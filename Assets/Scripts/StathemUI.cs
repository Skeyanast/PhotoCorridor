using UnityEngine;
using UnityEngine.UI;

public class StathemUI : MonoBehaviour
{
    [SerializeField] private Image _stathemImage;
    [SerializeField] private Sprite _stathemFaceSprite;
    [SerializeField] private Sprite _stathemBackSprite;
    [SerializeField] private AnimationCurve _spriteScaleCurve;
    [SerializeField] private AnimationCurve _spritePositionCurve;

    private void Start()
    {
        InitialSpriteSet();
    }

    public void UpdateSprite(float distance, bool visibility)
    {
        float scaleKoeff = SpriteScale(distance);
        float positionY = SpritePositionY(distance);

        _stathemImage.rectTransform.localScale = new Vector2(1f * scaleKoeff, 1f * scaleKoeff);
        _stathemImage.rectTransform.anchoredPosition = new Vector3(0f, positionY);
        _stathemImage.enabled = visibility;
    }

    public void SetFaceSprite()
    {
        _stathemImage.sprite = _stathemFaceSprite;
    }

    public void SetBackSprite()
    {
        _stathemImage.sprite = _stathemBackSprite;
    }

    private void InitialSpriteSet()
    {
        _stathemImage.preserveAspect = true;
        _stathemImage.rectTransform.anchoredPosition = Vector2.zero;
        _stathemImage.rectTransform.sizeDelta = new(100f, 100f);
        _stathemImage.sprite = _stathemFaceSprite;
        _stathemImage.enabled = true;
    }

    private float SpriteScale(float distance)
    {
        return _spriteScaleCurve.Evaluate(distance);
    }

    private float SpritePositionY(float distance)
    {
        return _spritePositionCurve.Evaluate(distance);
    }
}
