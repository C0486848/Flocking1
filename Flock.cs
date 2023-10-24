using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Bounds.cs;

public class Flock : MonoBehaviour
{
    public FlockManager myManager;
    public float speed;

    Bounds bounds = new Bounds(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(5.0f, 5.0f, 5.0f));



    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }
    //End Of Start

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit = new RaycastHit();
        Vector3 direction;

        bool turning = false;
        if (bounds.Contains(myManager.flockBoundary))
        {
            if (turning == false)
            {
                if (Random.Range(0.0f, 100.0f) < 20.0f)
                {
                    Debug.Log(Random.Range(0.0f, 100.0f) < 20.0f);
                    ApplyRules();
                }
                //End Of If

                if (Random.Range(0.0f, 100.0f) < 10.0f)
                {
                    speed = Random.Range(0.0f, 2.0f);
                }
                //End Of If
            }
            //End Of If
            //False clause done
        }
        else
        {

            turning = true;
            if (!bounds.Contains(myManager.flockBoundary)) //Outside Boundary
            {
                if (turning == true)
                {

                    //Reflect Step
                    Physics.Raycast(transform.position, this.transform.forward * 50.0f, out hit);
                    direction = Vector3.Reflect(this.transform.forward, hit.normal);

                    //Quaternion Slerp Step
                    transform.rotation = Quaternion.Slerp(
                            transform.rotation,
                            Quaternion.LookRotation(direction),
                            myManager.rotationSpeed * Time.deltaTime
                       );
                }
                

            }
            //End Of If
        }
        //End Of If Else


        this.transform.Translate(0.0f, 0.0f, Time.deltaTime * speed);
    }
    //End Of Update


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
            // is this in the neighbourhood?
            nDistance = Vector3.Distance(go.transform.position, this.transform.position);
            if (nDistance <= myManager.neighbourDistance)
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
        // End Of ForEach of all the gos

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
               );
            }

        }// End Of If
    }
}