using UnityEngine;

public class View : MonoBehaviour
{
    public virtual bool isActive {
        set => gameObject.SetActive(value);
        get => gameObject.activeInHierarchy;
    }
}
