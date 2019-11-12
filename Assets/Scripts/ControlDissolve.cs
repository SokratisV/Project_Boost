using UnityEngine;

public class ControlDissolve : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            foreach (var item in transform.GetComponentsInChildren<Dissolve>())
            {
                item.StartDissolving();
            }
        }
    }
}
