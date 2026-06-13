using UnityEngine;

public class ArrowTrigger : MonoBehaviour
{

    [Header("References")]
    public FloorArrow floorArrow;
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        floorArrow.Show();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        floorArrow.Hide();
    }
}