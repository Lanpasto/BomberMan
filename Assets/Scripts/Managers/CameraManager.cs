using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private LevelPropetries levelPropetries;
 
    private int cameraPadding = 3;

    void Start()
    {
   
        Camera mainCamera = Camera.main;

        if (mainCamera != null && levelPropetries != null)
        {
            // Визначається розмір камери як максимальний розмір мапи плюс відступи
            int cameraWidth = levelPropetries.width + cameraPadding * 2;
            int cameraHeight = levelPropetries.height + cameraPadding * 2;

            // Отримуємо координати центра мапи
            float mapCenterX = (float)(levelPropetries.width - 1) / 2.0f;
            float mapCenterY = (float)(levelPropetries.height - 1) / 2.0f;

            // Встановлюємо розмір та позицію мапb
            mainCamera.orthographicSize = Mathf.Max(cameraWidth / 2.0f, cameraHeight / 2.0f);
            mainCamera.transform.position = new Vector3(mapCenterX, mapCenterY, mainCamera.transform.position.z);
        }
        else
        {
            Debug.LogError("Main camera or LevelPropetries object not assigned!");
        }
    }
}

