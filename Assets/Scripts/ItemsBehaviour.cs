using System;
using UnityEngine;
public enum ItemType {SpeedCan}
public class ItemsBehaviour : MonoBehaviour
{
    [Header("Item Data")]
    public ItemType Type;
    public float Duration = 3f;

    [Header("Item Animation")]
    public float AnimationSpeed = 0.015f;

    [Header("Objects Needed")]
    public GameObject Player;

    // Internal Variables
    public static Action OnPlayerTake;
    private float AnimationTime;

    private void ItemBehaviour(ItemType Item)
    {
        switch (Item)
        {
            case ItemType.SpeedCan:
                Player.GetComponent<StarterAssets.FirstPersonController>().CanCollected(Duration);
                break;
            default:
                break;
        }
    }
    private void GetItem()
    {
        ItemBehaviour(Type);
        OnPlayerTake?.Invoke();
        Debug.Log($"Player colleted {Type} at X: {transform.position.x} Y: {transform.position.y} Z: {transform.position.z}");
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

        transform.SetPositionAndRotation(
            new Vector3(
                ObjectPosition.x,
                ObjectPosition.y + Mathf.Sin(AnimationTime)*AnimationSpeed,
                ObjectPosition.z
            ),
            Quaternion.Euler(ObjectRotation.x, ObjectRotation.y + AnimationSpeed*100, ObjectRotation.z)
        );
    }
}
