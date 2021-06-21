using System;
using Player;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;
using Random = UnityEngine.Random;

namespace Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(GunController))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private float speed;

        [SerializeField]
        private float rotationSpeed;

        [SerializeField] private float pickPlayerTargetProbability;

        [SerializeField] private float fireRotationToleranceAngle;

        [SerializeField] private float minDistanceToTarget;

        [SerializeField] private Vector3 targetBounds;

        [SerializeField] private GameObject markPrefab;

        private Rigidbody _rigidbody;

        private Vector3 _target;

        private bool _playerTargeted;

        private Quaternion _targetRotation;

        private float _lastMagnitude;

        private GunController _gunController;

        private Transform _player;

        public bool isDestroying;


        [Inject]
        private void Construct(PlaneController player)
        {
            _player = player.transform;
        }


        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _gunController = GetComponent<GunController>();
            GenerateNewTarget();
        }

    
        void FixedUpdate()
        {
            var newPosition = transform.position + Time.deltaTime * speed * transform.forward;
            _rigidbody.MovePosition(newPosition);

            if ((_target - transform.position).sqrMagnitude > minDistanceToTarget * minDistanceToTarget)
            {
                var newRotation = Quaternion.Lerp(transform.rotation, _targetRotation, Time.deltaTime * rotationSpeed);
                if (_playerTargeted)
                {
                    newRotation.ToAngleAxis(out var angle,out Vector3 axis);
                    if (angle < fireRotationToleranceAngle)
                    {
                        _gunController.Fire();
                        GenerateNewTarget();
                    }
                }
                _rigidbody.MoveRotation(newRotation);
            }
            else
            {
                GenerateNewTarget();
            }
        
            if (_lastMagnitude < (_target - transform.position).sqrMagnitude) GenerateNewTarget();

            _lastMagnitude = (_target - transform.position).sqrMagnitude;

        }

        private void Update()
        {
            if (isDestroying) Destroy(gameObject);
        }


        private void GenerateNewTarget()
        {
            if (Random.Range(0, 100) < pickPlayerTargetProbability)
            {
                _target = _player.position;
                _playerTargeted = true;
            }
            else
            {
                _playerTargeted = false;
                _target = new Vector3(Random.Range(-targetBounds.x, targetBounds.x),
                    Random.Range(-targetBounds.y, targetBounds.y), Random.Range(-targetBounds.z, targetBounds.z));
                _targetRotation = Quaternion.LookRotation(_target - transform.position);
            }
        }
    }
}