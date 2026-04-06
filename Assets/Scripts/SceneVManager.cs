using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneVManager : MonoBehaviour
{
    public GameObject[] enemies;

    public float delay = 3f;

    public TextMeshProUGUI Mission;

    void Update()
    {
        bool allEnemiesInactive = true;
        foreach (GameObject enemy in enemies)
        {
            if (enemy.activeSelf)
            {
                allEnemiesInactive = false;
                break;
            }
        }

        if (allEnemiesInactive)
        {
            if (Mission != null)
            {
                Mission.color = Color.gray;
                Mission.text = "<s>" + Mission.text + "</s>";
            }
            StartCoroutine(RestartSceneWithDelay(delay));
        }
    }

    IEnumerator RestartSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        GameObject[] allObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject obj in allObjects)
        {
            obj.transform.position = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("MainMenu");
    }
}