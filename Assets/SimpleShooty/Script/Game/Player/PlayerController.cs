using System;
using System.Collections;
using System.Collections.Generic;
using SimpleShooty.Common;
using UnityEngine;

namespace SimpleShooty.Game
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float sensitivity;
        [SerializeField] private float speed;
        [SerializeField] private float turnSpeed;
        [SerializeField] private float height = 0.5f;
        [SerializeField] private float fallHeight = -10f;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private PlayerAnimtion playerAnimtion;
        [SerializeField] private CombatManager combatManager;
        [SerializeField] private List<Rigidbody> rigidbodies;

        private Rigidbody rb;
        private Vector3 curDir;
        private Vector3 mouseStartPos;
        private Vector3 forward;
        private float slopeLimitAngle = 120f;
        private float slopeAngle;
        private RaycastHit hitInfo;
        private bool isGround;

        private bool joystickActive;
        private Transform lookAtTarget;
        public bool IsAlive { get; private set; }

        private void Start()
        {
            IsAlive = true;
            GameManager.Instance.SetPlayer(this);
            joystickActive = false;
            rb = GetComponent<Rigidbody>();
            VictoryDance();

            GameManager.Instance.OnStateUpdate += OnStateUpdate;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnStateUpdate -= OnStateUpdate;
        }

        private void Update()
        {
            if (!joystickActive)
            {
                return;
            }
            GetInput();
            Rotate();
        }
        private void FixedUpdate()
        {
            if (!joystickActive)
            {
                return;
            }
            CalculateForwardAngle();
            Move();
        }

        private void OnStateUpdate(GameState status)
        {
            switch (status)
            {
                case GameState.MainMenu:
                    joystickActive = false;
                    break;

                case GameState.Start:
                    ResetPlayer();
                    joystickActive = true;
                    break;

                case GameState.Won:
                    joystickActive = false;
                    break;

                case GameState.Lost:
                    joystickActive = false;
                    break;

                default:
                    break;
            }
        }
        private void VictoryDance()
        {
            combatManager.DisableAllGuns();
            transform.rotation = Quaternion.Euler(0, 180, 0);
            playerAnimtion.Move(Vector3.zero);
            playerAnimtion.Dance();

        }
        public void ReachedFinishPoint(Vector3 targetPosition)
        {
            joystickActive = false;
            GameManager.Instance.UpdateState(GameState.Won);

            targetPosition.y = transform.position.y;
            transform.LeanMove(targetPosition, 0.6f).setOnComplete(VictoryDance);
        }

        public void GetInput()
        {
            if (!joystickActive)
            {
                curDir = Vector3.zero;
                return;
            }

            var mousePos = Input.mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                mouseStartPos = mousePos;
            }
            else if (Input.GetMouseButton(0))
            {
                float distance = (mousePos - mouseStartPos).magnitude;

                if (distance > sensitivity)
                {
                    mouseStartPos = mousePos - (curDir * sensitivity / 2f);
                }

                var curDir2D = (mousePos - mouseStartPos).normalized;
                curDir = new Vector3(curDir2D.x, 0, curDir2D.y);

            }
            else if (Input.GetMouseButtonUp(0))
            {
                curDir = Vector3.zero;
            }
        }
        public void Move()
        {
            if (!joystickActive)
            {
                return;
            }
            if (curDir != Vector3.zero)
            {
                if (slopeAngle < slopeLimitAngle)
                {
                    curDir.Normalize();
                    transform.position += new Vector3(curDir.x, curDir.y, curDir.z) * speed * Time.deltaTime;
                }
            }
            playerAnimtion.Move(curDir);
        }
        public void LookAt(Transform target)
        {
            this.lookAtTarget = target;
            transform.LookAt(lookAtTarget);
        }
        public void LookAt(Enemy target)
        {
            if (target == null)
            {
                lookAtTarget = null;
                return;
            }
            LookAt(target.transform);
        }
        public void Rotate()
        {
            if (GameManager.Instance.HasWon())
            {
                return;
            }

            if (lookAtTarget != null)
            {
                return;
            }

            if (curDir != Vector3.zero)
            {
                var targetRot = Quaternion.LookRotation(curDir);
                if (rb.rotation != targetRot)
                {
                    rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRot, turnSpeed);
                }
            }
        }
        public void CalculateForwardAngle()
        {
            if (transform.position.y < fallHeight)
            {
                ReceiveDamage(100);
            }
            isGround = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitInfo, height, groundMask);

            if (!isGround)
            {
                forward = transform.forward;
                slopeAngle = 90;
                return;
            }

            forward = Vector3.Cross(transform.right, hitInfo.normal);
            slopeAngle = Vector3.Angle(hitInfo.normal, transform.forward);
        }

        public void ResetPlayer()
        {
            combatManager.SelectWeapon(WeaponType.PISTOL);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            playerAnimtion.Idle();
        }
        public void ReceiveDamage(float damage)
        {
            playerAnimtion.Disable();
            IsAlive = false;
            ToggleRigidBody(true);
        }
        public void ToggleRigidBody(bool enable)
        {
            GameManager.Instance.UpdateState(GameState.Lost);
            GetComponent<Collider>().enabled = !enable;
            rb.isKinematic = true;

            foreach (var body in rigidbodies)
            {
                body.isKinematic = !enable;
            }
        }
    }
}
