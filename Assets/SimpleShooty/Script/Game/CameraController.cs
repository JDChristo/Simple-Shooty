using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooty.Game
{
    public class CameraController : MonoBehaviour
    {
        #region --------------------------------------- Private variables ---------------------------------------
        [SerializeField] private Transform target;
        [SerializeField] private float smoothDampTime;
        private Vector3 startOffset;
        private Vector3 targetPosition;
        private Quaternion targetRotation;
        private Vector3 currentVelocity;
        #endregion ---------------------------------------

        #region --------------------------------------- Monobehaviour Methods-----------------------------------
        private void Start()
        {
            SetTarget();
        }
        private void Update()
        {
            if (target != null)
            {
                transform.LookAt(target);
            }
        }
        private void FixedUpdate()
        {
            if (target != null)
            {
                targetPosition = target.position + startOffset + Vector3.up;
                targetRotation = Quaternion.LookRotation(target.position - transform.position);

                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothDampTime * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothDampTime * Time.deltaTime);
            }
            else
            {
                SetTarget();
            }
        }

        private void SetTarget()
        {
            if (target != null)
            {
                startOffset = transform.position - target.position;
            }
        }
        #endregion ---------------------------------------
    }
}
