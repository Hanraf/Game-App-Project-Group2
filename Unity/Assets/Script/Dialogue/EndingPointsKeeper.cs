using UnityEngine;

public class EndingPointsKeeper : MonoBehaviour
{
    private static EndingPointsKeeper instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
