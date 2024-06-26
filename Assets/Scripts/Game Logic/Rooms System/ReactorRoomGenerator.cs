﻿using UnityEngine;

/// <summary>
/// Генерация комнаты с временным реактором, которая будет находиться в самом низу лестницы
/// </summary>
public class ReactorRoomGenerator : MonoBehaviour
{
    [SerializeField] private GameObject roomPrefab;
    // Позиция создания финальной комнаты (совмещённая с центром комнаты) находится в виде пустого GameObject
    // в нижней лестничной структуре ("Lower Structure")
    [SerializeField] private Transform spawnPoint;
    // Стена со входом в комнату с временным реактором, которая препятствует дальнейшему спуску
    [SerializeField] private GameObject stopWall;

    // Границы для генерации номера самого нижнего этажа лестницы (отрицательные)
    [SerializeField] private int minBoundOfLastFloorNumber;
    [SerializeField] private int maxBoundOfLastFloorNumber;

    // Направление игрока по оси Z (вверх или вниз по лестнице), когда он входит в триггер передвижения структур ступеней
    private float onColliderEnterZAxisValue;
    private System.Random random = new System.Random();
    private bool finalRoomWasGenerated = false;

    private void Awake()
    {
        GenerateGeneralFloorCount();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "Player")
        {
            return;
        }

        // Финальная комната создаётся на последнем этаже лестницы, когда игрок проходит триггер генератора
        if (GameProperties.FloorNumber == GameProperties.LastFloorNumber)
        {
            var playerController = other.gameObject.GetComponent<PlayerController>();
            onColliderEnterZAxisValue = playerController.PlayerVelocity.z;

            if (!finalRoomWasGenerated)
            {
                GenerateFinalRoom();
                finalRoomWasGenerated = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name != "Player" || GameProperties.FloorNumber != GameProperties.LastFloorNumber)
        {
            return;
        }

        var playerController = other.gameObject.GetComponent<PlayerController>();

        // Проверка на соответсвие одному направлению входа и выхода (игрок полностью прошёл триггер) для того,
        // чтобы сделать активной/неактивной стену, загораживающую дальнейший спуск на последнем этаже
        if (playerController.PlayerVelocity.z > 0f && onColliderEnterZAxisValue > 0f)
        {
            stopWall.SetActive(false) ;
        }
        else if (playerController.PlayerVelocity.z < 0f && onColliderEnterZAxisValue < 0f)
        {
            stopWall.SetActive(true);
        }
    }

    /// <summary>
    /// Сгенерировать количество этажей на лестнице
    /// </summary>
    private void GenerateGeneralFloorCount()
    {
        int generatedFloorCount = random.Next(minBoundOfLastFloorNumber, maxBoundOfLastFloorNumber + 1);
        GameProperties.LastFloorNumber = generatedFloorCount;
    }

    /// <summary>
    /// Создать и спозиционировать финальную комнату с реактором внизу лестницы
    /// </summary>
    private void GenerateFinalRoom()
    {
        var room = Instantiate(roomPrefab);
        room.transform.position = spawnPoint.position;
    }
}