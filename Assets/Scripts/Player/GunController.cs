using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class GunController : MonoBehaviour
    {
        [SerializeField] private Transform gunPoint;

        [SerializeField] private GameObject bulletPrefab;
        
        [SerializeField] private float firingRate;

        [SerializeField] private float bulletSpeed;

        [SerializeField] private bool playerMode;

        private Rigidbody _rigidbody;

        private bool _canFire;
    
    
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();

            _canFire = true;
        }

    
        void Update()
        {
            if (playerMode && Input.GetButton("Fire")) Fire();
        }


        private IEnumerator WaitForNextShot()
        {
            yield return new WaitForSeconds(1f/firingRate);
            _canFire = true;
        }

        public void Fire()
        {
            if (!_canFire) return;
            var bullet = Instantiate(bulletPrefab, gunPoint.position,gunPoint.rotation);
            bullet.GetComponent<Rigidbody>().velocity =
                _rigidbody.velocity + _rigidbody.velocity.normalized * bulletSpeed;
            if (playerMode) bullet.GetComponent<Bullet>().destroyEnemy = true;
            _canFire = false;
            StartCoroutine(WaitForNextShot());
        }
    }
}
