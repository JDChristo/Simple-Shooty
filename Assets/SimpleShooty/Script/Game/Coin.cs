using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            transform.LeanMove(other.transform.position, 0.2f).setEaseOutBack();
            transform.LeanScale(Vector3.zero, 0.2f).setEaseOutBack().setOnComplete(() =>
            {
                Destroy(this.gameObject);
            });
        }
    }
    public void PlayAnimation()
    {
        var p1 = transform.position;
        var p2 = transform.position + (Vector3.up * 4);
        var p3 = transform.position + (Vector3.up * 4) + Vector3.right;
        var p4 = transform.position + (Vector3.up) + Vector3.right;

        transform.LeanMove(new Vector3[] { p1, p2, p3, p4 }, 1f).setEaseOutBack();
        LeanTween.rotateAround(gameObject, Vector3.up, 360, 6f).setLoopClamp();
    }
}
