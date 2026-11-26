using System;
using UnityEngine;
public class ItemsBehaviour : MonoBehaviour
{
    public float AnimationSpeed = 0.015f;
    public static Action OnPlayerTake;
    private float AnimationTime;

    private void GetItem()
    {
        OnPlayerTake?.Invoke();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Projectile"))
            GetItem();
    }
    private void Update()
    {
        AnimationTime += Time.deltaTime;

        Vector3 ObjectPosition = transform.position;
        Vector3 ObjectRotation = transform.rotation.eulerAngles;

        transform.position = new Vector3(
            ObjectPosition.x,
            ObjectPosition.y + Mathf.Sin(AnimationTime)*AnimationSpeed,
            ObjectPosition.z
        );
        transform.SetPositionAndRotation(
            new Vector3(
                ObjectPosition.x,
                ObjectPosition.y + Mathf.Sin(AnimationTime)*AnimationSpeed,
                ObjectPosition.z
            ),
            Quaternion.Euler(ObjectRotation.x, ObjectRotation.y + AnimationTime*0.25f, ObjectRotation.z)
        );
    }
}
