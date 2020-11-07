using UnityEngine;
using UnityEngine.AI;


namespace Runtime.Chicken {
    public class ChickenRandomMovement : MonoBehaviour
    {
        [SerializeField, Range(-20, 20)]
        float randomPosMinX;
        [SerializeField, Range(-20, 20)]
        float randomPosMinZ;
        [SerializeField, Range(-20, 20)]
        float randomPosMaxX;
        [SerializeField, Range(-20, 20)]
        float randomPosMaxZ;

        NavMeshAgent agent;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {

            if (agent.pathStatus == NavMeshPathStatus.PathPartial || 
                agent.pathStatus == NavMeshPathStatus.PathInvalid || 
                !agent.hasPath)
            {
                agent.destination = GetRandomVector();
            }
        }

        Vector3 GetRandomVector()
        {
            return new Vector3(
                Random.Range(randomPosMinX, randomPosMaxX), transform.position.y, 
                Random.Range(randomPosMinZ, randomPosMaxZ)
            );
        }
    }
}