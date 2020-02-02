using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    [SerializeField] float interval;
    [SerializeField] float incrementAmount;

    public enum Colors
    {
        Red,
        Green
    }

    Dictionary<Colors, Color> _colors;
    WaitForSeconds _interval = default;
    Material _material;
    Coroutine _dissolveCoroutine;
    private static readonly int Effect = Shader.PropertyToID("_Effect");

    private void Start()
    {
        _material = GetComponent<Renderer>().material;
        _interval = new WaitForSeconds(interval);
        _colors = new Dictionary<Colors, Color>();
        _colors.Add(Colors.Green, new Color(0, 191, 30));
        _colors.Add(Colors.Red, new Color(191, 1, 0));
    }

    public void StartDissolving(Colors color)
    {
        if (_dissolveCoroutine == null)
        {
            _colors.TryGetValue(color, out var tempColor);
            _material.SetColor("_edgeColor", tempColor);
            _dissolveCoroutine = StartCoroutine(_StartDissolving());
        }
    }

    private IEnumerator _StartDissolving()
    {
        float incrementValue = 0;
        while (true)
        {
            _material.SetFloat(Effect, incrementValue += incrementAmount);
            if (incrementValue >= 1) break;
            yield return _interval;
        }
    }
}