using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : UIElement
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform container;

    private void Start()
    {
        Highlight();
    }
    public override void Hide()
    {
        base.Hide();
        Dehightlight();
    }
    public override void OnHighlight()
    {
        base.OnHighlight();
        animator.enabled = true;
        animator.SetTrigger("Highlight");
    }

    public override void OnDehighlight()
    {
        base.OnDehighlight();
        animator.SetTrigger("Dehighlight");
        //animator.enabled = false;
    }
}
