using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tram25SC : MonoBehaviour
{
    private NavMeshAgent NavAgent; //the NavMeshAgent for navigation AI 
    RetData data; //the data object which contains RET data information

    
    public Transform[] CheckPoints; //Stations checkpoints will be added in this array to navigate through
    Vector3 beginPoint; //the starting point which is just the loaction of this object
    Transform destinationPoint; //the destination point which is the next checkpoint this object will move to
    double time; //this variable is for storing the time difference of station Beurs and current time to calculate the speed of the object
    int index = 0; //index that will be used for the CheckPoints array

    private void Awake()
    {
        //initializing variables
        NavAgent = GetComponent<NavMeshAgent>();
        data = FindObjectOfType<RetData>();

    }

    private void Start()
    {
        //initializing variables
        GetComponent<BoxCollider>().enabled = false; //disabling the collisions to prevent early triggers
        time = data.TimeDifference_25SC; //storing the time difference data which is available in the New Data script
        beginPoint = transform.position; //setting the beginPoint to the location of this object
        destinationPoint = CheckPoints[0]; //setting the desitinationPoint to the location of the destination station
    }

    private void Update()
    {
        StartCoroutine(EnableCollider()); //starting a couroutine to enable the collider 
        NavAgent.destination = CheckPoints[index].position; //using NavAgent destination function to tell the object to move to the this checkpoint index

        Move(beginPoint, destinationPoint); //calling the move function to update the movement of the object

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Checkpoint")
        {
            Debug.LogWarning($"Tram: {gameObject.name}, Collided: {other.gameObject.name}"); //send a debug message each time this object collides with a trigger at the station
            StartCoroutine(ChangeDirection()); //starting a couroutine to change the direction to the next station (checkpoint)
            

        }

      

        //if object collides with the trigger on object with tag ("Finish") the object gets destroyed for performance purposes.
        if (other.gameObject.tag == "Finish")
        {
           
            data.isSpawnable_25SC = true;
            Destroy(gameObject);
        }

    }



    //change direction function
    IEnumerator ChangeDirection()
    {
        //this function get's called if the object arrived the checkpoint and the trigger gets activated.
        //first it will wait for 5 seconds then check the conditions
        yield return new WaitForSeconds(5);
        if (index == CheckPoints.Length - 1)
        {
            System.Array.Reverse(CheckPoints); //to avoid out of bounds errors, this reverses the array when the index has reached the final checkpoint
            index = 1;
        }
        else
        {
            //else just increase the index, and the objects destination is updated to the next checkpoint
            index++;

            if (index < CheckPoints.Length - 1)
            {
                //the beginPoint gets an update with the current location of the object
                //the desitantionPoint gets an update to the next checkpoint
                //these variables are for calculation the speed so that the object reaches the station at the expected real time value

                beginPoint = transform.position;
                destinationPoint = CheckPoints[index];
            }


        }
    }

    //the function to acivate the collider
    IEnumerator EnableCollider() 
    {
        yield return new WaitForSeconds(3);
        GetComponent<BoxCollider>().enabled = true;
    }

    //the function to update the movement of the object
    void Move(Vector3 pos, Transform ob2)
    {
        //this is to calculate the distance between beginPoint (the object's current location) and
        //the next station (destinationPoint)
        float distance = Vector3.Distance(pos, ob2.transform.position);
      
        //with a formula : speed = distance / time, the NavAgent.speed gets updated with the distance calculated above and the time difference data we get from RetData
        NavAgent.speed = (float)(distance / time);

    }

}
