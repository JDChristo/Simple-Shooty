using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimtion : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private int blendSpeed;
    private int blendSideSpeed;
    private int victoryDanceHash;
    private int idleHash;

    private void Awake()
    {
        blendSpeed = Animator.StringToHash("Blend");
        blendSideSpeed = Animator.StringToHash("BlendSide");

        victoryDanceHash = Animator.StringToHash("Victory");
        idleHash = Animator.StringToHash("Idle");
    }
    public void Disable()
    {
        anim.enabled = false;
    }
    public void Enable()
    {
        anim.enabled = true;
    }
    public void Idle()
    {
        anim.Play(idleHash, 0);
    }
    public void Move(Vector3 direction)
    {
        direction.Normalize();
        anim.SetFloat(blendSpeed, direction.x);
        anim.SetFloat(blendSideSpeed, direction.z);
    }
    public void Dance()
    {
        anim.Play(victoryDanceHash, 0);
    }
}
