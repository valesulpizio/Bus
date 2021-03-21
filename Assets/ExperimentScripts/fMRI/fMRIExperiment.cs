using System.Collections;
using UnityEngine;

public class fMRIExperiment : Experiment<fMRITrial>
{
    // User-settable values
    [Header("Bus experiment setup")]

    [Tooltip("Position of the camera at the beginning of the trial.")]
    public float InitialX_CameraPosition = 0f;

    [Tooltip("Position of the first bus on the road at the beginning of the trial.")]
    public float InitialX_BusPosition = 355f;

    // References to game objects
    private GameObject BusLine;
    private Rigidbody BusLineRigidBody;
    private GameObject cameraObject;
    private Rigidbody cameraRigidBody;

    override protected IEnumerator InitializeExperiment()
    {
        // Get reference to bus
        BusLine = GameObject.Find("BusLine");
        BusLineRigidBody = BusLine.GetComponent<Rigidbody>();

        // Get reference to camera
        cameraObject = GameObject.Find("Camera");
        cameraRigidBody = cameraObject.GetComponent<Rigidbody>();

        // Initialize visibility
        BusLine.SetActive(false);

        yield return null;
    }


    override protected void DestroyExperiment()
    {
        BusLine.SetActive(true);
    }


    override protected IEnumerator StartExperiment()
    {
        yield return null;
    }


    override protected IEnumerator ConcludeExperiment()
    {
        yield return null;
    }


    override protected IEnumerator RunTrial(fMRITrial currentTrial)
    {
        // FIRST TRIAL PERIOD

        // Reset camera position
        cameraObject.transform.position = new Vector3(InitialX_CameraPosition, cameraObject.transform.position.y, 0);

        BusLine.transform.position = new Vector3(InitialX_BusPosition, 0, 25);

        // Set Structure        
        BusLine.SetActive(true);

        // Set camera and bus speed
        cameraRigidBody.velocity = new Vector3(currentTrial.SM_velocity * 0.277778f, 0, 0);
        BusLineRigidBody.velocity = new Vector3(currentTrial.OM_velocity * 0.277778f,0, 0);

        // Save current time
        currentTrial.StartTime = Time.time;

        // Wait for TrialDuration seconds
        yield return new WaitForSeconds(currentTrial.duration);

        // END OF TRIAL

        // Stop camera and bus, hide bus
        BusLineRigidBody.velocity = new Vector3(0, 0, 0);
        cameraRigidBody.velocity = new Vector3(0, 0, 0);
        BusLine.SetActive(false);
       
        // Save current time
        currentTrial.EndTime = Time.time;

        // Wait a minimum intertrial time
        yield return new WaitForSeconds(0);

        // Wait for response within a timeout
        yield return Response.Wait(currentTrial.ITI_duration - 0);

        // Wait for the subject to release response keys
        yield return Response.WaitRelease();

        // Check if valid response
        if (currentTrial.ResponseTime <= currentTrial.ChangeTime)
            currentTrial.Valid = false;
               
    }


    override protected void UpdateExperiment()
    {
    }
}