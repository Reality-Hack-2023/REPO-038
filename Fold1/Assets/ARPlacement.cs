using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacement : MonoBehaviour
{
    public GameObject arObjectToSpawn;
    public GameObject placementIndicator;
    public GameObject spawnedObject;

    private Pose PlacementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false; 

    
    void Start()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
    }

 
    void Update()
    {
        if(spawnedObject == null && placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ARPlaceObject();
        }

        UpdatePlacementIndicator();
        UpdatePlacementPose();
       
    }

    void UpdatePlacementIndicator() 
    {
        if (spawnedObject == null && placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
        }
        else 
        {
            placementIndicator.SetActive(false);
        }
    }

    void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            PlacementPose = hits[0].pose;
        }
    }

    void ARPlaceObject()
    {
        spawnedObject = Instantiate(arObjectToSpawn, PlacementPose.position, PlacementPose.rotation);

    }
}