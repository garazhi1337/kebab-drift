using UnityEngine;
using Unity.XR.CoreUtils;

public class XRCameraSetup : MonoBehaviour
{
    [SerializeField] private Transform _cameraDesiredPlace;

    void Start()
    {
        Invoke(nameof(ForceCameraToPose), 0.1f);
    }

    public void ForceCameraToPose()
    {
        XROrigin xrOrigin = GetComponent<XROrigin>();

        if (xrOrigin != null && xrOrigin.Camera != null)
        {
            // Get the camera's current world position and rotation
            Transform cameraTransform = xrOrigin.Camera.transform;
       

            // Use the XROrigin method to teleport the camera
            xrOrigin.MoveCameraToWorldLocation(_cameraDesiredPlace.position);

            // Match the origin's forward direction to align the camera
            xrOrigin.MatchOriginUpCameraForward(Vector3.up, cameraTransform.forward);

            Debug.Log("XR Camera has been forced to its scene position.");
        }
        else
        {
            Debug.LogError("XROrigin or Camera not found!");
        }
    }
}