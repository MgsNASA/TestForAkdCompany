using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;  // Чувствительность мыши
    private Transform playerCamera;        // Ссылка на камеру
    private float xRotation = 0f;          // Поворот камеры по оси X

    void Start()
    {
      
        playerCamera = Camera.main.transform;

        // Блокируем курсор в центре экрана
        Cursor.lockState = CursorLockMode.Locked; 
    }

    void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        // Получаем ввод мыши
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Поворачиваем игрока по горизонтали (влево-вправо)
        transform.Rotate(Vector3.up * mouseX);

        // Ограничиваем поворот камеры по вертикали (вверх-вниз)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Применяем вращение по вертикали только к камере
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
