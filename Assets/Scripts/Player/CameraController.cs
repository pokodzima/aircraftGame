using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Player
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform target;

        [SerializeField] private float positionDamping;
        [SerializeField] private float rotationDamping;
    

    
        void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * positionDamping);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * rotationDamping);
        }
    }
}
