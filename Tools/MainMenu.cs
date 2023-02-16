using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MainMenu : MonoBehaviour
{
    Button newGameBtn;
    Button resumeBtn;
    Button exitBtn;
    PlayableDirector director;
    void Awake()
    {
        newGameBtn = transform.GetChild(1).GetComponent<Button>();
        resumeBtn = transform.GetChild(2).GetComponent<Button>();
        exitBtn = transform.GetChild(3).GetComponent<Button>();

        newGameBtn.onClick.AddListener(PlayTmieLine);
        resumeBtn.onClick.AddListener(ResumeGame);
        exitBtn.onClick.AddListener(QuitGame);

        
        director = FindObjectOfType<PlayableDirector>();
        director.stopped += NewGame;
    }
    void PlayTmieLine()
    {
        director.Play();   
    
    }
    void NewGame(PlayableDirector obj)
    {
        PlayerPrefs.DeleteAll();
        //ÇÐ»»³¡¾° 
        SceneController.Instance.TransitionToFirstLevel();
    }
    void ResumeGame()
    { 
        SceneController.Instance.TransitionToLoadGame();  
       
    }

    void QuitGame()
    { 
        Application.Quit();
    }
}
