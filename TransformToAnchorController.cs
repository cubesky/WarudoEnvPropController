using System;
using UnityEngine;

public class TransformToAnchorController : MonoBehaviour
{
    [HideInInspector] WInteractionManager.T2A waypoint = new WInteractionManager.T2A();
    [SerializeField] public WInteractionManager.NamePolicy namePolicy;
    [SerializeField] public string customName;
    [SerializeField] public GameObject customNameGameObject;
    [SerializeField] public string namePrefix;

    [SerializeField] public string managerName;

    void Start()
    {
        
        waypoint.gameObject = gameObject;
        switch (namePolicy)
        {
            case WInteractionManager.NamePolicy.SELF:
                waypoint.name = gameObject.name;
                break;
            case WInteractionManager.NamePolicy.PARENT:
                if (gameObject.transform.parent != null)
                {
                    waypoint.name = gameObject.transform.parent.gameObject.name;
                }
                else
                {
                    waypoint.name = Guid.NewGuid().ToString();
                }
                break;
            case WInteractionManager.NamePolicy.CUSTOM:
                if (customName != null && customName != "")
                {
                    waypoint.name = customName;
                }
                else
                {
                    waypoint.name = Guid.NewGuid().ToString();
                }
                break;
            case WInteractionManager.NamePolicy.OTHER_GAMEOBJECT:
                if (customNameGameObject != null)
                {
                    waypoint.name = customNameGameObject.name;
                }
                else
                {
                    if (customName != null && customName != "")
                    {
                        waypoint.name = customName;
                    }
                    else
                    {
                        waypoint.name = Guid.NewGuid().ToString();
                    }
                }
                break;
            default:
                waypoint.name = Guid.NewGuid().ToString();
                break;
        }
        waypoint.name = namePrefix + waypoint.name;
        GameObject.Find(managerName).GetComponent<WInteractionManager>().transformToAnchor.Add(waypoint);
    }

}
