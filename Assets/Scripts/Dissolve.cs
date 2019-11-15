using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    [SerializeField] float interval = default;
    [SerializeField] float incrementAmount = default;
    public enum Colors { Red, Green };
    Dictionary<Colors, Color> colors;
    WaitForSeconds _interval = default;
    Material material;
    Coroutine dissolveCoroutine;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        _interval = new WaitForSeconds(interval);
        colors = new Dictionary<Colors, Color>();
        colors.Add(Colors.Green, new Color(0, 191, 30));
        colors.Add(Colors.Red, new Color(191, 1, 0));
    }

    public void StartDissolving(Colors color)
    {
        if (dissolveCoroutine == null)
        {
            Color tempColor;
            colors.TryGetValue(color, out tempColor);
            material.SetColor("_edgeColor", tempColor);
            dissolveCoroutine = StartCoroutine(_StartDissolving());
        }
    }

    private IEnumerator _StartDissolving()
    {
        float incrementValue = 0;
        while (true)
        {
            material.SetFloat("_Effect", incrementValue += incrementAmount);
            if (incrementValue >= 1) break;
            yield return _interval;
        }
    }
}
