using UnityEngine;
using UnityEngine.AI;

public class EnemyGhost : MonoBehaviour
{
    NavMeshAgent nav;
    MapGenerator MapGenerator;
    Vector3 randomPos;
    Vector3 distenceToPos;
    Transform player;

    private float noMovementThreshold = 0.0001f;
    private const int noMovementFrames = 2;
    Vector3[] previousLocations = new Vector3[noMovementFrames];
    private bool isMoving;

    float timer;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        MapGenerator = GameObject.Find("GameController").gameObject.GetComponent<MapGenerator>();
        player = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
        setrandompath();
        timer = Random.Range(5, 15);

        //For good measure, set the previous locations
        for (int i = 0; i < previousLocations.Length; i++)
        {
            previousLocations[i] = Vector3.zero;
        }

    }

    void Update()
    {
        if (timer > 0 && player != null)
        {
            timer -= Time.deltaTime;
            distenceToPos = transform.position - randomPos;

            for (int i = 0; i < previousLocations.Length - 1; i++)
            {
                previousLocations[i] = previousLocations[i + 1];
            }
            previousLocations[previousLocations.Length - 1] = transform.position;

            for (int i = 0; i < previousLocations.Length - 1; i++)
            {
                if (Vector3.Distance(previousLocations[i], previousLocations[i + 1]) >= noMovementThreshold)
                {
                    //The minimum movement has been detected between frames
                    isMoving = true;
                    break;
                }
                else
                {
                    isMoving = false;
                }
            }

            if (timer <= 0 || (distenceToPos.x < 2 && distenceToPos.z < 2) || !isMoving)
            {
                setrandompath();
                timer = Random.Range(5, 15);
            }
        }
    }

    void setrandompath()
    {
        bool pathSelected = false;

        while (!pathSelected)
        {
            randomPos = new Vector3(Random.Range(MapGenerator.mapLeft + 1, MapGenerator.mapRight + 1), 0, Random.Range(MapGenerator.mapDown + 1, MapGenerator.mapUp + 1));

            Ray ray = new Ray(new Vector3(randomPos.x, 2, randomPos.z), Vector3.down);
            Physics.Raycast(ray, out RaycastHit hit);

            //Check if cell don't empty or wall
            if (hit.collider != null && hit.collider.gameObject.tag != "Wall")
            {
                pathSelected = true;
            }
        }

        nav.SetDestination(randomPos);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            setrandompath();
        }
    }
}
