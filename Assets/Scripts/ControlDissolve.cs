using UnityEngine;

public class ControlDissolve : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Friendly")) return;
        if (other.gameObject.CompareTag("Finish"))
        {
            foreach (var item in transform.GetComponentsInChildren<Dissolve>())
            {
                item.StartDissolving(Dissolve.Colors.Green);
                print("Dissolving");
            }
        }
        else
        {
            foreach (var item in transform.GetComponentsInChildren<Dissolve>())
            {
                item.StartDissolving(Dissolve.Colors.Red);
                print("Dissolving");
            }
        }
    }
}
