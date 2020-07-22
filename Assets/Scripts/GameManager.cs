using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public MultiAdButton adButton;
    public GameObject startButton;
    public Image heart;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Vector2 startingPosition;
    public Movement movement;
    public GameObject rewardPanel;

    void Start()
    {
        heart.sprite = fullHeart;
        rewardPanel.SetActive(false);
        ResetGame();
    }

    public void StartGame()
    {
        startButton.SetActive(false);
        movement.speed = 3;
    }

    public void GrantReward()
    {
        heart.sprite = fullHeart;
        rewardPanel.SetActive(true);
        ResetGame();
    }

    public void HandleError()
    {
        Debug.Log("Error!");
    }


    public void OnGameOver()
    {
        heart.sprite = emptyHeart;
        movement.speed = 0;
        movement.gameObject.SetActive(false);
        adButton.EnableButton();
    }

    void ResetGame()
    {
        movement.transform.position = startingPosition;
        movement.speed = 0;
        movement.gameObject.SetActive(true);
        startButton.SetActive(true);
    }
}