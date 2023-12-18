using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    //check for laser collision (trigger)
    //instantiate explosion at position of the asteroid (us)
    //destroy explosion after 3 seconds

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, 0.25f);
        }
    }

    IEnumerator DestroyExplosionRoutine()
    {
        yield return new WaitForSeconds(3f);
        Destroy(_explosionPrefab.gameObject);

    }
    IEnumerator DelayAsteroidDestructionRoutine()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Made it!");

    }
}
