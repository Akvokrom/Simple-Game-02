using UnityEngine;
using UnityEngine.AI;

public class EnemyFast : MonoBehaviour
{
    NavMeshAgent nav;
    Transform player;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
        nav.SetDestination(player.position);
    }

    void Update()
    {
        if (player != null)
        {
            nav.SetDestination(player.position);
        }     
    }
}
