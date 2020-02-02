using UnityEngine;
using UnityEngine.UI;

public class ArrowMovementUI : MonoBehaviour
{
    [SerializeField] private float perSecond;
    private Image _image;
    private float _fillAmount;

    private void Start()
    {
        _image = GetComponent<Image>();
        _image.fillAmount = 0;
    }

    void Update()
    {
        _fillAmount += Time.deltaTime * perSecond;
        if (_fillAmount > 0.999f)
        {
            if (_fillAmount > 2f)
            {
                _fillAmount = 0;
            }
        }
        else
        {
            _image.fillAmount = _fillAmount;
        }
    }
}