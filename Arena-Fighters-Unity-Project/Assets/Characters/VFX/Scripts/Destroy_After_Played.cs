using UnityEngine;

public class Destroy_After_Played : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject, 1.3f);
    }
}
