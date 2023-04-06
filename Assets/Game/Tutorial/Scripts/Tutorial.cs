using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : UIElement
{
    [SerializeField] private GameManager gameManager;
    [SerializeReference] private TutorialData tutorialData;
    private Transform targetObject = null;

    private void Awake()
    {
        targetObject = transform.parent;
    }

    public void CheckConditions()
    {
        if (tutorialData.IsItSuitableAndUnpassed())
        {
            gameManager.PauseGame();
            Show();
            Highlighter.Instance.Highlight(targetObject);

        }
    }

    public void Pass()
    {
        if (tutorialData.IsItSuitableAndUnpassed())
        {
            tutorialData.Pass();
            Hide();
            Highlighter.Instance.Dehighlight(targetObject);
            gameManager.ResumeGame();
        }
    }
}
