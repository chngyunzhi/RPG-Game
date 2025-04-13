using TMPro;
using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    [SerializeField] private float xLimit = 960;
    [SerializeField] private float yLimit = 540;

    [SerializeField] private float xOffset = 150;
    [SerializeField] private float yOffset = 150;
    private bool adjustedFontSize = false;

    public virtual void AdjustPosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        float newXOffset = 0;
        float newYOffset = 0;

        if (mousePosition.x > xLimit)
            newXOffset = -xOffset;
        else
            newXOffset = xOffset;

        if (mousePosition.y > yLimit)
            newYOffset = -yOffset;
        else
            newYOffset = yOffset;

        transform.position = new Vector2(mousePosition.x + newXOffset, mousePosition.y + newYOffset);

    }

    public void AdjustFontSize(TextMeshProUGUI _text)
    {
        adjustedFontSize = true;

        if (_text.text.Length > 12 && !adjustedFontSize)
            _text.fontSize = _text.fontSize * .8f;

    }
}
