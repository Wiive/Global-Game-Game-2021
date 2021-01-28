using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RelicPointerController : MonoBehaviour
{
    public Image targetSymbol;
    public Image arrow;

    public GameObject target;

    public float speed = 1.0f;

    private void Update()
    {
        Vector3 targetDirection = target.transform.position - arrow.transform.position;

        float singleStep = speed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(arrow.transform.forward, targetDirection, singleStep, 0.0f);

        Debug.DrawRay(arrow.transform.position, newDirection, Color.red);

        arrow.transform.rotation = Quaternion.LookRotation(newDirection);
    }



}
