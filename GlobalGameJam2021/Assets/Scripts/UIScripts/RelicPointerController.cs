using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RelicPointerController : MonoBehaviour
{
    public Image targetSymbol;
    public Image arrow;

    public int positionOffset = 16;

    private int arrowOffset = 25;

    public bool isActive;

    public Relic target;

    [SerializeField] int angleOffset;

    private void Update()
    {
        if(isActive == true && !IsRelicVisibleOnScreen())
        {
            SetRelicPointerVisable(true);

            RotateArrow();
            MoveRelicPointerObject();
        }

        else
        {
            SetRelicPointerVisable(false);
        }
    }

    bool IsRelicVisibleOnScreen()
    {
        Vector2 cameraPosition = Camera.main.transform.position;
        Vector2 relicPosition = target.transform.position;

        float width = 320 / 2;
        float height = 180 / 2;
        float relicSize = 16 / 2;

        if (relicPosition.x < cameraPosition.x - (width + relicSize) ||
            relicPosition.x > cameraPosition.x + (width + relicSize) ||
            relicPosition.y < cameraPosition.y - (height + relicSize) ||
            relicPosition.y > cameraPosition.y + (height + relicSize))
        {
            return false;
        }

        return true;
    }

    public void SetRelicPointerVisable(bool show)
    {
        targetSymbol.enabled = show;
        arrow.enabled = show;
    }

    private void RotateArrow()
    {
        Vector3 targetDirection = target.transform.position - arrow.rectTransform.position;

        var angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg + angleOffset;

        arrow.rectTransform.rotation = Quaternion.AngleAxis(angle + angleOffset * 2, Vector3.forward);
    }

    private void MoveRelicPointerObject()
    {
        Vector2 cameraPosition = Camera.main.transform.position;
        Vector2 relicPosition = target.transform.position;

        float width = 320 / 2;
        float height = 180 / 2;
        float arrowSize = 16 / 2;

        float y = Mathf.Clamp(relicPosition.y, cameraPosition.y - (height - arrowSize), cameraPosition.y + (height - arrowSize));
        float x = Mathf.Clamp(relicPosition.x, cameraPosition.x - (width - arrowSize), cameraPosition.x + (width - arrowSize));

        transform.position = new Vector3(x, y, 0);
    }
}
