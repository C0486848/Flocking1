using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    //Public Variables
    public GameObject boidPrefab;
    public int numBoids = 20;
    public GameObject[] allBoids;
    public Vector3 boidLimits = new Vector3(5.0f, 5.0f, 5.0f);

    public Vector3 goalPos;

    [Header("Boid Settings")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;

    [Range(0.0f, 5.0f)]
    public float maxSpeed;

    [Range(1.0f, 10.0f)]
    public float neighbourDistance;

    [Range(0.0f, 5.0f)]
    public float rotationSpeed;






    // Start is called before the first frame update
    void Start()
    {
        allBoids = new GameObject[numBoids];
        for (int i = 0; i < allBoids.Length; ++i)
        {
            Vector3 pos = this.transform.position +
                new Vector3(Random.Range(-boidLimits.x, boidLimits.x),
                            Random.Range(-boidLimits.y, boidLimits.y),
                            Random.Range(-boidLimits.z, boidLimits.z));
            allBoids[i] = (GameObject)Instantiate(boidPrefab, pos, Quaternion.identity);
            allBoids[i].GetComponent<Flock>().myManager = this;


            goalPos = this.transform.position;
        }
    }

    private void Update()
    {
        if (Random.Range(0.0f, 100.0f) < 10.0f)
        {
            goalPos = this.transform.position + new Vector3 (Random.Range(-boidLimits.x, boidLimits.x),
                                                            Random.Range(-boidLimits.x, boidLimits.x),
                                                            Random.Range(-boidLimits.x, boidLimits.x));
        }
    }

}