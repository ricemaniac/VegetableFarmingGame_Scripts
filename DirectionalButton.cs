using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DirectionalButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public enum Direction { Up, Down, Left, Right }
    public Direction direction;

    [SerializeField] private PlayerController player;
    [SerializeField] private Tutorial_Walk tutorialWalk; // チュートリアル監視用

    private bool isPointerDown = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        if (tutorialWalk != null) tutorialWalk.PushDown();

        SetMovement(direction);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            GameObject hoveredObject = eventData.pointerCurrentRaycast.gameObject;
            DirectionalButton btn = hoveredObject.GetComponentInParent<DirectionalButton>();

            if (btn != null)
            {
                SetMovement(btn.direction);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isPointerDown)
        {
            isPointerDown = false;
            if (tutorialWalk != null) tutorialWalk.PushUp();
        }
        ResetMovement();
    }

    private void SetMovement(Direction dir)
    {
        if (player == null) return;

        // 一旦全リセットしてから、指が乗っている方向だけONにする
        player.SetDirectionInput(dir.ToString());
    }

    private void ResetMovement()
    {
        if (player == null) return;

        player.ResetAllInput();
    }
}
