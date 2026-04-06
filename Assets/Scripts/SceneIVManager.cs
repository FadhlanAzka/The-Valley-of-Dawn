using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneIVManager : MonoBehaviour
{
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;

    public GameObject[] objectsToReset;

    public float delay = 5f;

    public TextMeshProUGUI Mission;

    void Update()
    {
        if (!Enemy1.activeSelf && !Enemy2.activeSelf && !Enemy3.activeSelf)
        {
            if (Mission != null)
            {
                Mission.text = "<s>" + Mission.text + "</s>";
                Mission.color = Color.gray;
            }
            StartCoroutine(RestartSceneWithDelay(delay));
        }
    }

    IEnumerator RestartSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (GameObject obj in objectsToReset)
        {
            obj.transform.position = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("Chapter V");
    }
}
