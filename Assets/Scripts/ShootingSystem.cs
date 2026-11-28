using UnityEngine;
using UnityEngine.InputSystem;
public class ShootingSystem : MonoBehaviour
{
    [Header("Shooting Settings")]
    public float ProjectileSpeed = 15f;
    public float ShootingDelay = 0.3f;
    public float DestroyTime = 3f;
    [ShowIf(typeof(HealthSystem), "IsPlayer")] public int SuperShots = 0;
    [ShowIf(typeof(HealthSystem), "IsPlayer")] public float AnimationTime = 15f;

    [Header("Objects Needed")]
    public GameObject Projectile;
    [ShowIf(typeof(HealthSystem), "IsPlayer")] public GameObject Ray;
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
    public void OnShoot(InputValue value)
    {
        if (StarterAssetsInputs.Locked)
            return;

        Shoot(SuperShots != 0);
    }
    public void Shoot(bool Supershot = false)
    {
        if (DelayTimer > 0)
            return;
        DelayTimer = ShootingDelay;

        if (Supershot)
        {
            SuperShots--;
            Ray.GetComponent<RayColor>().SetAlpha(1); //i am too lazy and feeling silly
            Debug.Log($"{name} shot Ray from X: {transform.position.x} Y: {transform.position.y} Z: {transform.position.z}");
        } 
        else 
        {
            GameObject ProjectileObject = Instantiate(Projectile, SpawnPoint.position, SpawnPoint.rotation);
            ProjectileObject.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,0, ProjectileSpeed), ForceMode.Impulse);

            if (!HealthSystem.IsPlayer)
                ProjectileObject.GetComponent<Outline>().OutlineColor = Color.red;

            Destroy(ProjectileObject, DestroyTime);
            Debug.Log($"{name} shot Projectile from X: {ProjectileObject.transform.position.x} Y: {ProjectileObject.transform.position.y} Z: {ProjectileObject.transform.position.z}");
        }
    }
    private void Update()
    {
        if (DelayTimer > 0) 
            DelayTimer -= Time.deltaTime;

        if (!HealthSystem.IsPlayer) //Below: Only for the player
            return;

        GunModel.localPosition = Vector3.Lerp(GunModel.localPosition, (SuperShots > 0) ? Vector3.down * 10 : Vector3.zero, Time.deltaTime*AnimationTime);
        FastGunModel.localPosition = Vector3.Lerp(FastGunModel.localPosition, (SuperShots < 1) ? Vector3.up * 10 : Vector3.zero, Time.deltaTime*AnimationTime);
    }
}
