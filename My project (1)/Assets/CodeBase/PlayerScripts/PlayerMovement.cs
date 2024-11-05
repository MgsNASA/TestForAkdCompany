using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;               // Скорость передвижения

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        // Получаем ввод от клавиатуры
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Направление движения относительно игрока
        Vector3 direction = transform.right * horizontal + transform.forward * vertical;

        // Применяем движение с учетом CharacterController
        controller.Move(direction * speed * Time.deltaTime);
    }
}
