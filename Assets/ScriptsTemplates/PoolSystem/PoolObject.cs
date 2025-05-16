using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public bool isActive { get; private set; }
    public string poolTag { get; set; }

    public virtual void OnCreate()
    {
        isActive = true;
        gameObject.SetActive(true);
    }

    public virtual void OnRecycle()
    {
        isActive = false;
        gameObject.SetActive(false);
    }

    public virtual void OnSpawn()
    {
        isActive = true;
        gameObject.SetActive(true);
    }

    public virtual void OnDespawn()
    {
        isActive = false;
        gameObject.SetActive(false);
    }
}