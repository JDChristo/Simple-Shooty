using System.Collections;
using System.Collections.Generic;
using SimpleShooty.Game;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            other.TryGetComponent<PlayerController>(out PlayerController player);
            player?.ReachedFinishPoint(transform.position);
        }
    }
}
