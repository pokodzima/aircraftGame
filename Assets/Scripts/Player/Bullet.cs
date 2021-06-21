using System;
using System.Collections;
using UnityEngine;

namespace Player
{
    public class Bullet : MonoBehaviour
    {
        public bool destroyEnemy;
        
        [SerializeField] private float timeToDeath;

        private const String PlayerString = "Player";
        private const String EnemyString = "Enemy";
        
        void Start()
        {
            StartCoroutine(Die());
        }
    

        private IEnumerator Die()
        {
            yield return new WaitForSeconds(timeToDeath);
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(PlayerString))
            {
                Debug.Log("Player Damaged!!!");
                Destroy(gameObject);
            }
            else if (other.gameObject.CompareTag(EnemyString) && destroyEnemy)
            {
                other.gameObject.GetComponent<Enemy.Enemy>().isDestroying = true;
                Destroy(gameObject);
            }
        }
    }
}
