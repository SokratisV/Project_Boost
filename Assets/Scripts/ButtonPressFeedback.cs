using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressFeedback : MonoBehaviour
{
    [SerializeField] private MyKeyCodes keyCode;
    
    private Image _image;
    private KeyCode _targetKeyCode;

    enum MyKeyCodes
    {
        W,
        A,
        D,
        Space,
        Up,
        Left,
        Right
    }

    private Dictionary<MyKeyCodes, KeyCode> _myKeyCodes = new Dictionary<MyKeyCodes, KeyCode>
    {
        {MyKeyCodes.W, KeyCode.W},
        {MyKeyCodes.A, KeyCode.A},
        {MyKeyCodes.D, KeyCode.D},
        {MyKeyCodes.Space, KeyCode.Space},
        {MyKeyCodes.Up, KeyCode.UpArrow},
        {MyKeyCodes.Left, KeyCode.LeftArrow},
        {MyKeyCodes.Right, KeyCode.RightArrow},
    };

    private void Start()
    {
        _image = GetComponent<Image>();
        _myKeyCodes.TryGetValue(keyCode, out _targetKeyCode);
    }

    private void Update()
    {
        if (Input.GetKeyDown(_targetKeyCode))
        {
            _image.color = Color.green;
        }
        else if (Input.GetKeyUp(_targetKeyCode))
        {
            _image.color = Color.white;
        }
    }
}