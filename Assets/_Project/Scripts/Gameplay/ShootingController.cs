using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Character;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [SerializeField] private Camera raycastCamera;
    [Space(10)]
    [SerializeField] private float shootingForce;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }


    public void Shoot()
    {
        var ray = raycastCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return;
        }
        
        var ragdollController = raycastHit.collider.GetComponentInParent<RagdollController>();
        if (ragdollController == null)
        {
            return;
        }
        
        var forceDirection = ragdollController.transform.position - raycastCamera.transform.position;
        forceDirection.y = 0;
        forceDirection.Normalize();

        var force = shootingForce * forceDirection;
            
        ragdollController.PushCharacter(force, raycastHit.point);
    }
}
