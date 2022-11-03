using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _asteroidSpeed = 19.0f;
    [SerializeField]
    GameObject _explosionPrefab;
    private SpawnManager _spawnManager;

    


    private void Start()
    {
        
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        
        
        
        if( _spawnManager == null)
        {
            Debug.LogError("SpawnManager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _asteroidSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, 0.25f);
            
        }
    }


    //check for Laser collission 
    //instantiate explosion at the position of the asteroid
    //destory the explosion after 3 seconds. 
}
