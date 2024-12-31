using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectPlacer : MonoBehaviour {

    public static ObjectPlacer Instance;

    public LayerMask placementLayer; // Set the layers you want to place on
    public bool isBuilding;
    private bool isColliding;
    private GameObject currentPrefab; // Holds the prefab passed by the button
    private GameObject previewPrefab;
    private GameObject previewObject; // Preview object that follows the mouse

    private void Awake() {
        if (Instance) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    void OnEnable() {
        // Subscribe to the event
        BuildingCollision.OnCollisionHappened += ResponseToCollision;
        BuildingCollision.OnCollisionEnded += ResponseToCollisionExit;
    }

    void Update() {
        // Update preview object's position
        if (isBuilding && previewObject == null) {
            InstantiatePreview();
        }
        else if (isBuilding && previewObject != null) {
            UpdatePreviewPosition();
        }

        // Place prefab on left mouse click
        if (Input.GetMouseButtonDown(0) && isBuilding && !EventSystem.current.IsPointerOverGameObject() && !isColliding) {
            PlacePrefab();
        }

        // Cancel building on Escape
        if (Input.GetKeyDown(KeyCode.Escape)) {
            CancelBuilding();
        }
    }

    public void SetPrefabFromSO(BuildingSO buildingSO) {
        if (previewObject != null) {
            Destroy(previewObject);
        }

        
        currentPrefab = buildingSO.mainPrefab;

        // Create the preview object
        previewPrefab = buildingSO.previewPrefab;
        isBuilding = true;
    }

    private void UpdatePreviewPosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, placementLayer)) {
            previewObject.transform.position = hit.point;
        }
    }

    public void PlacePrefab() {
        if (previewObject == null || currentPrefab == null) {
            Debug.LogWarning("No prefab or preview object available for placement!");
            return;
        }

        Instantiate(currentPrefab, previewObject.transform.position, previewObject.transform.rotation);

    }

    public void CancelBuilding() {
        isBuilding = false;
        currentPrefab = null;

        if (previewObject != null) {
            Destroy(previewObject);
        }
    }

    private void DisablePhysics(GameObject obj) {
        foreach (var rb in obj.GetComponentsInChildren<Rigidbody>()) {
            rb.isKinematic = true;
        }
        foreach (var collider in obj.GetComponentsInChildren<Collider>()) {
            collider.enabled = false;
        }
    }

    private void InstantiatePreview() {

        previewObject = Instantiate(previewPrefab);
    }

    private void ResponseToCollision() {
        isColliding = true;
    }

    private void ResponseToCollisionExit() {
        isColliding = false;
    }
}

