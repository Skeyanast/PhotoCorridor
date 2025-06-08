using UnityEngine;
using UnityEngine.UI;

public class StathemUI : MonoBehaviour
{
    [SerializeField] private Image _stathemImage;
    [SerializeField] private Sprite _stathemFaceSprite;
    [SerializeField] private Sprite _stathemBackSprite;
    [SerializeField] private AnimationCurve _spriteScaleCurve;

    private void Start()
    {
        InitialSpriteSet();
    }

    public void UpdateSprite(float distance)
    {
        float koeff = SpriteScale(distance);
        _stathemImage.rectTransform.localScale = new Vector2(1f * koeff, 1f * koeff);
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
}
