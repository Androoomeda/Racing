using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public Vector3 start;
    [HideInInspector] public Vector3 moveDirection;

    [SerializeField] private Transform loseZone;
    [SerializeField] private float maxLengthArrow;
    [SerializeField] private float speedMove;
    [SerializeField] private Text strengthText;

    private float maxTensionStrength;
    private Rigidbody rb;
    private LineRenderer lineRenderer;
    private Vector2 touchStartPos;
    private Vector3 lastPos;
    

    private void Awake()
    {
        start = transform.position;
        lineRenderer = GetComponentInChildren<LineRenderer>();
        rb = GetComponent<Rigidbody>();
        maxTensionStrength = Screen.height / 2;
    }

    private void Update()
    {
        CheckInput();
        CheckLose();
    }

    private void CheckInput()
    {
        CheckTouches();
#if UNITY_EDITOR
        CheckClickes();
#endif
    }

    private void CheckTouches()
    {
        if (Input.touchCount > 0 && !UI.pause)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
            }
            if (touch.position.y > Screen.height / 2 || touchStartPos.y > Screen.height / 2)
            {
                moveDirection = Vector3.zero;
                UpdateStrengthProgress();
                lineRenderer.SetPosition(1, Vector3.zero);
                return;
            }

            if (touch.position.y <= touchStartPos.y)
                moveDirection = touchStartPos - touch.position;

            if (touch.phase == TouchPhase.Moved && moveDirection.magnitude <= maxTensionStrength)
            {
                int strengthProgress = UpdateStrengthProgress();
                ShowTrajectory(strengthProgress);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                lastPos = transform.position;
                MoveCar();
            }
        }
    }

    private void CheckClickes()
    {
        if (Input.GetMouseButton(0) && !UI.pause)
        {
            Vector2 mousePos = Input.mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                touchStartPos = mousePos;
            }
            if (mousePos.y > Screen.height / 2 || touchStartPos.y > Screen.height / 2)
            {
                moveDirection = Vector3.zero;
                UpdateStrengthProgress();
                lineRenderer.SetPosition(1, Vector3.zero);
                return;
            }

            if (mousePos.y <= touchStartPos.y)
                moveDirection = touchStartPos - mousePos;

            if (moveDirection.magnitude <= maxTensionStrength)
            {
                int strengthProgress = UpdateStrengthProgress();
                ShowTrajectory(strengthProgress);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            lastPos = transform.position;
            MoveCar();
        }
    }

    private int UpdateStrengthProgress()
    {
        int progress = Mathf.RoundToInt(moveDirection.magnitude / maxTensionStrength * 100);
        strengthText.text = "Сила натяжения: " + progress.ToString() + "%";

        return progress;
    }

    private void ShowTrajectory(int strengthProgress)
    {
        Vector3 position = moveDirection.normalized * (maxLengthArrow / 100 * strengthProgress);
        Vector3 pointPos = new Vector3(position.x , 0, position.y);
        lineRenderer.SetPosition(1, pointPos);
    }

    private void MoveCar()
    {
        Vector3 carDirection = new Vector3(moveDirection.x, 0, moveDirection.y).normalized * speedMove;
        rb.velocity = carDirection;
        lineRenderer.SetPosition(1, Vector3.zero);
        strengthText.text = "Сила натяжения: 0%";
    }

    private void CheckLose()
    {
        if(transform.position.y < loseZone.position.y)
        {
            rb.velocity = Vector3.zero;
            transform.rotation = Quaternion.Euler(Vector3.zero);
            transform.position = lastPos;
        }
    }
}
