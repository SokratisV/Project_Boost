using System;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    //Move Old
    // [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    // [Range(0, 1)] [SerializeField] float movementFactor;
    // [SerializeField] float period = 2f;
    [SerializeField] float rotationSpeed = 5f, translationSpeed = 1f;
    [SerializeField]
    bool shouldRotate = true, shouldMove = true,
        rotateLocal = false, moveLocal = false,
        rotateX = false, rotateY = true, rotateZ = false,
        translateX = false, translateY = true, translateZ = false;
    [SerializeField] AnimationCurve curve;

    Vector3 startingPos;
    const float tau = Mathf.PI * 2;
    float deltaTime;

    private void Start()
    {
        startingPos = transform.position;
    }

    private void Update()
    {
        deltaTime = Time.deltaTime;
        Move();
        Rotate();
    }

    private void Move()
    {
        if (!shouldMove) return;
        float v = curve.Evaluate(Time.time);
        Vector3 translation = GetTranslationVector(v) * translationSpeed;
        if (moveLocal)
        {
            transform.Translate(translation, Space.Self);
        }
        else
        {
            transform.Translate(translation, Space.World);
        }
    }

    private Vector3 GetTranslationVector(float v)
    {
        return new Vector3(v * deltaTime * ActivateAxis(translateX),
            v * deltaTime * ActivateAxis(translateY),
            v * deltaTime * ActivateAxis(translateZ));
    }

    private void Rotate()
    {
        if (!shouldRotate) return;
        Vector3 rotationVector = GetRotationVector() * rotationSpeed;
        if (rotateLocal)
        {
            transform.Rotate(rotationVector, Space.Self);
        }
        else
        {
            transform.Rotate(rotationVector, Space.World);
        }
    }

    private Vector3 GetRotationVector()
    {
        return new Vector3(deltaTime * ActivateAxis(rotateX),
            deltaTime * ActivateAxis(rotateY),
            deltaTime * ActivateAxis(rotateZ));
    }

    private float ActivateAxis(bool condition)
    {
        if (condition) { return 1; }
        return 0;
    }

    private void OldMove()
    {
        // if (period <= Mathf.Epsilon || !shouldMove) { return; }
        // float cycle = Time.time / period;
        // float rawSinWave = Mathf.Sin(cycle * tau);

        // movementFactor = rawSinWave / 2f + .5f;
        // Vector3 offset = movementFactor * movementVector;
        // transform.position = startingPos + offset;
    }
}
