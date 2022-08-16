using System.Collections;
using System.Collections.Generic;
using SimpleShooty.Common;
using UnityEngine;
using UnityEngine.AI;

namespace SimpleShooty.Game
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private PlayerAnimtion anim;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Collider agentCollider;
        [SerializeField] private Coin coinPrefab;
        [SerializeField] private float fullHealth;
        [SerializeField] private float speed;
        [SerializeField] private float range;
        [SerializeField] private Material deadMat;
        [SerializeField] private List<Rigidbody> rigidbodies;
        [SerializeField] private List<MeshRenderer> renderers;

        private float currentHealth;
        private bool isStop;
        public bool IsAlive { get; private set; }

        private PlayerController player { get => GameManager.Instance.Player; }

        private void Start()
        {
            EnemyManager.AddEnemy(this);
            IsAlive = true;

            agent.speed = speed;
            agent.updateRotation = false;

            currentHealth = fullHealth;
            isStop = true;
        }
        private void Update()
        {
            if (GameManager.Instance.CurrentGameState != GameState.Start)
            {
                return;
            }

            if (!isStop && IsAlive && player.IsAlive)
            {
                MoveToward(player.transform.position);
            }
        }
        private void FixedUpdate()
        {
            if (GameManager.Instance.CurrentGameState != GameState.Start)
            {
                return;
            }
            if (isStop && IsAlive && Vector3.Distance(player.transform.position, this.transform.position) < range)
            {
                isStop = false;
                anim.Idle();
            }
        }
        private void LateUpdate()
        {
            if (GameManager.Instance.CurrentGameState != GameState.Start)
            {
                return;
            }
            if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                other.GetComponent<PlayerController>().ReceiveDamage(100);
            }
        }
        private void MoveToward(Vector3 target)
        {
            agent.SetDestination(target);
            anim.Move(agent.velocity.normalized);
        }
        private void DropCoin()
        {
            var coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            coin.PlayAnimation();
        }
        public void Stop(bool isStop)
        {
            agent.isStopped = isStop;
            if (isStop)
            {
                anim.Move(Vector3.zero);
                anim.Idle();
            }
        }
        public void ReceiveDamage(float damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                DropCoin();
                anim.Disable();
                IsAlive = false;
                isStop = true;
                EnemyManager.RemoveEnemy(this);
                agent.isStopped = true;
                ToggleRigidBody(true);
            }
        }
        public void ToggleRigidBody(bool enable)
        {
            agent.enabled = !enable;
            agentCollider.enabled = !enable;
            foreach (var renderer in renderers)
            {
                renderer.material = deadMat;
            }
            foreach (var body in rigidbodies)
            {
                body.isKinematic = !enable;
            }
        }
    }
}
