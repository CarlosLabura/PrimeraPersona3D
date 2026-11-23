using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(HealthSystem))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    // Inspector Variables
    public Transform Target;
    [SerializeField] private Animator ModelAnimator;

    // Needed Components 
    private HealthSystem HealthSystem;
    private NavMeshAgent NavMeshAgent;
    private void Awake()
    {
        HealthSystem = GetComponent<HealthSystem>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void UpdateTarget()
    {
        if (HealthSystem.Dead)
            return;

        NavMeshAgent.destination = Target.position;
    }
    //https://github.com/microsoft/Microsoft.Unity.Analyzers/blob/main/doc/UNT0016.md
    private void Start()
    {
        UpdateTarget();
        InvokeRepeating(nameof(UpdateTarget), 3f, .2f);
    }
    //https://github.com/microsoft/Microsoft.Unity.Analyzers/blob/main/doc/UNT0041.md
    private static readonly int SpeedHash = Animator.StringToHash("WalkingSpeed");
    private void Update()
    {
        if (HealthSystem.Dead)
            NavMeshAgent.speed = 0;
        ModelAnimator.SetFloat(SpeedHash, NavMeshAgent.velocity.magnitude);
    }
}
