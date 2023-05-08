
using UnityEngine;

public class NoInternetPanel : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    public void Show()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            canvas.enabled = true;
        }
    }
    public void Retry()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            canvas.enabled = false;
        }
    }
}
