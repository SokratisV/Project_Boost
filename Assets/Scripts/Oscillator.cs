using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [Range(0, 1)] [SerializeField] float movementFactor;

    Vector3 startingPos;
    [SerializeField] float period = 2f;
    const float tau = Mathf.PI * 2;

    private void Start()
    {
        startingPos = transform.position;
    }

    private void Update()
    {
        if (period <= Mathf.Epsilon) { return; }
        float cycle = Time.time / period;
        float rawSinWave = Mathf.Sin(cycle * tau);

        movementFactor = rawSinWave / 2f + .5f;
        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;
    }
}
