using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurGun : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float upwardForce;
    [SerializeField] float timeBetweenShooting;
    float shootForce = 100;
    [SerializeField] float timeBetweenShoot;
    [SerializeField] Transform firePoint;
    [SerializeField] Camera cam;
    [SerializeField] CrossHair crossHair;
    bool readyToShoot;
    private void Start()
    {
        cam = Camera.main;
        crossHair = cam.gameObject.GetComponentInChildren<CrossHair>();
        readyToShoot = true;
        
    }
    void ShootInput()
    {
        if (Input.GetMouseButton(0) && readyToShoot)
        {
            Shoot();
        }
    }
    private void Update()
    {
        ShootInput();
    }
    void Shoot()
    {
        readyToShoot = false;
        Vector3 targetPoint = crossHair.gameObject.transform.position;

        Vector3 shootDir = targetPoint - firePoint.position;

        GameObject currentBullet = Instantiate(bullet, firePoint.position, Quaternion.identity);
        currentBullet.transform.forward = shootDir.normalized;
        currentBullet.GetComponent<Rigidbody>().AddForce(shootDir.normalized * shootForce, ForceMode.Impulse);
        HotBarForTur.instance.WeaponUse();
        Invoke("ResetShoot", 0.1f);
    }
    void ResetShoot()
    {
        readyToShoot = true;
    }
}
