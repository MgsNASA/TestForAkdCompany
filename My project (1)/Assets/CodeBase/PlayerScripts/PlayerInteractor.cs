using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public float interactRange = 3f;              // Максимальная дистанция для взаимодействия
    public LayerMask interactableLayer;           // Слой для взаимодействия
    public Color highlightColor = Color.yellow;   // Цвет выделения при наведении
    public Color heldColor = Color.green;         // Цвет, когда объект поднят

    private Camera playerCamera;                  // Ссылка на камеру игрока
    private GameObject heldObject;                // Объект, который игрок держит
    private Rigidbody heldObjectRb;               // Rigidbody удерживаемого объекта
    private Color originalColor;                  // Оригинальный цвет объекта для возврата
    private Renderer objectRenderer;              // Renderer объекта для изменения цвета
    private Vector3 holdOffset = new Vector3(0, 0, 2f); // Смещение позиции для удерживаемого объекта

    void Start()
    {
        playerCamera = Camera.main;

        if (playerCamera == null)
        {
            Debug.LogError("Основная камера (MainCamera) не найдена! Убедитесь, что в сцене есть камера с тегом 'MainCamera'.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Нажатие клавиши E
        {
            if (heldObject == null)
            {
                TryPickUpObject();
            }
            else
            {
                DropObject();
            }
        }

        if (heldObject != null)
        {
            MoveHeldObject();
        }
        else
        {
            HighlightObject();
        }
    }

    private void HighlightObject()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, interactableLayer))
        {
            Renderer hitRenderer = hit.collider.GetComponent<Renderer>();

            if (hitRenderer != null)
            {
                if (objectRenderer != null && objectRenderer != hitRenderer)
                {
                    objectRenderer.material.color = originalColor;
                }

                objectRenderer = hitRenderer;
                originalColor = objectRenderer.material.color;
                objectRenderer.material.color = highlightColor;
            }
        }
        else if (objectRenderer != null)
        {
            objectRenderer.material.color = originalColor;
            objectRenderer = null;
        }
    }

    private void TryPickUpObject()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, interactableLayer))
        {
            if (hit.collider != null && hit.collider.GetComponent<Rigidbody>() != null)
            {
                PickUpObject(hit.collider.gameObject);
            }
        }
    }

    private void PickUpObject(GameObject obj)
    {
        heldObject = obj;
        heldObjectRb = obj.GetComponent<Rigidbody>();

        if (heldObjectRb != null)
        {
            heldObjectRb.useGravity = false;
            heldObjectRb.velocity = Vector3.zero;  // Останавливаем объект

            objectRenderer = heldObject.GetComponent<Renderer>();
            if (objectRenderer != null)
            {
                originalColor = objectRenderer.material.color;
                objectRenderer.material.color = heldColor;
            }

            Debug.Log("Объект поднят: " + heldObject.name);
        }
        else
        {
            Debug.LogError("Не удалось найти Rigidbody у объекта.");
        }
    }

    private void MoveHeldObject()
    {
        // Устанавливаем объект перед игроком на заданное расстояние
        Vector3 targetPosition = playerCamera.transform.position + playerCamera.transform.TransformDirection(holdOffset);
        heldObject.transform.position = targetPosition;
        heldObject.transform.rotation = Quaternion.LookRotation(-playerCamera.transform.forward); // Повернуть объект к игроку
    }

    private void DropObject()
    {
        if (heldObjectRb != null)
        {
            heldObjectRb.useGravity = true;

            if (objectRenderer != null)
            {
                objectRenderer.material.color = originalColor;
                objectRenderer = null;
            }

            Debug.Log("Объект отпущен: " + heldObject.name);

            heldObject = null;
            heldObjectRb = null;
        }
    }
}
