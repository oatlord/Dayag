using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending2Trigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameSceneManager.instance.MoveToScene("Ending 2");
        }
    }
}
