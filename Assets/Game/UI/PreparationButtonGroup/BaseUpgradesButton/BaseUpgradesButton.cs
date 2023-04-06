using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseUpgradesButton : UIElement
{
    [SerializeField] private TutorialData tutorialData;
    [SerializeField] protected UnityEvent openRealPanel;
    [SerializeField] protected UnityEvent openFakePanel;

    public void CheckTutorial()
    {
        if (tutorialData.IsPassed())
        {
            openRealPanel.Invoke();
        }
        else
        {
            openFakePanel.Invoke();
        }
    }
}
