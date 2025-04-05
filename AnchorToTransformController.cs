using System;
using System.Collections;
using System.Collections.Generic;
using UMod;
using UnityEngine;

public class AnchorToTransformController : MonoBehaviour
{
    [HideInInspector] WInteractionManager.TransformSync sync = new WInteractionManager.TransformSync();
    [SerializeField] public WInteractionManager.NamePolicy namePolicy;
    [SerializeField] public string customName;
    [SerializeField] public GameObject customNameGameObject;
    [SerializeField] public string namePrefix;

    [SerializeField] public string managerName;

    void Start()
    {
        sync.gameObject = gameObject;
        switch (namePolicy)
        {
            case WInteractionManager.NamePolicy.SELF:
                sync.name = gameObject.name;
                break;
            case WInteractionManager.NamePolicy.PARENT:
                if (gameObject.transform.parent != null)
                {
                    sync.name = gameObject.transform.parent.gameObject.name;
                }
                else
                {
                    sync.name = Guid.NewGuid().ToString();
                }
                break;
            case WInteractionManager.NamePolicy.CUSTOM:
                if (customName != null && customName != "")
                {
                    sync.name = customName;
                }
                else
                {
                    sync.name = Guid.NewGuid().ToString();
                }
                break;
            case WInteractionManager.NamePolicy.OTHER_GAMEOBJECT:
                if (customNameGameObject != null)
                {
                    sync.name = customNameGameObject.name;
                }
                else
                {
                    if (customName != null && customName != "")
                    {
                        sync.name = customName;
                    }
                    else
                    {
                        sync.name = Guid.NewGuid().ToString();
                    }
                }
                break;
            default:
                sync.name = Guid.NewGuid().ToString();
                break;
        }
        sync.name = namePrefix + sync.name;
        GameObject.Find(managerName).GetComponent<WInteractionManager>().transforms.Add(sync);
    }
}
