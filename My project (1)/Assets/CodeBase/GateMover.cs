using UnityEngine;

public class GateMover : MonoBehaviour
{
    public GameObject gateObject;
    public float moveDistance = 5f;
    public float moveSpeed = 2f;

    private Vector3 initialPosition;
    private Vector3 openPosition;
    private bool isMoving = false;
    private bool isOpen = false;

    void Start()
    {
        if (gateObject == null)
        {
            Debug.LogError("Gate object не назначен! Укажите дочерний объект, который будет перемещаться.");
            return;
        }

        initialPosition = gateObject.transform.localPosition;
        openPosition = initialPosition + new Vector3(0, 0, moveDistance);
        ToggleGate();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            ToggleGate();
        }

        if (isMoving)
        {
            Vector3 targetPosition = isOpen ? initialPosition : openPosition;
            gateObject.transform.localPosition = Vector3.MoveTowards(gateObject.transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);

            if (gateObject.transform.localPosition == targetPosition)
            {
                isMoving = false;
                isOpen = !isOpen;
            }
        }
    }

    private void ToggleGate()
    {
        isMoving = true;
    }
}