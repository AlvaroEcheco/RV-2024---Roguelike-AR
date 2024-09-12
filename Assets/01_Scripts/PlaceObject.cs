using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlaceObject : MonoBehaviour
{

    ARRaycastManager raycastManager;
    ARPlaneManager planeManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public Canvas canvas;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
        canvas.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void FingerDown(EnhancedTouch.Finger finger)
    {
        // Permitir el primer toque (índice 0)
        if (finger.index != 0) return;

        // Realizar un raycast en la posición del toque
        if (raycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            foreach (var hit in hits)
            {
                Pose pose = hit.pose;

                // Desactivar el plane manager para dejar de detectar planos
                planeManager.enabled = false;
                foreach (var plane in planeManager.trackables)
                {
                    plane.gameObject.SetActive(false); // Ocultar los planos detectados
                }

                dungeonManager.instance.dungeonPoint.transform.position = pose.position;
                dungeonManager.instance.player.transform.position += pose.position;

                dungeonManager.instance.dungeonPoint.gameObject.SetActive(true);
                dungeonManager.instance.player.gameObject.SetActive(true);
                FindObjectOfType<Sword>().gameObject.SetActive(true);

                canvas.gameObject.SetActive(true);
                enabled = false;

                return;
            }
        }
    }
}
