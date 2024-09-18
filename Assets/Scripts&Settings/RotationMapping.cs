using UnityEngine;
using Vuforia;

public class RotationMapping : MonoBehaviour {
    public GameObject trackedObjectA;
    public GameObject objectB;
    private ObserverBehaviour observerA;

    void Start() {
        if (trackedObjectA != null) {
            observerA = trackedObjectA.GetComponent<ObserverBehaviour>();
        }

        if (observerA != null) {
            observerA.OnTargetStatusChanged += OnTrackingStatusChanged;
        }
    }

    private void OnDestroy() {
        if (observerA != null) {
            observerA.OnTargetStatusChanged -= OnTrackingStatusChanged;
        }
    }

    private void OnTrackingStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus) {
        if (targetStatus.Status == Status.TRACKED || targetStatus.Status == Status.EXTENDED_TRACKED) {
            UpdateRotation();
        }
        else {
            // objectB.transform.rotation = Quaternion.identity;
        }
    }

    void UpdateRotation() {
        if (trackedObjectA != null && objectB != null) {
            objectB.transform.rotation = trackedObjectA.transform.rotation;
        }
    }
}