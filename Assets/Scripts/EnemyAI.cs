using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform Target;
    private NavMeshAgent NavMeshAgent;
    [SerializeField] private Animator Animator;
    private HealthSystem HealthSystem;
    void Start()
    {
        HealthSystem = GetComponent<HealthSystem>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        UpdateTarget();
        InvokeRepeating("UpdateTarget", 3f, .2f);
    }
    void UpdateTarget()
    {
        if (HealthSystem.Dead)
            return;

        NavMeshAgent.destination = Target.position;
    }
    void Update()
    {
        if (HealthSystem.Dead)
            NavMeshAgent.speed = 0;
        Animator.SetFloat("WalkingSpeed", NavMeshAgent.velocity.magnitude);
    }
}
