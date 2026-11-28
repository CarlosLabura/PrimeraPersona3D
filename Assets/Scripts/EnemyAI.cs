using UnityEngine;
using UnityEngine.AI;
public enum EnemyState { Patrol, Following }
[RequireComponent(typeof(HealthSystem))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
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

    private void Awake()
    {
        HealthSystem = GetComponent<HealthSystem>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
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
    }
    private void UpdateStateMachine()
    {
        if (HealthSystem.IsDead || Target == null)
            return;

        float sqrDistance = (Target.position - transform.position).sqrMagnitude;
        switch (State)
        {
            case EnemyState.Patrol:
                if (sqrDistance <= DetectionRange * DetectionRange)
                    ChangeState(EnemyState.Following);
                break;

            case EnemyState.Following:
                if (sqrDistance > LoseRange * LoseRange)
                    ChangeState(EnemyState.Patrol);
                else
                    NavMeshAgent.destination = Target.position;
                break;
        }
    }
    private void ChangeState(EnemyState newState)
    {
        State = newState;
        switch (newState)
        {
            case EnemyState.Patrol:
                NavMeshAgent.ResetPath();
                break;
            case EnemyState.Following:
                NavMeshAgent.destination = Target.position;
                break;
        }
    }
    private void EnemyDeath()
    {
        CancelInvoke(nameof(UpdateStateMachine));
        NavMeshAgent.ResetPath();
        ModelAnimator.SetBool(DeadAnim, true);
        Debug.Log($"Playing {name} Death Animation");
    }
}