using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public Button playButton;
    public GameObject panel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start() {
        print("Started");
        print(playButton);
        playButton.onClick.AddListener(PlayGame);
    }

    public void PlayGame() {
        print("Clicked");

        SceneManager.LoadSceneAsync(1);
        print("Opened");
    }
}
