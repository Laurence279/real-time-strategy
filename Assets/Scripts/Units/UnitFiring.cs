using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFiring : NetworkBehaviour
{
    [SerializeField] private Targeter targeter = null;
    [SerializeField] private GameObject projectilePrefab = null;
    [SerializeField] private Transform projectileSpawn = null;
    [SerializeField] private float fireRange = 5f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float rotateSpeed = 20f;

    private float lastTimeFired;

    [ServerCallback]
    private void Update()
    {
        Targetable target = targeter.GetTarget();
        if(target == null) { return; }
        if(!CanFireAtTarget()) { return; }

        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        if (Time.time > (1/fireRate) + lastTimeFired)
        {
            Quaternion projectileRotation = Quaternion.LookRotation(target.GetAimAtPoint().position - projectileSpawn.position);
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.position, projectileRotation);

            NetworkServer.Spawn(projectile, connectionToClient);

            lastTimeFired = Time.time;
        }
    }

    [Server]
    private bool CanFireAtTarget()
    {
        return (targeter.GetTarget().transform.position - transform.position).sqrMagnitude <= fireRange * fireRange;
    }

}
