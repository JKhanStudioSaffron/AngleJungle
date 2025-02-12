using UnityEngine;

public class Gem : MonoBehaviour
{
	// Ints
	private int layerMask;

	public int gemAngle = 45;

	// Audiosources
	public AudioSource as_PickGem;

	// Game objects
	public GameObject initialMirror;
	private Gem gemToBeSwapped;
	public GameObject OnSelectPar;
	private Mirror MirrorGO;

	// Vectors
	private Vector2 slotPosition;

	private Vector3 offset;

	private Vector3 originalPosition;

	private Vector3 onSlotScale, originalScale;

	// Bools
	private bool dragging = false;
	private bool collMirror = false;

	public bool onSlot = false;

	// Tranforrms
	private Transform toDrag;

	// Floats
	private float dist;

	//Camera
	Camera cam;

	//SpriteRenderer
	SpriteRenderer mySprite;

    private void OnEnable()
    {
		InputManager.Instance.Pressed += OnGemPressed;
		InputManager.Instance.Held += OnGemHeld;
		InputManager.Instance.Released += OnGemReleased;
    }

    private void OnDisable()
    {
        InputManager.Instance.Pressed -= OnGemPressed;
        InputManager.Instance.Held -= OnGemHeld;
        InputManager.Instance.Released -= OnGemReleased;
    }

    // Use this for initialization
    void Start ()
	{
		cam = Camera.main;
		mySprite = GetComponent<SpriteRenderer>();
        layerMask = ~ (1 << LayerMask.NameToLayer(Global.LAYER_POWER_GEM));
		originalScale = transform.localScale;
		onSlotScale = originalScale / 1.6f;
		originalPosition = transform.position;
        OnSelectPar.SetActive (false);
		PutGemInMirror ();
	}

    /// <summary>
    /// Handles the case when Gem is pressed by touch or mouse
    /// </summary>
    void OnGemPressed(Vector3 position)
	{
        if (Global.isPaused)
            return;

        RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(position), Vector2.zero, Mathf.Infinity, layerMask);

		// Handle collisions between raycast and gameobjects
		if (hit && (hit.collider.gameObject == gameObject))
		{
			if (onSlot && MirrorGO != null)
			{
				AnalyticsSingleton.Instance.gemHistory.AddAction(MirrorGO.name, Global.ANALYTICS_ACTION_REMOVED, gameObject.name, Time.time);
				onSlot = false;
				MirrorGO.slots--;
				MirrorGO.pickerNumber -= gemAngle;
				MirrorGO.ReleasePosition(gameObject);
				Global.antiCheater = 2;
			}

			toDrag = hit.transform;
			dist = hit.transform.position.z - cam.transform.position.z;
			Vector3 v3 = new(position.x, position.y, dist);
			v3 = cam.ScreenToWorldPoint(v3);
			offset = toDrag.position - v3;
			dragging = true;
			Global.isDragging = true;
			OnSelectPar.SetActive(true);

		}
	}

    /// <summary>
    /// Handles the case when Gem is held and moved by touch or mouse
    /// </summary>
    void OnGemHeld(Vector3 position)
	{
        if (Global.isPaused)
            return;

        // Handling phase of input touch is moved
        if (dragging)
        {
            Vector3 v3 = new(position.x, position.y, dist);
            v3 = cam.ScreenToWorldPoint(v3);
            toDrag.position = v3 + offset;
            mySprite.sortingOrder = 11;
        }
    }

    /// <summary>
    /// Handles the case when Gem is released by touch or mouse
    /// </summary>
    void OnGemReleased()
	{
		if (Global.isPaused)
			return;

		if (dragging)
		{
			SetNoInput();

			// Gem swap function
			if (gemToBeSwapped != null && MirrorGO != null)
			{
				AnalyticsSingleton.Instance.gemHistory.AddAction(MirrorGO.name, Global.ANALYTICS_ACTION_REMOVED, gemToBeSwapped.name, Time.time);
				gemToBeSwapped.ReleaseThisGem();
				gemToBeSwapped = null;
			}

			// Handling collisions with mirror
			if (collMirror && MirrorGO != null)
			{
				if (MirrorGO.slots + 1 > MirrorGO.maximumSlots)
				{
					onSlot = false;
				}
				else
				{
					AnalyticsSingleton.Instance?.gemHistory.AddAction(MirrorGO.name, Global.ANALYTICS_ACTION_PLACED, gameObject.name, Time.time);
					MirrorGO.slots++;
					onSlot = true;
					//fetch the position of gem
					slotPosition = MirrorGO.ArrangePosition(gameObject);
					MirrorGO.pickerNumber += gemAngle;
					//set anti-cheater conuter to 2
					Global.antiCheater = 2;

					if (gemToBeSwapped != null)
						gemToBeSwapped = null;
				}
			}
			ScaleOnPlacement();
		}

        if (!dragging && IsOffScreen())
        {
            transform.position = originalPosition;
        }
    }

    /// <summary>
    /// Scales the gem as per where it is placed
    /// </summary>
    void ScaleOnPlacement()
	{
        if (onSlot)
        {
            transform.position = new Vector3(slotPosition.x, slotPosition.y, transform.position.z);
            transform.localScale = onSlotScale;
        }
        else
        {
            transform.localScale = originalScale;
        }
    }


	/// <summary>
	/// Checks if gem is placed outside the screen view
	/// </summary>
    bool IsOffScreen()
    {
        Vector3 viewportPos = cam.WorldToViewportPoint(transform.position);
        return viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1;

    }

    /// <summary>
    /// Sets the world state to no input.
    /// </summary>
    private void SetNoInput()
	{
		dragging = false; 
		Global.isDragging = false;
		OnSelectPar.SetActive (false);
        mySprite.sortingOrder = 10;
    }

	/// <summary>
	/// Puts the gem in slot and ask mirror to auto allocate the gem
	/// </summary>
	public void PutGemInMirror()
	{
		if (initialMirror != null) 
		{
			Mirror initMirror = initialMirror.GetComponent<Mirror>();
			if (initMirror.slots + 1 > initMirror.maximumSlots) 
			{
				onSlot = false;
			} 
			else 
			{
				initMirror.slots++;
				onSlot = true;
				//fetch the position of gem
				slotPosition = initMirror.ArrangePosition (gameObject);
				initMirror.pickerNumber += gemAngle;
			}

            ScaleOnPlacement();
        }
    }

	/// <summary>
	/// gem got released from its current slot
	/// </summary>
	public void ReleaseThisGem()
	{
		if (onSlot && MirrorGO != null) 
		{
			onSlot = false;
			MirrorGO.slots--;
			MirrorGO.pickerNumber -= gemAngle;
			MirrorGO.ReleasePosition (gameObject);

			if (!dragging) 
			{
				transform.position += new Vector3 (0f, -1f, 0f);
			}
			ScaleOnPlacement();
		}
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		// behavior of entering mirror
		if (coll.gameObject.CompareTag(Global.TAG_MIRROR_COLLIDER)) 
		{
			collMirror = true;
			MirrorGO = coll.gameObject.GetComponent<Mirror>();
        }

		//the gem on slot checks if the interacting gem can be swapped
        if (coll.gameObject.CompareTag(Global.TAG_DRAGGABLE) && onSlot)
		{
			if (coll.gameObject.TryGetComponent<Gem>(out var interactedGem))
			{
				if (!interactedGem.onSlot)
				{
					if (interactedGem.gemToBeSwapped != null && interactedGem.gemToBeSwapped.Equals(this))
					{
						ScaleOnPlacement();
					}
					else
					{
						transform.localScale = originalScale * 1.2f;
						interactedGem.gemToBeSwapped = this;
					}
				}
			}
		}

    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag(Global.TAG_MIRROR_COLLIDER))
        {
            collMirror = false;
            MirrorGO = null;
        }

        //the gem on slot checks if the interacting gem has stopped interacting
        if (coll.gameObject.CompareTag(Global.TAG_DRAGGABLE) && onSlot)
        {
            if (coll.gameObject.TryGetComponent<Gem>(out var interactedGem))
            {
                if (!interactedGem.onSlot)
                {
                    ScaleOnPlacement();
					interactedGem.gemToBeSwapped = null;
                }
            }
        }
    }

}