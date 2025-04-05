using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    [HideInInspector] WInteractionManager worldAnimationManager;
    [HideInInspector] WInteractionManager.Animations animations = new WInteractionManager.Animations();
    [HideInInspector] Animator animator;

    [SerializeField] public WInteractionManager.NamePolicy namePolicy;
    [SerializeField] public string customName;
    [SerializeField] public GameObject customNameGameObject;
    [SerializeField] public string namePrefix;

    [SerializeField] public string managerName;
    [SerializeField] public bool stopAnimatorAtStartup = true;
    [SerializeField] public float animatorSpeed = 1.0f;

    [SerializeField] public WInteractionManager.Animations.AnimationParamsType animationParamsType;
    [SerializeField] public string paramName;
    [SerializeField] public float valueFloat;
    [SerializeField] public int valueInt;
    [SerializeField] public bool valueBool;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (stopAnimatorAtStartup)
        {
            animator.speed = 0f;
        }
        animations.animationController = this;
        switch (namePolicy)
        {
            case WInteractionManager.NamePolicy.SELF:
                animations.name = gameObject.name;
                break;
            case WInteractionManager.NamePolicy.PARENT:
                if (gameObject.transform.parent != null)
                {
                    animations.name = gameObject.transform.parent.gameObject.name;
                } else
                {
                    animations.name = Guid.NewGuid().ToString();
                }
                break;
            case WInteractionManager.NamePolicy.CUSTOM:
                if (customName != null && customName != "")
                {
                    animations.name = customName;
                }
                else
                {
                    animations.name = Guid.NewGuid().ToString();
                }
                break;
            case WInteractionManager.NamePolicy.OTHER_GAMEOBJECT:
                if (customNameGameObject != null)
                {
                    animations.name = customNameGameObject.name;
                } else
                {
                    if (customName != null && customName != "")
                    {
                        animations.name = customName;
                    } else
                    {
                        animations.name = Guid.NewGuid().ToString();
                    }
                }
                break;
            default:
                animations.name = Guid.NewGuid().ToString();
                break;
        }
        animations.name = namePrefix + animations.name;
        animations.animationParamsType = animationParamsType;
        animations.gameObject = gameObject;
        worldAnimationManager = GameObject.Find(managerName).GetComponent<WInteractionManager>();
        worldAnimationManager.animations.Add(animations);
    }

    
    void Update()
    {
        if (worldAnimationManager == null || managerName == null || paramName == null || managerName == "" || paramName == "")
        {
            return;
        }
        switch (animationParamsType)
        {
            case WInteractionManager.Animations.AnimationParamsType.FLOAT:
                {
                    if (animator.GetFloat(paramName) != valueFloat)
                    {
#if UNITY_EDITOR
                        animations.valueFloat = valueFloat;
#endif
                        animator.speed = animatorSpeed;
                        animator.SetFloat(paramName, valueFloat);
                    }
                    break;
                }
            case WInteractionManager.Animations.AnimationParamsType.INT:
                {
                    if (animator.GetInteger(paramName) != valueInt)
                    {
#if UNITY_EDITOR
                        animations.valueInt = valueInt;
#endif
                        animator.speed = animatorSpeed;
                        animator.SetInteger(paramName, valueInt);
                    }
                    break;
                }
            case WInteractionManager.Animations.AnimationParamsType.BOOL:
                {
                    if (animator.GetBool(paramName) != valueBool)
                    {
#if UNITY_EDITOR
                        animations.valueBool = valueBool;
#endif
                        animator.speed = animatorSpeed;
                        animator.SetBool(paramName, valueBool);
                    }
                    break;
                }
        }
    }
}
