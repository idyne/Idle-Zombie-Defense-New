using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierImageRendererController : MonoBehaviour
{

    [SerializeField] private List<GameObject> soldiers = new List<GameObject>();
    [SerializeField] private GameObject _camera = null;
    [SerializeField] private GameObject soldierParent = null;

    private int soldierIndexShown = -1;

    public void ShowSoldier(int soldierIndex)
    {
        if (soldierIndexShown == -1)
        {
            if (soldiers.Count > soldierIndex)
            {
                soldiers[soldierIndex].SetActive(true);
                soldierIndexShown = soldierIndex;
                _camera.SetActive(true);
                soldierParent.SetActive(true);
            }
            else
                Debug.LogError("There is no soldier index to show.");
        }
        else
            Debug.LogError("There is already showing another soldier.");
    }

    public void CloseShow()
    {
        if (soldierIndexShown != -1)
        {
            soldiers[soldierIndexShown].SetActive(false);
            soldierIndexShown = -1;
            _camera.SetActive(false);
            soldierParent.SetActive(false);
        }
        else
            Debug.LogError("Already no soldiers are shown.");

    }
}
