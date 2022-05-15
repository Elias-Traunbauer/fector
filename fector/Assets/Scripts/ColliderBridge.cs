using UnityEngine;

public class ColliderBridge : MonoBehaviour
{
    public int collisionCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        collisionCount++;
    }

    private void OnTriggerExit(Collider other)
    {
        collisionCount--;
    }

    private void OnTriggerStay(Collider other)
    {
    }
}