using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : UIElement
{
    [SerializeField] private GameManager gameManager;
    [SerializeReference] private TutorialData tutorialData;
    private RectTransform targetObject = null;
    [SerializeField] private bool anchorCenter = false;

    protected override void Awake()
    {
        base.Awake();
        targetObject = transform.parent.GetComponent<RectTransform>();
    }

    public void CheckConditions()
    {
        if (tutorialData.IsItSuitableAndUnpassed())
        {
            gameManager.PauseGame();
            Show();
            TutorialHighlighter.Instance.Highlight(targetObject, anchorCenter);

        }
    }

    public void Pass()
    {
        if (tutorialData.IsItSuitableAndUnpassed())
        {
            tutorialData.Pass();
            Hide();
            TutorialHighlighter.Instance.Dehighlight(targetObject);
            gameManager.ResumeGame();
        }
    }
}
