using UnityEngine;
using Q3Movement;
public class BillboardAndDamage : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int damageAmount = 10;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player has the tag 'Player'.");
        }
    }

    void Update()
    {
        // Rotate to face the player
        transform.LookAt(player);

        // Move towards the player
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DealDamage(other.gameObject);
            Destroy(gameObject); // Destroy the billboard upon collision with player
        }
    }

    void DealDamage(GameObject player)
    {
        Q3PlayerController playerController = player.GetComponent<Q3PlayerController>();
        playerController.DealDamage(damageAmount, 1);
    }
}
