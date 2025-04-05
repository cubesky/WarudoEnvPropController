using System;
using System.Collections.Generic;
using UnityEngine;
using Warudo.Core;
using Warudo.Plugins.Core.Assets.Utility;

public class WInteractionManager : MonoBehaviour
{
    [SerializeField]
    public string managerPrefix = "";
    [SerializeField]
    public string waypointSyncNamePrefix = "WaypointSync_";
    [SerializeField]
    public string animationSyncNamePrefix = "AnimationSync_";
    [SerializeField]
    public string transformSyncNamePrefix = "TransformSync_";


    [Serializable]
    public class Waypoint
    {
        public string name;
        [HideInInspector] public GameObject gameObject;
        public Transform transform
        {
            get { return gameObject.transform; }
        }
    }

    [Serializable]
    public class Animations
    {
        public enum AnimationParamsType
        {
            UNSET, FLOAT, INT, BOOL
        }

        [SerializeField] public string name;
        [HideInInspector] public GameObject gameObject;
        [HideInInspector] public AnimationController animationController;
        [HideInInspector] public AnimationParamsType animationParamsType = AnimationParamsType.UNSET;
        [SerializeField] public float valueFloat;
        [SerializeField] public int valueInt;
        [SerializeField] public bool valueBool;
    }

    [Serializable]
    public class TransformSync
    {
        public string name;
        [HideInInspector] public GameObject gameObject;
    }

    public enum NamePolicy
    {
        SELF, PARENT, OTHER_GAMEOBJECT, CUSTOM
    }

    public List<Waypoint> waypoints = new List<Waypoint>();

    public List<Animations> animations = new List<Animations>();

    public List<TransformSync> transforms = new List<TransformSync>();


#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach (var item in animations)
        {
            switch (item.animationParamsType)
            {
                case Animations.AnimationParamsType.FLOAT:
                    {
                        item.animationController.valueFloat = item.valueFloat;
                        break;
                    }
                case Animations.AnimationParamsType.INT:
                    {
                        item.animationController.valueInt = item.valueInt;
                        break;
                    }
                case Animations.AnimationParamsType.BOOL:
                    {
                        item.animationController.valueBool = item.valueBool;
                        break;
                    }
            }
        }
    }
#endif

    private void Awake()
    {
        waypoints.Clear();
        animations.Clear();
        transforms.Clear();
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            return;
        }

            IReadOnlyList<AnchorAsset> anchors = Context.OpenedScene.GetAssets<AnchorAsset>();
            foreach (AnchorAsset anchor in anchors)
            {
                TransformSync transformSync = transforms.Find((tS) =>
                {
                    return anchor.Name == managerPrefix + transformSyncNamePrefix + tS.name;
                });
                if (transformSync != null && transformSync != default(TransformSync) && anchor.Enabled)
                {
                    if (transformSync.gameObject != null)
                    {
                        transformSync.gameObject.transform.position = anchor.GameObject.transform.position + Vector3.zero;
                        transformSync.gameObject.transform.eulerAngles = anchor.GameObject.transform.eulerAngles + Vector3.zero;
                        transformSync.gameObject.transform.localScale = anchor.GameObject.transform.localScale + Vector3.zero;
                    }
                }
                Waypoint waypoint = waypoints.Find((way) =>
                {
                    return anchor.Name == managerPrefix + waypointSyncNamePrefix + way.name;
                });
                if (waypoint != null && waypoint != default(Waypoint) && anchor.Enabled)
                {
                    anchor.Attachable.Parent = null;
                    anchor.Transform.CopyFromWorldTransform(waypoint.gameObject.transform);
                }
                Animations anim = animations.Find((a) =>
                {
                    return anchor.Name == managerPrefix + animationSyncNamePrefix + a.name;
                });
                if (anim != null && anim != default(Animations))
                {
                    switch (anim.animationParamsType)
                    {
                        case Animations.AnimationParamsType.FLOAT:
                            {
#if UNITY_EDITOR
                                anim.valueFloat = anchor.Transform.Position.x;
#endif
                                anim.animationController.valueFloat = anchor.Transform.Position.x;
                                break;
                            }
                        case Animations.AnimationParamsType.INT:
                            {
#if UNITY_EDITOR
                                anim.valueInt = (int)anchor.Transform.Position.x;
#endif
                                anim.animationController.valueInt = (int)anchor.Transform.Position.x;
                                break;
                            }
                        case Animations.AnimationParamsType.BOOL:
                            {
#if UNITY_EDITOR
                                anim.valueBool = anchor.Enabled;
#endif
                                anim.animationController.valueBool = anchor.Enabled;
                                break;
                            }
                    }
                }
             
            
        }
    }
}
