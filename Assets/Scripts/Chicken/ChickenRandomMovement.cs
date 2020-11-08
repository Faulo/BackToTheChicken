using UnityEngine;
using UnityEngine.AI;


namespace Runtime.Chicken {
    public class ChickenRandomMovement : MonoBehaviour
    {
        [SerializeField, Range(-20, 20)]
        float randomPosMinX = -10;
        [SerializeField, Range(-20, 20)]
        float randomPosMinZ = -10;
        [SerializeField, Range(-20, 20)]
        float randomPosMaxX = 10;
        [SerializeField, Range(-20, 20)]
        float randomPosMaxZ = 10;
        [SerializeField, Range(2, 10)]
        float waitingTimeMin = 2;
        [SerializeField, Range(2, 10)]
        float waitingTimeMax = 5;


        NavMeshAgent agent;
        float lastTimeUpdate = -1;
        float currentWaitingTime = -1;
        Quaternion targetRotation;

        void Awake() {
            if (waitingTimeMin > waitingTimeMax) {
                Debug.LogError("The minimum waiting time must be smaller or equal to the maximum waiting time!");
            }     
        }

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.destination = GetRandomVector();
        }

        // Update is called once per frame
        void Update()
        {

            if (agent.pathStatus == NavMeshPathStatus.PathPartial || 
                agent.pathStatus == NavMeshPathStatus.PathInvalid || 
                !agent.hasPath)
            {
                if (IsWaitingTimeOver()) {
                    var destination = GetRandomVector();
                    targetRotation = Quaternion.LookRotation(destination - transform.position, Vector3.up);
                }

                if (transform.rotation == targetRotation) {
                    agent.destination = GetRandomVector();
                } else {
                    var rigidbody = GetComponent<Rigidbody>();
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.05f);
                }
                
            }
        }

        

        bool IsWaitingTimeOver() {
            if (lastTimeUpdate < 0) {
                lastTimeUpdate = Time.time;
                currentWaitingTime = Random.Range(waitingTimeMin, waitingTimeMax);
                Debug.Log(currentWaitingTime);

            } else if(Time.time - lastTimeUpdate >= currentWaitingTime) {
                lastTimeUpdate = currentWaitingTime = -1;
                return true;
            }

            return false;
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