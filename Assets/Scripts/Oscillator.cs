using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [Range(0, 1)] [SerializeField] float movementFactor;

    Vector3 _startingPos;
    [SerializeField] float period = 2f;
    const float Tau = Mathf.PI * 2;

    private void Start()
    {
        _startingPos = transform.position;
    }

    private void Update()
    {
        if (period <= Mathf.Epsilon)
        {
            return;
        }

        var cycle = Time.time / period;
        var rawSinWave = Mathf.Sin(cycle * Tau);

        movementFactor = rawSinWave / 2f + .5f;
        var offset = movementFactor * movementVector;
        transform.position = _startingPos + offset;
    }
}