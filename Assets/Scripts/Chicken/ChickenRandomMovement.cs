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
        float startPosY;
        Vector3 targetPositon;
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
            startPosY = transform.position.y;
        }

        // Update is called once per frame
        void Update()
        {

            if (agent.pathStatus == NavMeshPathStatus.PathPartial || 
                agent.pathStatus == NavMeshPathStatus.PathInvalid || 
                !agent.hasPath)
            {
                if (IsWaitingTimeOver()) {
                    Vector3 destination = GetRandomVector();
                    targetRotation = Quaternion.LookRotation(destination - transform.position, Vector3.up);
                    targetPositon = transform.position;
                    targetPositon.y = 1;
                }

                if (transform.rotation == targetRotation) {
                    agent.destination = GetRandomVector();
                } else {
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.05f);
                }

                if (transform.position != targetPositon) {
                    Vector3 velocity = Vector3.zero;
                    targetPositon = Vector3.SmoothDamp(transform.position, targetPositon, ref velocity, 1.0f, 1.0f);
                } else {
                    
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