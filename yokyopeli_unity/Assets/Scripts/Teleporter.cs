using UnityEngine;
using Q3Movement;
using EasyTransition;

public class TeleportTrigger : MonoBehaviour
{
    public Transform teleportDestination;
    public TransitionSettings transition;
    private Q3Movement.Q3PlayerController controller;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            print("OTHER ENTERED");
            // Teleport the player to the destination
            controller = other.GetComponent<Q3Movement.Q3PlayerController>();
            controller.Teleport(teleportDestination.transform.position);
            TransitionManager.Instance().Transition(transition, 0.0f);

    }
    }
    private void OnDrawGizmos()
    {
        if (teleportDestination != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(teleportDestination.position, 0.5f);
        }
    }
}
