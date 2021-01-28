using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RelicPointerController : MonoBehaviour
{
    public Image targetSymbol;
    public Image arrow;
    public bool isActive;

    public Transform target;

    [SerializeField] int angleOffset;

    private void Update()
    {
        if (isActive == true)
        {
            gameObject.SetActive(true);
            RotateArrow();
        }
        else
            gameObject.SetActive(false);
    }

    private void RotateArrow()
    {
        Vector3 targetDirection = target.position - arrow.rectTransform.position;

        var angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg + angleOffset;

        arrow.rectTransform.rotation = Quaternion.AngleAxis(angle + angleOffset * 2, Vector3.forward);
    }

    public void EnableRelicArrow(RelicPointerController relicArrowObject)
    {
        relicArrowObject.isActive = true;
    }

    public void DisableRelicArrow(RelicPointerController relicArrowObject)
    {
        relicArrowObject.isActive = false;
    }

}
