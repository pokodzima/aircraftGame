using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlaneController : MonoBehaviour
    {
        [SerializeField]
        private float normalSpeed;

        [SerializeField] private float acceleration;

        private float _currentSpeed;

        [SerializeField] private float pitchSpeed;
        [SerializeField] private float rollSpeed;

        [SerializeField] private bool inversePitch = false;
        [SerializeField] private bool inverseRoll = false;

        private Vector3 _pitchAxis;
        private Vector3 _rollAxis;

        private Rigidbody _rigidbody;

        public Transform CameraTarget;


        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _pitchAxis = inversePitch ? Vector3.left : Vector3.right;
            _rollAxis = inverseRoll ? Vector3.forward : Vector3.back;
        }
    
    
        void FixedUpdate()
        {
            var pitch = Input.GetAxis("Vertical");
            var roll = Input.GetAxis("Horizontal");
            var accelerationInput = Input.GetAxis("Accelerate");

            _currentSpeed = Mathf.Lerp(_currentSpeed, normalSpeed + acceleration * accelerationInput, Time.deltaTime * acceleration);
        
            var newPosition = transform.position + Time.deltaTime * _currentSpeed * transform.forward;
            _rigidbody.MovePosition(newPosition);
        
       
            var newRotation = transform.rotation * Quaternion.AngleAxis(pitch * pitchSpeed * Time.deltaTime, _pitchAxis) *
                              Quaternion.AngleAxis(roll * rollSpeed * Time.deltaTime, _rollAxis);
            _rigidbody.MoveRotation(newRotation);
        }
    
    }
}
