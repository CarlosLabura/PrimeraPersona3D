using UnityEngine;
using UnityEngine.AI;
public enum EnemyType { Walker, Shooter, Flyer }
public enum EnemyState { Patrol, Following, Dead }
[RequireComponent(typeof(HealthSystem))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public EnemyType Type = EnemyType.Walker;
    public EnemyState State;

    [Header("Enemy Sight")]
    [Min(0f)] public float DetectionRange = 10f;
    [Min(0f)] public float LoseRange = 15f;

    [Header("Objects Needed")]
    public Transform Target;
    public Animator ModelAnimator;

    // Internal Variables
    //https://github.com/microsoft/Microsoft.Unity.Analyzers/blob/main/doc/UNT0041.md
    private static readonly int DeadAnim = Animator.StringToHash("Dead");
    private static readonly int SpeedAnim = Animator.StringToHash("WalkingSpeed");

    // Needed Components
    private HealthSystem HealthSystem;
    private NavMeshAgent NavMeshAgent;
    private ShootingSystem ShootingSystem;

    private void Awake()
    {
        HealthSystem = GetComponent<HealthSystem>();
        NavMeshAgent = GetComponent<NavMeshAgent>();

        if (Type == EnemyType.Shooter)
            ShootingSystem = GetComponent<ShootingSystem>();
    }
    //https://github.com/microsoft/Microsoft.Unity.Analyzers/blob/main/doc/UNT0016.md
    private void Start()
    {
        HealthSystem.OnEnemyDeath += EnemyDeath;
        InvokeRepeating(nameof(UpdateStateMachine), 0f, .2f);
    }
    private void OnDestroy()
    {
        HealthSystem.OnEnemyDeath -= EnemyDeath;
    }
    private void Update()
    {
        if (HealthSystem.IsDead)
            NavMeshAgent.speed = 0;

        ModelAnimator.SetFloat(SpeedAnim, NavMeshAgent.velocity.magnitude);

        if (Type == EnemyType.Shooter && State == EnemyState.Following && !NavMeshAgent.pathPending && NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance)
        {
            transform.LookAt(new Vector3(Target.position.x, transform.position.y, Target.position.z));
            ShootingSystem.Shoot();
        }
    }
    private void UpdateStateMachine()
    {
        if (HealthSystem.IsDead || Target == null)
            return;

        float distance = Vector3.Distance(transform.position, Target.position);
        switch (State)
        {
            case EnemyState.Patrol:
                if (distance <= DetectionRange)
                    State = EnemyState.Following;
                break;
            case EnemyState.Following:
                if (distance > LoseRange) {
                    State = EnemyState.Patrol;
                    NavMeshAgent.ResetPath();
                } else {
                    NavMeshAgent.destination = Target.position;
                }
                break;
        }
    }
    private void EnemyDeath()
    {
        CancelInvoke(nameof(UpdateStateMachine));
        NavMeshAgent.ResetPath();
        State = EnemyState.Dead;
        ModelAnimator.SetBool(DeadAnim, true);
        Debug.Log($"Playing {name} Death Animation");
    }
}