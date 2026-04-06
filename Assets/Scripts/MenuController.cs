using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject ChapterI;
    public GameObject ChapterII;
    public GameObject ChapterIII;
    public GameObject ChapterIV;
    public GameObject ChapterV;
    public void Exit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                    Application.Quit();
        #endif
    }

    public void PlayChapterIV()
    {
        SceneManager.LoadScene("Chapter IV");
    }
    
    public void PlayChapterV()
    {
        SceneManager.LoadScene("Chapter V");
    }

    public void mainMenu()
    {
        MainMenu.SetActive(true);
        ChapterI.SetActive(false);
        ChapterII.SetActive(false);
        ChapterIII.SetActive(false);
        ChapterIV.SetActive(false);
        ChapterV.SetActive(false);
    }

    public void chapterI()
    {
        MainMenu.SetActive(false);
        ChapterI.SetActive(true);
        ChapterII.SetActive(false);
        ChapterIII.SetActive(false);
        ChapterIV.SetActive(false);
        ChapterV.SetActive(false);
    }
    public void chapterII()
    {
        MainMenu.SetActive(false);
        ChapterI.SetActive(false);
        ChapterII.SetActive(true);
        ChapterIII.SetActive(false);
        ChapterIV.SetActive(false);
        ChapterV.SetActive(false);
    }
    public void chapterIII()
    {
        MainMenu.SetActive(false);
        ChapterI.SetActive(false);
        ChapterII.SetActive(false);
        ChapterIII.SetActive(true);
        ChapterIV.SetActive(false);
        ChapterV.SetActive(false);
    }
    public void chapterIV()
    {
        MainMenu.SetActive(false);
        ChapterI.SetActive(false);
        ChapterII.SetActive(false);
        ChapterIII.SetActive(false);
        ChapterIV.SetActive(true);
        ChapterV.SetActive(false);
    }public void chapterV()
    {
        MainMenu.SetActive(false);
        ChapterI.SetActive(false);
        ChapterII.SetActive(false);
        ChapterIII.SetActive(false);
        ChapterIV.SetActive(false);
        ChapterV.SetActive(true);
    }
}