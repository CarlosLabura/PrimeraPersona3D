using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform Target;
    void Update()
    {
        GetComponent<NavMeshAgent>().destination = Target.position;
    }
}
