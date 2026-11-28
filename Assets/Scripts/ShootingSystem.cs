using UnityEngine;
using UnityEngine.InputSystem;
public class ShootingSystem : MonoBehaviour
{
    [Header("Shooting Settings")]
    public float ProjectileSpeed = 15f;
    public float ShootingDelay = 0.3f;
    public float DestroyTime = 3f;
    public int SuperShots = 0;
    public float AnimationTime = 15f;

    [Header("Objects Needed")]
    public GameObject Projectile;
    public Transform SpawnPoint;
    [ShowIf(typeof(HealthSystem), "IsPlayer")] public Transform GunModel;
    [ShowIf(typeof(HealthSystem), "IsPlayer")] public Transform FastGunModel;

    [Header("Script Data")]
    [ReadOnly] public float DelayTimer;
    
    // Components Needed
    private HealthSystem HealthSystem;
    private StarterAssets.StarterAssetsInputs StarterAssetsInputs;
    private void Awake()
    {
        HealthSystem = GetComponent<HealthSystem>();

        if (HealthSystem.IsPlayer)
            StarterAssetsInputs = GetComponent<StarterAssets.StarterAssetsInputs>();
    }
    private void Start()
    {

    }
    public void OnShoot(InputValue value)
    {
        if (StarterAssetsInputs.Locked)
            return;

        Shoot();
    }
    public void Shoot()
    {
        if (DelayTimer > 0)
            return;

        DelayTimer = ShootingDelay;

        GameObject ProjectileObject = Instantiate(Projectile, SpawnPoint.position, SpawnPoint.rotation);
        ProjectileObject.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,0, ProjectileSpeed), ForceMode.Impulse);
        Destroy(ProjectileObject, DestroyTime);
        Debug.Log($"{name} shot Projectile from X: {ProjectileObject.transform.position.x} Y: {ProjectileObject.transform.position.y} Z: {ProjectileObject.transform.position.z}");
    }
    private void Update()
    {
        if (DelayTimer > 0) 
            DelayTimer -= Time.deltaTime;
        else {
            if (!HealthSystem.IsPlayer)
                Shoot();
        }

        GunModel.localPosition = Vector3.Lerp(GunModel.localPosition, (SuperShots > 0) ? Vector3.down * 10 : Vector3.zero, Time.deltaTime*AnimationTime);
        FastGunModel.localPosition = Vector3.Lerp(FastGunModel.localPosition, (SuperShots < 1) ? Vector3.up * 10 : Vector3.zero, Time.deltaTime*AnimationTime);
    }
}
