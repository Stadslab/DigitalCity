using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tram21EW : MonoBehaviour
{
    private NavMeshAgent NavAgent;
    public Transform[] CheckPoints;
    Vector3 beginPoint;
    Transform destinationPoint;
    RetData data;
    double time;
    int index = 0;
    private void Awake()
    {
        NavAgent = GetComponent<NavMeshAgent>();
        data = FindObjectOfType<RetData>();

    }

    private void Start()
    {
        GetComponent<BoxCollider>().enabled = false;
        time = data.TimeDifference_21EW;
        beginPoint = transform.position;
        destinationPoint = CheckPoints[0];
    }

    private void Update()
    {
        StartCoroutine(EnableCollider());
        NavAgent.destination = CheckPoints[index].position;

        Move(beginPoint, destinationPoint);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Checkpoint")
        {
            Debug.LogWarning($"Tram: {gameObject.name}, Collided: {other.gameObject.name}");
            StartCoroutine(ChangeDirection());
            

        }

       


        if (other.gameObject.tag == "Finish")
        {
          
            data.isSpawnable_21EW = true;
            Destroy(gameObject);

        }

    }

    



    IEnumerator ChangeDirection()
    {

        yield return new WaitForSeconds(5);
        if (index == CheckPoints.Length - 1)
        {
            System.Array.Reverse(CheckPoints);
            index = 1;
        }
        else
        {
            index++;

            if (index < CheckPoints.Length - 1)
            {
                beginPoint = transform.position;
                destinationPoint = CheckPoints[index];
            }


        }
    }

    IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(3);
        GetComponent<BoxCollider>().enabled = true;
    }


    void Move(Vector3 pos, Transform ob2)
    {
        float distance = Vector3.Distance(pos, ob2.transform.position);
      
        NavAgent.speed = (float)(distance / time);

    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
