using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using DG.Tweening;
using System;

[System.Serializable]
public class SlotUnit
{
	public bool isSlotEmpty;
	public Vector2 position;
	public GameObject targetGO;

	public SlotUnit(bool _isSlotEmpty, Vector2 _position)
	{
		isSlotEmpty = _isSlotEmpty;
		position = _position;
	}
}

public class Mirror : MonoBehaviour
{
	public GameObject sectorImage;
	public int pickerNumber = 0;
	public int maximumSlots = 1;
	public int slots = 0;
	public AudioSource as_putIn;
	public GameObject line;
	public TMP_Text Angle_Text;
	public bool isReceiver = false;

	private Sprite greyMirrorSp;
	private Sprite redMirrorSp;

	public GameObject protractor;
	public AudioSource as_toggle;
	public GameObject toggleCollider;

	public GameObject TutorialHand;
	bool isActivated = false;

	[HideInInspector]
	public List<SlotUnit> slotList;

	public List<SlotPositions> SlotPositions;

	public Vector2 BasePositionOffset;

	float showNumber = 0f;
	float degreeNumber = 0f;
	SpriteRenderer sr;

	public float TurningDuration;
	private bool isProtractorOn = false;
	private Vector3 proOriginalScale;

	HashSet<CastRayFromMirror> activeLights = new HashSet<CastRayFromMirror>();
	public Action SetAngle, MirrorStopped;

    Tweener tween;

    private void OnEnable()
	{
		InputManager.Instance.Pressed += ToggleD;
		SetAngle += StartAngleLerp;
    }

	private void OnDisable()
	{
		InputManager.Instance.Pressed -= ToggleD;
        SetAngle -= StartAngleLerp;
    }

    void ToggleD(Vector3 position)
	{
		if (Global.isPaused)
			return;

		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(position), Vector2.zero);

		// If touched open the protractor
		if (hit && (hit.collider.gameObject == toggleCollider))
		{
			ToggleProtractor();
		}
	}

	/// <summary>
	/// Initialize the slot system, and each slot's position on awake.
	/// </summary>
	void Awake()
	{
		sr = GetComponent<SpriteRenderer>();

		Vector2 slotBasePosition = (Vector2)transform.position + BasePositionOffset;
		slotList = new List<SlotUnit>(maximumSlots);

		SlotPositions CurrentSlot = SlotPositions.Find(slot => slot.Offsets.Length == maximumSlots);

		for(int i = 0; i < maximumSlots; i++)
		{
			slotList.Add(new SlotUnit(true, slotBasePosition + CurrentSlot.Offsets[i]));
		}
		greyMirrorSp = CurrentSlot.GreyObject;
		redMirrorSp = CurrentSlot.RedObject;


        if (!isReceiver)
        {
            ActiveLight();
        }
        else
        {
            DeactivateLight();
        }

		proOriginalScale = new Vector3(0.7f, 0.7f, 0.7f);
		protractor.transform.localScale = Vector3.zero;

		StartAngleLerp();

    }

    /// <summary>
    /// Core mechanism!!!
    /// This calculate the angle and parse it into laser angle in this 2D game world
    /// </summary>
    void StartAngleLerp()
    {
		// Kill any previous tweens on degreeNumber to prevent conflicts
		tween?.Kill();

        // DoTween tween for smooth angle transition
        tween = DOTween.To(() => degreeNumber, x => degreeNumber = x, pickerNumber, TurningDuration)
            .SetEase(Ease.OutQuad) // Adjust ease type as needed
            .OnUpdate(UpdateAngleVisuals)
            .SetTarget(this) // Useful for killing previous tweens when restarting
            .OnComplete(() => MirrorStopped?.Invoke());
    }

    void UpdateAngleVisuals()
    {
		if (Global.isPaused) return;

        Vector2 vect2_tmp = Quaternion.AngleAxis(showNumber / 2, Vector3.forward) * Vector2.right;
        sectorImage.GetComponent<TestMesh>().angleDegree = showNumber;
        Vector2 vect2_angle = Quaternion.AngleAxis(degreeNumber, Vector3.forward) * Vector2.right;

        line.transform.rotation = Quaternion.LookRotation(vect2_angle);

		SetAngleText(vect2_tmp);
    }

	void SetAngleText(Vector2 vect2_tmp)
	{
        if (Angle_Text)
        {
            showNumber = normalizeAngle(degreeNumber);
            Angle_Text.text = new CultureInfo(LocalizationSettings.SelectedLocale.Identifier.Code).TextInfo.IsRightToLeft
                ? "°" + LocalizedNumberManager.GetLocalizedNumber(Mathf.RoundToInt(showNumber))
                : LocalizedNumberManager.GetLocalizedNumber(Mathf.RoundToInt(showNumber)) + "°";

            Angle_Text.gameObject.transform.localPosition = vect2_tmp * 100f;
        }
    }
    
	/// <summary>
	/// Normalizes the angle within 360. It also works with negative angles
	/// </summary>
	/// <returns>The angle.</returns>
	/// <param name="angle">Angle.</param>
	private float normalizeAngle(float angle)
	{
		return (angle %= 360) >= 0 ? angle : (angle + 360);
	}

	/// <summary>
	/// Toggles the protractor for real.
	/// </summary>
	public void ToggleProtractor()
	{
		GameObject handClickTutorial = GameObject.FindGameObjectWithTag(Global.TAG_HAND_TUTORIAL);

		if (handClickTutorial != null)
			handClickTutorial.SetActive(false);

		isProtractorOn = !isProtractorOn;
		as_toggle.Play();

		if (isProtractorOn)
		{
			protractor.transform.DOScale(proOriginalScale, 0.5f);
			AnalyticsSingleton.Instance.gemHistory.AddAction(GetComponent<Mirror>().name, Global.ANALYTICS_PROTRACTOR_CLOSED, "-1", Time.time);
		}
		else
		{
			protractor.transform.DOScale(Vector3.zero, 0.5f);
			AnalyticsSingleton.Instance.gemHistory.AddAction(GetComponent<Mirror>().name, Global.ANALYTICS_PROTRACTOR_OPENED, "-1", Time.time);
		}
	}

	/// <summary>
	/// Auto allocate gems
	/// </summary>
	/// <returns>The position of the provided slot</returns>
	/// <param name="go">gem gameobject</param>
	public Vector2 ArrangePosition(GameObject go)
	{
		if (TutorialHand != null)
			TutorialHand.SetActive(false);

		foreach (SlotUnit su in slotList)
		{
			if (su.isSlotEmpty)
			{
				as_putIn.Play();
				su.targetGO = go;
				su.isSlotEmpty = false;
				return su.position;
			}
		}

		return Vector2.zero;
	}

	/// <summary>
	/// Releases gem from its slot.
	/// </summary>
	/// <param name="go">gem got released</param>
	public void ReleasePosition(GameObject go)
	{
		foreach (SlotUnit su in slotList)
		{
			if (su.targetGO == go)
			{
				su.targetGO = null;
				su.isSlotEmpty = true;
			}
		}
	}

    /// <summary>
    /// Actives this receiver mirror if one light intersects with it
    /// </summary>

    public void ActivateIfLightIntersects(CastRayFromMirror light)
	{
		if (!isReceiver)
			return;

		if (isActivated)
			return;

		activeLights.Add(light);
		if (activeLights.Count > 0)
		{
			ActiveLight();
		}
    }

    /// <summary>
    /// Actives this receiver mirror to emit light
    /// </summary>

    public void ActiveLight()
	{				
		isActivated = true;
		line.SetActive(true);
		sectorImage.SetActive(true);
		Angle_Text.gameObject.SetActive(true);
		sr.sprite = redMirrorSp;
    }

    /// <summary>
    /// Deactivates this receiver mirror if no light intersects with it
    /// </summary>

    public void DeactivateIfNoLightIntersects(CastRayFromMirror light)
    {
		if (!isReceiver)
			return;

		activeLights.Remove(light);

        if (activeLights.Count == 0)
		{
			DeactivateLight();
		}
	}

    /// <summary>
    /// Deactivates this receiver mirror.
    /// </summary>
    public void DeactivateLight()
	{
		SetAngle?.Invoke();
		isActivated = false;
		PowerGem.CheckLightActivated?.Invoke(line);
		line.SetActive(false);
		sectorImage.SetActive(false);
		Angle_Text.gameObject.SetActive(false);
		sr.sprite = greyMirrorSp;
    }
}