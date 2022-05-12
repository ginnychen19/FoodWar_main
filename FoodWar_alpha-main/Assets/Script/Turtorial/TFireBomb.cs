using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TFireBomb : MonoBehaviour
{
    [SerializeField] Transform launchPoint;
    [SerializeField] float force = 10f;
    [SerializeField] float flySpeed = 1f;

    [SerializeField] Vector3 launchToPos;

    Vector3 randomPosOffset = Vector3.zero;
    [SerializeField] TurBomb tb;
    bool launch;
    private void Start()
    {
        tb = GetComponent<TurBomb>();

        randomPosOffset = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5));
    }
    private void Update()
    {
        ShootInput();
    }
    void ShootInput()
    {
        AimState state = GetAimState();
        
        if (state == AimState.Start)
        {
            launchToPos = CrossHair.instance.transform.position;
            tb.CheckVector(launchToPos);
        }
        if (state == AimState.Move)
        {
            launchToPos = CrossHair.instance.transform.position;
            tb.CheckVector(launchToPos);
        }
        if (state == AimState.Ended)
        {
            launchToPos = CrossHair.instance.transform.position;
            tb.CheckVector(launchToPos);
            tb.line.positionCount = 0;
        }
        if (Input.GetMouseButtonDown(0) && state == AimState.Move)
        {

            tb.ShootObj(launchToPos);
            HotBarForTur.instance.WeaponUse();



        }
        else if (Input.GetMouseButtonDown(0) && state == AimState.None)
        {
            launchToPos = CrossHair.instance.transform.position;


            tb.ShootObj(launchToPos + randomPosOffset);
            HotBarForTur.instance.WeaponUse();

        }


    }
    private AimState GetAimState()
    {

        if (Input.GetMouseButtonDown(1)) { return AimState.Start; }
        if (Input.GetMouseButton(1)) { return AimState.Move; }
        if (Input.GetMouseButtonUp(1)) { return AimState.Ended; }
        else
        {
            return AimState.None;
        }




    }
    private enum AimState
    {
        Start = 0,
        Move = 1,
        Stay = 2,
        Ended = 3,

        None = 9
    }
}
