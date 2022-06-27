using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Linq;
using UnityEngine;
using System.Collections;

public class Stops
{
    //stops model layout
    public bool IsTimingStop { get; set; }
    public string DestinationName50 { get; set; }
    public string DataOwnerCode { get; set; }
    public string OperatorCode { get; set; }
    public int FortifyOrderNumber { get; set; }
    public string TransportType { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int JourneyNumber { get; set; }
    public int JourneyPatternCode { get; set; }
    public int LocalServiceLevelCode { get; set; }
    public int LineDirection { get; set; }
    public string OperationDate { get; set; }
    public string TimingPointCode { get; set; }
    public string WheelChairAccessible { get; set; }
    public string LineName { get; set; }
    public string LinePublicNumber { get; set; }

    public string LastUpdateTimeStamp { get; set; }
    public string DestinationCode { get; set; }

    public string ExpectedDepartureTime { get; set; }
    public int UserStopOrderNumber { get; set; }
    public string ProductFormulaType { get; set; }
    public string TimingPointName { get; set; }
    public string LinePlanningNumber { get; set; }
    public string StopAreaCode { get; set; }
    public string TimingPointDataOwnerCode { get; set; }
    public string TimingPointTown { get; set; }
    public string TripStopStatus { get; set; }
    public string UserStopCode { get; set; }
    public string JourneyStopType { get; set; }

    public string TargetArrivalTime { get; set; }

    public string TargetDepartureTime { get; set; }

    public string ExpectedArrivalTime { get; set; }
    public int NumberOfCoaches { get; set; }
    public string TimingPointWheelChairAccessible { get; set; }
    public string TimingPointVisualAccessible { get; set; }
}

public class Journey
{
    //journey model layout
    public string ServerTime { get; set; }
    public Dictionary<string, Stops> Stops { get; set; }
}

public class RetData : MonoBehaviour
{
    // Start is called before the first frame update
    

    Dictionary<string, int> ResponseData;

    [HideInInspector]
    public Dictionary<string, Journey> RetTrams = new Dictionary<string, Journey>();
    public KeyValuePair<string, Stops> stopObject;



    //Legenda
    /*
     * 20LR => Lombardijen - Rotterdam Centraal
     * 20RL => Rotterdam - Lombardijen
     * 
     * 21EW => Esch - Woudhoek
     * 21WE => Woudhoek - Esch
     * 
     * 23BM => trem 23 Beverwaard - Marconiplein
     * 23MB => trem 23 Marconiplein - Beverwaard
     * 
     * 24EH => Esch - Holy
     * 24HE => Holy - Esch
     * 
     * 25CS => Carnisselande - Schiebroek
     * 25SC => Schiebroek - Carnisselande
     * 
     */

    //tram and lines setup
    //trem prefab
    //trem spawnpoint
    //trem time difference
    public GameObject TramPreFab_20LR; // the prefab of the tram model that will be used to visualize the movement
    public Transform SpawnPoint_20LR; //the spawn point of the model
    [HideInInspector]
    public double TimeDifference_20LR; //the time differnce between station arrival time and current time
    [HideInInspector]
    public bool isSpawnable_20LR = false; //to prevent multiple spawning at once.

    public GameObject TramPreFab_20RL;
    public Transform SpawnPoint_20RL;
    [HideInInspector]
    public double TimeDifference_20RL;
    [HideInInspector]
    public bool isSpawnable_20RL = false;

    public GameObject TramPreFab_21EW;
    public Transform SpawnPoint_21EW;
    [HideInInspector]
    public double TimeDifference_21EW;
    [HideInInspector]
    public bool isSpawnable_21EW = false; 
    

    public GameObject TramPreFab_21WE;
    public Transform SpawnPoint_21WE;
    [HideInInspector]
    public double TimeDifference_21WE;
    [HideInInspector]
    public bool isSpawnable_21WE = false;


    public GameObject TramPreFab_23BM;
    public Transform SpawnPoint_23BM;
    [HideInInspector]
    public double TimeDifference_23BM;
    [HideInInspector]
    public bool isSpawnable_23BM = false;

    public GameObject TramPreFab_23MB;
    public Transform SpawnPoint_23MB;
    [HideInInspector]
    public double TimeDifference_23MB;
    [HideInInspector]
    public bool isSpawnable_23MB = false;

    public GameObject TramPreFab_24EH;
    public Transform SpawnPoint_24EH;
    [HideInInspector]
    public double TimeDifference_24EH;
    [HideInInspector]
    public bool isSpawnable_24EH = false;

    public GameObject TramPreFab_24HE;
    public Transform SpawnPoint_24HE;
    [HideInInspector]
    public double TimeDifference_24HE;
    [HideInInspector]
    public bool isSpawnable_24HE = false;

    public GameObject TramPreFab_25CS;
    public Transform SpawnPoint_25CS;
    [HideInInspector]
    public double TimeDifference_25CS;
    [HideInInspector]
    public bool isSpawnable_25CS = false;

    public GameObject TramPreFab_25SC;
    public Transform SpawnPoint_25SC;
    [HideInInspector]
    public double TimeDifference_25SC;
    [HideInInspector]
    public bool isSpawnable_25SC = false;





    void Awake()
    {
        //api call on awake function
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://v0.ovapi.nl/journey/"); //create web request to the api for OV data
        HttpWebResponse res = (HttpWebResponse)req.GetResponse(); //getting the response
        StreamReader ResponseStream = new StreamReader(res.GetResponseStream()); //openinge a response stream
        ResponseData = JsonConvert.DeserializeObject<Dictionary<string, int>>(ResponseStream.ReadToEnd()); //converting the data to a Dictionary

    }

    private void Start()
    {
        //initialize dictionary with data on start
        RetTrams = GetData();

        //make every tram be able to spawn once
        isSpawnable_20LR = true;
        isSpawnable_20RL = true;

        isSpawnable_21EW = true;
        isSpawnable_21WE = true;

        isSpawnable_23BM = true;
        isSpawnable_23MB = true;

        isSpawnable_24EH = true;
        isSpawnable_24HE = true;

        isSpawnable_25CS = true;
        isSpawnable_25SC = true;



    }

    private void Update()
    {

        foreach (KeyValuePair<string, Journey> journey in RetTrams)
        {
            foreach (KeyValuePair<string, Stops> stops in journey.Value.Stops)
            {
                //trem lombardijen to Rotterdam Centraal (line number = 20, line direction = 1, data of station (TimingPointName) = Beurs
                //if condition is to filter data for getting specific trams that are available after the passed time and that's going to a specific destination
                if (stops.Value.LinePublicNumber == "20" && stops.Value.TimingPointName == "Beurs" && stops.Value.LineDirection == 1 && stops.Value.TripStopStatus != "PASSED" && DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes > 0)
                {
                    //if the difference between current time and arrival time is smaller than 3 minutes than the object can be spawned in the scene
                    if (DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes < 3 && DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes > 1)
                    {
                        //calculate the time difference to use it for the movement of the object
                        TimeDifference_20LR = DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalSeconds;

                        if (isSpawnable_20LR)
                        {

                            Debug.Log($"Trem: {stops.Value.LinePublicNumber}, " +
                                    $"Direction: {stops.Value.TimingPointName}, " +
                                    $" Om  {DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).ToLongTimeString()} Komt hij aan op: {stops.Value.TimingPointName} ");

                            //Instatiate / Spawn the specific tram object based on the information filtered in this case Tram Lombardijen to Rotterdam Centraal
                            Instantiate(TramPreFab_20LR, SpawnPoint_20LR.position, Quaternion.AngleAxis(-180f, Vector3.up));
                            isSpawnable_20LR = false;

                        }

                    }

                }
                //trem Rotterdam Centraal to Lombardijen (line number = 20, line direction = 2, data of station (TimingPointName) = Beurs

                if (stops.Value.LinePublicNumber == "20" && stops.Value.TimingPointName == "Beurs" && stops.Value.LineDirection == 2 && stops.Value.TripStopStatus != "PASSED" && DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes > 0)
                {

                    if (DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes < 3 && DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes > 1)
                    {
                        TimeDifference_20RL = DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalSeconds;

                        if (isSpawnable_20RL)
                        {

                            Debug.Log($"Trem: {stops.Value.LinePublicNumber}, " +
                                        $"Direction: {stops.Value.TimingPointName}, " +
                                        $" Om  {DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).ToLongTimeString()} Komt hij aan op: {stops.Value.TimingPointName} ");

                            Instantiate(TramPreFab_20RL, SpawnPoint_20RL.position, Quaternion.AngleAxis(-180f, Vector3.up));
                            isSpawnable_20RL = false;

                        }

                    }

                }
                //trem Esch naar Woudhoek (line number = 21, line direction = 1, data van station (TimingPointName) = Beurs

                if (stops.Value.LinePublicNumber == "21" && stops.Value.TimingPointName == "Beurs" && stops.Value.LineDirection == 1 && stops.Value.TripStopStatus != "PASSED" && DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes > 0)
                {

                    if (DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes < 2 && DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes > 1)
                    {
                        TimeDifference_21EW = DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalSeconds;

                        if (isSpawnable_21EW)
                        {

                            Debug.Log($"Trem: {stops.Value.LinePublicNumber}, " +
                                        $"Direction: {stops.Value.TimingPointName}, " +
                                            $" Om  {DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).ToLongTimeString()} Komt hij aan op: {stops.Value.TimingPointName} ");

                            Instantiate(TramPreFab_21EW, SpawnPoint_21EW.position, Quaternion.AngleAxis(-180f, Vector3.up));
                            isSpawnable_21EW = false;

                        }

                    }

                }
                //trem Woudhoek naar Esch (line number = 21, line direction = 2, data van station (TimingPointName) = Beurs

                if (stops.Value.LinePublicNumber == "21" && stops.Value.TimingPointName == "Beurs" && stops.Value.LineDirection == 2 && stops.Value.TripStopStatus != "PASSED" && DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes > 0)
                {

                    if (DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes < 2 && DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes > 1)
                    {
                        TimeDifference_21WE = DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalSeconds;

                        if (isSpawnable_21WE)
                        {

                            Debug.Log($"Trem: {stops.Value.LinePublicNumber}, " +
                                        $"Direction: {stops.Value.TimingPointName}, " +
                                        $" Om  {DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).ToLongTimeString()} Komt hij aan op: {stops.Value.TimingPointName} ");

                            Instantiate(TramPreFab_21WE, SpawnPoint_21WE.position, Quaternion.AngleAxis(-16.5f, Vector3.up));
                            isSpawnable_21WE = false;

                        }

                    }

                }

                //trem beverwaard naar marconiplein (line number = 23, line direction = 1, data van station (TimingPointName) = Beurs

                if (stops.Value.LinePublicNumber == "23" && stops.Value.TimingPointName == "Beurs" && stops.Value.LineDirection == 1 && stops.Value.TripStopStatus != "PASSED" && DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes > 0)
                {

                    if (DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes < 3 && DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes > 1)
                    {
                        TimeDifference_23BM = DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalSeconds;

                        if (isSpawnable_23BM)
                        {

                            Debug.Log($"Trem: {stops.Value.LinePublicNumber}, " +
                                         $"Direction: {stops.Value.TimingPointName}, " +
                                        $" Om  {DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).ToLongTimeString()} Komt hij aan op: {stops.Value.TimingPointName} ");

                            Instantiate(TramPreFab_23BM, SpawnPoint_23BM.position, Quaternion.AngleAxis(-180f, Vector3.up));
                            isSpawnable_23BM = false;

                        }

                    }

                }

                //trem marconiplein naar beverwaard (line number = 23, line direction = 2, data van station (TimingPointName) = Beurs

                if (stops.Value.LinePublicNumber == "23" && stops.Value.TimingPointName == "Beurs" && stops.Value.LineDirection == 2 && stops.Value.TripStopStatus != "PASSED" && DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes > 0)
                {

                    if (DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes < 3 && DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes > 1)
                    {
                        TimeDifference_23MB = DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalSeconds;

                        if (isSpawnable_23MB)
                        {

                            Debug.Log($"Trem: {stops.Value.LinePublicNumber}, " +
                                         $"Direction: {stops.Value.TimingPointName}, " +
                                        $" Om  {DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).ToLongTimeString()} Komt hij aan op: {stops.Value.TimingPointName} ");

                            Instantiate(TramPreFab_23MB, SpawnPoint_23MB.position, Quaternion.AngleAxis(-16.5f, Vector3.up));
                            isSpawnable_23MB = false;

                        }

                    }

                }

                //trem Esch naar Holy (line number = 24, line direction = 1, data van station (TimingPointName) = Beurs

                if (stops.Value.LinePublicNumber == "24" && stops.Value.TimingPointName == "Beurs" && stops.Value.LineDirection == 1 && stops.Value.TripStopStatus != "PASSED" && DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes > 0)
                {

                    if (DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes < 2 && DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes > 1)
                    {
                        TimeDifference_24EH = DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalSeconds;

                        if (isSpawnable_24EH)
                        {

                            Debug.Log($"Trem: {stops.Value.LinePublicNumber}, " +
                                     $"Direction: {stops.Value.TimingPointName}, " +
                                    $" Om  {DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).ToLongTimeString()} Komt hij aan op: {stops.Value.TimingPointName} ");

                            Instantiate(TramPreFab_24EH, SpawnPoint_24EH.position, Quaternion.AngleAxis(-16.5f, Vector3.up));
                            isSpawnable_24EH = false;

                        }

                    }

                }

                //trem Holy naar Esch (line number = 24, line direction = 2, data van station (TimingPointName) = Beurs

                if (stops.Value.LinePublicNumber == "24" && stops.Value.TimingPointName == "Beurs" && stops.Value.LineDirection == 2 && stops.Value.TripStopStatus != "PASSED" && DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes > 0)
                {

                    if (DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes < 2 && DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes > 1)
                    {
                        TimeDifference_24HE = DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalSeconds;

                        if (isSpawnable_24HE)
                        {

                            Debug.Log($"Trem: {stops.Value.LinePublicNumber}, " +
                                         $"Direction: {stops.Value.TimingPointName}, " +
                                        $" Om  {DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).ToLongTimeString()} Komt hij aan op: {stops.Value.TimingPointName} ");

                            Instantiate(TramPreFab_24HE, SpawnPoint_24HE.position, Quaternion.AngleAxis(-16.5f, Vector3.up));
                            isSpawnable_24HE = false;

                        }

                    }

                }

                //trem Carnisselande naar Schiebroek (line number = 25, line direction = 1, data van station (TimingPointName) = Beurs

                if (stops.Value.LinePublicNumber == "25" && stops.Value.TimingPointName == "Beurs" && stops.Value.LineDirection == 1 && stops.Value.TripStopStatus != "PASSED" && DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes > 0)
                {

                    if (DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes < 3 && DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes > 1)
                    {
                        TimeDifference_25CS = DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalSeconds;

                        if (isSpawnable_25CS)
                        {

                            Debug.Log($"Trem: {stops.Value.LinePublicNumber}, " +
                                         $"Direction: {stops.Value.TimingPointName}, " +
                                    $" Om  {DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).ToLongTimeString()} Komt hij aan op: {stops.Value.TimingPointName} ");


                            Instantiate(TramPreFab_25CS, SpawnPoint_25CS.position, Quaternion.AngleAxis(-16.5f, Vector3.up));
                            isSpawnable_25CS = false;

                        }

                    }

                }

                //trem Schiebroek naar Carnisselande (line number = 25, line direction = 2, data van station (TimingPointName) = Beurs

                if (stops.Value.LinePublicNumber == "25" && stops.Value.TimingPointName == "Beurs" && stops.Value.LineDirection == 2 && stops.Value.TripStopStatus != "PASSED" && DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes > 0)
                {

                    if (DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes < 3 && DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalMinutes > 1)
                    {
                        TimeDifference_25SC = DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).Subtract(DateTime.Now).TotalSeconds;

                        if (isSpawnable_25SC)
                        {

                            Debug.Log($"Trem: {stops.Value.LinePublicNumber}, " +
                                        $"Direction: {stops.Value.TimingPointName}, " +
                                        $" Om  {DateTime.ParseExact(stops.Value.TargetArrivalTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).ToLongTimeString()} Komt hij aan op: {stops.Value.TimingPointName} ");

                            Instantiate(TramPreFab_25SC, SpawnPoint_25SC.position, Quaternion.AngleAxis(-16.5f, Vector3.up));
                            isSpawnable_25SC = false;

                        }

                    }

                }






            }
        }


    }

    //Function to Get Data from api
    Dictionary<string, Journey> GetData()
    {
        var tramData = new Dictionary<string, Journey>(); //this variable will be used to store the filtered RET tram data and return it
        foreach (KeyValuePair<string, int> trem in ResponseData) //looping through the response data from the first api call in start
        {
            if (!trem.Key.Contains("M") && trem.Key.Contains("RET")) //filtering the data specific on trams and busses of RET services
            {
                HttpWebRequest reqRET = (HttpWebRequest)WebRequest.Create("https://v0.ovapi.nl/journey/" + trem.Key); //creating another call with the filtered data by using trem.Key
                HttpWebResponse resRET = (HttpWebResponse)reqRET.GetResponse();
                StreamReader ResponseStreamRET = new StreamReader(resRET.GetResponseStream());
                Dictionary<string, Journey> ResponseDataRET = JsonConvert.DeserializeObject<Dictionary<string, Journey>>(ResponseStreamRET.ReadToEnd()); //converting data to dictionary

                foreach (KeyValuePair<string, Journey> j in ResponseDataRET) //loop through all the trams and busses from RET services
                {

                    foreach (KeyValuePair<string, Stops> s in j.Value.Stops)
                    {
                        //filter specific trams from the data and add them to the tramData value
                        if (s.Value.LinePublicNumber == "20" && (s.Value.TimingPointName == "Beurs" || s.Value.TimingPointName == "Leuvehaven" || s.Value.TimingPointName == "Lijnbaan") && s.Value.TripStopStatus != "PASSED")
                        {
                            stopObject = s;

                            tramData[j.Key] = j.Value;

                        }
                        if (s.Value.LinePublicNumber == "21" && (s.Value.TimingPointName == "Beurs" || s.Value.TimingPointName == "Keizerstraat" || s.Value.TimingPointName == "Stadhuis") && s.Value.TripStopStatus != "PASSED")
                        {
                            stopObject = s;

                            tramData[j.Key] = j.Value;

                        }
                        if (s.Value.LinePublicNumber == "23" && (s.Value.TimingPointName == "Beurs" || s.Value.TimingPointName == "Leuvehaven" || s.Value.TimingPointName == "Stadhuis") && s.Value.TripStopStatus != "PASSED")
                        {
                            stopObject = s;

                            tramData[j.Key] = j.Value;

                        }
                        if (s.Value.LinePublicNumber == "24" && (s.Value.TimingPointName == "Beurs" || s.Value.TimingPointName == "Keizerstraat" || s.Value.TimingPointName == "Stadhuis") && s.Value.TripStopStatus != "PASSED")
                        {
                            stopObject = s;

                            tramData[j.Key] = j.Value;

                        }
                        if (s.Value.LinePublicNumber == "25" && (s.Value.TimingPointName == "Beurs" || s.Value.TimingPointName == "Leuvehaven" || s.Value.TimingPointName == "Lijnbaan") && s.Value.TripStopStatus != "PASSED")
                        {
                            stopObject = s;

                            tramData[j.Key] = j.Value;

                        }

                    }

                }
            }
        }
        return tramData;
    }




}
