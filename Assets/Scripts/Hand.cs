using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public enum HandType
{
  Left,
  Right
}


public class Hand : MonoBehaviour
{

  public HandType type = HandType.Left;

  public bool isHidden { get; private set; } = false;

  public InputAction trackedAction = null;

  public InputAction gripAction  = null;

  public InputAction triggerAction = null;

  public Animator handAnimator = null;

  int m_gripAmountParameter = 0;
  int m_pointAmountParameter = 0;

  bool m_isCurrentlyTracked = false;

  List<Renderer> m_currentRenderers = new List<Renderer>();

  Collider[] m_colliders = null;

  public bool isCollisionEnabled { get; private set; } = false;

  public XRBaseInteractor interactor = null;


  private void Awake()
  {
      if (interactor == null)
      {
          interactor = GetComponentInParent<XRBaseInteractor>();
      }
  }

/*  private void OnEnable()
  {
    interactor.selectEntered.AddListener(OnGrab);
    interactor.selectEntered.AddListener(OnRelease);
  }

  private void OnDisable()
  {
    interactor.selectEntered.AddListener(OnGrab);
    interactor.selectEntered.AddListener(OnRelease);
  }*/

  // Start is called before the first frame update
  void Start()
    {
        m_colliders = GetComponentsInChildren<Collider>().Where(childCollider => !childCollider.isTrigger).ToArray();
        trackedAction.Enable();

        m_gripAmountParameter = Animator.StringToHash("GripAmount");
        m_pointAmountParameter = Animator.StringToHash("PointAmount");

        gripAction.Enable();
        triggerAction.Enable();
        Hide();
    }

  void UpdateHandAnimation()
  {
    float pointAmount = triggerAction.ReadValue<float>();
    handAnimator.SetFloat(m_pointAmountParameter, pointAmount);

    float gripAmount = gripAction.ReadValue<float>();
    handAnimator.SetFloat(m_gripAmountParameter, Mathf.Clamp01(gripAmount + pointAmount));
  }

  // Update is called once per frame
  void Update()
  {
    float isTracked = trackedAction.ReadValue<float>();
    if (isTracked == 1.0f && !m_isCurrentlyTracked)
    {
      m_isCurrentlyTracked = true;
      Show();
    }
    else if (isTracked == 0 && m_isCurrentlyTracked)
    {
      m_isCurrentlyTracked = false;
      Hide();
    }
    UpdateHandAnimation();
  }

  void Show() {
    foreach (Renderer renderer in m_currentRenderers)
    {
      renderer.enabled = true;
    }
    isHidden = false;
    EnableCollisions(true);
  }

  void Hide() {
    m_currentRenderers.Clear();
    Renderer[] renderers = GetComponentsInChildren<Renderer>();
    foreach(Renderer renderer in renderers)
    {
      if (renderer != null)
      {
        renderer.enabled = false;
      }
      m_currentRenderers.Add(renderer);
      isHidden = true;
      EnableCollisions(false);
    }
  }

  void EnableCollisions(bool enabled)
  {
    if (isCollisionEnabled == enabled) return;

    isCollisionEnabled = enabled;
    foreach (Collider collider in m_colliders)
    {
      collider.enabled = isCollisionEnabled;
    }
  }


  /*void OnGrab(XRBaseInteractable grabbedObject)
  {
    HandControl ctrl = grabbedObject.GetComponent<HandControl>();
    if (ctrl != null)
    {
      if (ctrl.hideHand)
      {
        Hide();
      }
    }
  }

  void OnRelease(XRBaseInteractable releaseObject)
  {
    HandControl ctrl = releaseObject.GetComponent<HandControl>();
      if (ctrl != null)
      {
          if (ctrl.hideHand)
          {
              Show();
          }
      }
  }*/
}
