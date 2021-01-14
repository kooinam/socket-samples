using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : Singleton<ResourcesManager>
{
    private GameObject canvasGO = null;

    [SerializeField]
    private GameObject errorPopupPrefab = null;
    public GameObject ErrorPopupPrefab {
        get {
            return this.errorPopupPrefab;
        }
    }
    protected override void Awake()
    {
        base.Awake();

        this.Setup();
    }

    public void Setup() {
        this.canvasGO = GameObject.FindGameObjectWithTag("MainCanvas");
    }

    public GameObject Instantiate(GameObject prefab, Transform parent)
    {
        return GameObject.Instantiate(prefab, parent);
    }

    public T InstantiateUI<T>(GameObject prefab)
    {
        GameObject go = this.Instantiate(prefab, this.canvasGO.transform);

        return go.GetComponent<T>();
    }
}
