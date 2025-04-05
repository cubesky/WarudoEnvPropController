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
    public string transformToAnchorSyncNamePrefix = "T2ASync_";
    [SerializeField]
    public string animationSyncNamePrefix = "AniSync_";
    [SerializeField]
    public string anchorTotransformSyncNamePrefix = "A2TSync_";


    [Serializable]
    public class T2A
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
    public class A2T
    {
        public string name;
        [HideInInspector] public GameObject gameObject;
    }

    public enum NamePolicy
    {
        SELF, PARENT, OTHER_GAMEOBJECT, CUSTOM
    }

    public List<T2A> transformToAnchor = new List<T2A>();

    public List<Animations> animations = new List<Animations>();

    public List<A2T> anchorToTransform = new List<A2T>();


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
        transformToAnchor.Clear();
        animations.Clear();
        anchorToTransform.Clear();
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
                A2T transformSync = anchorToTransform.Find((tS) =>
                {
                    return anchor.Name == managerPrefix + anchorTotransformSyncNamePrefix + tS.name;
                });
                if (transformSync != null && transformSync != default(A2T) && anchor.Enabled)
                {
                    if (transformSync.gameObject != null)
                    {
                        transformSync.gameObject.transform.position = anchor.GameObject.transform.position + Vector3.zero;
                        transformSync.gameObject.transform.eulerAngles = anchor.GameObject.transform.eulerAngles + Vector3.zero;
                        transformSync.gameObject.transform.localScale = anchor.GameObject.transform.localScale + Vector3.zero;
                    }
                }
                T2A waypoint = transformToAnchor.Find((way) =>
                {
                    return anchor.Name == managerPrefix + transformToAnchorSyncNamePrefix + way.name;
                });
                if (waypoint != null && waypoint != default(T2A) && anchor.Enabled)
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
