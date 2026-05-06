using UnityEngine;

public class GhostAI : MonoBehaviour
{
    public float speed = 3f;
    public float damageCooldown = 1f;

    private Transform target;
    private float lastDamageTime;

    public bool gameStarted = false;

    void Update()
    {
        if (!gameStarted) return;
        FindClosestPlayer();

        if (target != null)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime
            );
        }
    }

    void FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        float minDistance = Mathf.Infinity;
        Transform closest = null;

        foreach (GameObject p in players)
        {
            HeartHealth hp = p.GetComponent<HeartHealth>();


            if (hp == null || !hp.IsAlive())
                continue;

            float dist = Vector2.Distance(transform.position, p.transform.position);

            if (dist < minDistance)
            {
                minDistance = dist;
                closest = p.transform;
            }
        }

        target = closest;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!gameStarted) return;

        if (!Unity.Netcode.NetworkManager.Singleton.IsServer) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time - lastDamageTime > damageCooldown)
            {
                HeartHealth hp = collision.gameObject.GetComponent<HeartHealth>();

                if (hp != null && hp.IsAlive())
                {
                    hp.TakeDamage(1);
                    lastDamageTime = Time.time;
                }
            }
        }
    }
}