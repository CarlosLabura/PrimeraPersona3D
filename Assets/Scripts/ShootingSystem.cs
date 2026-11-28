using UnityEngine;
using UnityEngine.InputSystem;
public class ShootingSystem : MonoBehaviour
{
    [Header("Shooting Settings")]
    public float ProjectileSpeed = 15f;
    public float ShootingDelay = 0.3f;
    public float DestroyTime = 3f;

    [Header("Objects Needed")]
    public GameObject Projectile;
    public Transform SpawnPoint;

    [Header("Script Data")]
    [ReadOnly] public float DelayTimer;

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
    void Update()
    {
        if (DelayTimer > 0) 
            DelayTimer -= Time.deltaTime;
        else 
            if (!HealthSystem.IsPlayer)
                Shoot();
    }
}
