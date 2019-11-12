using System.Collections;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    [SerializeField] float interval = default;
    [SerializeField] float incrementAmount = default;
    WaitForSeconds _interval = default;
    Material material;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        _interval = new WaitForSeconds(interval);
    }

    public void StartDissolving()
    {
        StartCoroutine(_StartDissolving());
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
