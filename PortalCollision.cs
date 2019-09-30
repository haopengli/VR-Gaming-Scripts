using UnityEngine;

public class PortalCollision : MonoBehaviour
{
    public bool collided = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "[VRTK][AUTOGEN][BodyColliderContainer]")
        {
            collided = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collided = false;
    }
}