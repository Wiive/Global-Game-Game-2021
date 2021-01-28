using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RelicPointerController : MonoBehaviour
{
    public Image targetSymbol;
    public Image arrow;

    public Transform target;

    public float speed = 1.0f;
    [SerializeField] int angleOffset;

    private void Update()
    {
        Vector3 targetDirection = target.transform.position - Camera.main.WorldToScreenPoint(arrow.rectTransform.position);

        var angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg + angleOffset;

        Debug.DrawRay(targetDirection * 10000, arrow.rectTransform.position, Color.red);

        //float singleStep = speed * Time.deltaTime;

        //Vector3 newDirection = Vector3.RotateTowards(arrow.transform.up, targetDirection, singleStep, 0.0f);

        //Debug.DrawRay(arrow.transform.position, newDirection, Color.red);

        arrow.rectTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //arrow.transform.rotation = Quaternion.LookRotation(newDirection);
    }



}
