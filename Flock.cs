using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Bounds.cs;

public class Flock : MonoBehaviour
{
    public FlockManager myManager;
    public float speed;

    Bounds bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(5, 5, 5));



    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);    
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit = new RaycastHit();
        Vector3 direction = Vector3.zero;
        
        bool turning = false;
        if(bounds.Contains(transform.position)) {
            turning = false;
            ApplyRules();
        }
        else {
            turning = true;
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
            transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(direction),
                    myManager.rotationSpeed * Time.deltaTime
               );
        }


        this.transform.Translate(0.0f, 0.0f, Time.deltaTime * speed);
    }


    void ApplyRules()
    {
        GameObject[] gos;
        gos = myManager.allBoids;
        float nDistance;
        Vector3 vCenter = Vector3.zero;
        Vector3 vAvoid = Vector3.zero;
        float gSpeed = 0.0f;
        int groupSize = 0;




        foreach (GameObject go in gos)
        {
            if (go == this.gameObject) continue;
            // is this in the neighbouurhood?
            nDistance = Vector3.Distance(go.transform.position, this.transform.position);
            if (nDistance <= myManager.neightbourDistance)
            {
                // get the center of the neighbours
                vCenter += go.transform.position;
                groupSize++;


                // is it too close?
                if (nDistance < 1.0f)
                {
                    vAvoid += (this.transform.position - go.transform.position);
                }

                // get the average speed
                Flock anotherBoid = go.GetComponent<Flock>();
                gSpeed += anotherBoid.speed;

            }
           
        }
        // end of foreach of all the gos

        if (groupSize > 0)
        {
            vCenter = (vCenter / groupSize); // - this.transform.position
            speed = gSpeed / groupSize;

            Vector3 direction = (vCenter + vAvoid) - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(direction),
                    myManager.rotationSpeed * Time.deltaTime
               ) ;
            }

        }// end of groupSize if
    }
}
