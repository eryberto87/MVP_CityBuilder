using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildingCollision : MonoBehaviour
{
    [SerializeField]
    private GameObject previewVisual, cannotBuildVisual;
    
    public static event Action OnCollisionHappened;
    public static event Action OnCollisionEnded;

    void Start() {
        // Activate one cube and deactivate the other
        previewVisual.SetActive(true);
        cannotBuildVisual.SetActive(false);
    }

    public void OnCollisionEnter(Collision other) {
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Terrain")) {
                //Debug.Log("Triggered by: " + other.gameObject.name);
                //previewVisual.SetActive(false);
                cannotBuildVisual.SetActive(true);
                OnCollisionHappened?.Invoke();

            }

            
        }
    }

    public void OnCollisionExit(Collision other) {
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Terrain")) {
                //Debug.Log("Exit collision with: " + other.gameObject.name);
                cannotBuildVisual.SetActive(false);
                OnCollisionEnded?.Invoke();
            }
        }
    }
}
