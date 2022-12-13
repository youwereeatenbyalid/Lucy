using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Animations;
using Valve.VR;
using Valve.VR.InteractionSystem;


[RequireComponent(typeof(Interactable))]
public class MenuMoving : MonoBehaviour
{
	//Interaction
	public SteamVR_Action_Boolean menuToggleAction = SteamVR_Input.GetBooleanAction("ToggleMenu");
	private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & (~Hand.AttachmentFlags.SnapOnAttach) & (~Hand.AttachmentFlags.DetachOthers) & (~Hand.AttachmentFlags.VelocityMovement);
	private Player player;
	private Interactable interactable;
	//Gameplay

	public bool fallbackbuttonpush = false;
	private bool menuactive = false;
	Canvas canvas;

	public GameObject pausebutton;
	public GameObject resetbutton;

	//Positioning
	Vector3 prev_offset = new Vector3(0.0f,-1.0f, 1.0f).normalized* 0.5f;


	void updateButton(GameObject button, string newtext, bool interactable)
    {
		button.GetComponentInChildren<TMP_Text>().text = newtext;
		button.GetComponent<Button>().interactable = interactable;
		button.GetComponent<UIElement>().enabled = interactable;
    }

	public void GameStateUpdated(ScoreScript.GameState state)
    {
        switch (state)
        {
			case ScoreScript.GameState.Ready:
				updateButton(resetbutton, "Start Game", true);
				updateButton(pausebutton, "Pause Game", false);
				break;
			case ScoreScript.GameState.Starting:
				SetMenuVisible(false);
				updateButton(resetbutton, "Game Starting", false);
				updateButton(pausebutton, "Pause Game", false);
				break;
			case ScoreScript.GameState.Running:
				SetMenuVisible(false);
				updateButton(resetbutton, "Reset Game", true);
				updateButton(pausebutton, "Pause Game", true);
				break;
			case ScoreScript.GameState.Pause:
				updateButton(resetbutton, "Reset Game", true);
				updateButton(pausebutton, "Un-Pause Game", true);
				break;
			case ScoreScript.GameState.Win:
				updateButton(resetbutton, "Play Again", true);
				updateButton(pausebutton, "Pause Game", false);
				break;
			case ScoreScript.GameState.Lose:
				updateButton(resetbutton, "Play Again", true);
				updateButton(pausebutton, "Pause Game", false);
				break;
        }
    }

	//-------------------------------------------------
	void Awake()
	{
		player = this.transform.parent.GetComponent<Player>();
		interactable = GetComponent<Interactable>();
		canvas = GetComponentInChildren<Canvas>(true);
		transform.position = transform.position = player.hmdTransform.position + player.hmdTransform.forward * 0.5f;
		updateButton(resetbutton, "Start Game", true);
		updateButton(pausebutton, "Pause Game", false);
	}

    private void OnEnable()
    {
		ScoreScript.OnGameStateChange += GameStateUpdated;
    }

    private void OnDisable()
    {
		ScoreScript.OnGameStateChange += GameStateUpdated;
	}
    public void SetMenuVisible(bool state)
    {
		menuactive = state;
		canvas.gameObject.SetActive(menuactive);
		//if the menu is summoned, move it in front of the player and orient it in their direction.
		if (menuactive)
        {
			transform.position = player.headCollider.transform.position + player.headCollider.transform.forward * 0.5f;
			transform.LookAt(player.headCollider.transform, Vector3.up);
			//rotate to a more comfortable position
			transform.Rotate(20, 180, 0);
			prev_offset = (transform.position - player.hmdTransform.position);
		}
    }

    private bool lastHovering = false;
	private void Update()
	{

		if (interactable.isHovering != lastHovering) //save on the .tostrings a bit
		{
			//hoveringText.text = string.Format("Hovering: {0}", interactable.isHovering);
			lastHovering = interactable.isHovering;
		}

		if (!interactable.attachedToHand && menuactive)
		{
			if (prev_offset.magnitude > 0.7)
				prev_offset = prev_offset.normalized * 0.6f;
			
			transform.position = player.hmdTransform.position + prev_offset;
		}
		//if menu control binding is valid
		if (menuToggleAction != null && menuToggleAction.activeBinding)
        {
			//if menu button pressed, toggle menu
			bool dotoggle = menuToggleAction.GetStateDown(SteamVR_Input_Sources.LeftHand) || menuToggleAction.GetStateDown(SteamVR_Input_Sources.RightHand);
			if (dotoggle)
            {
				
				SetMenuVisible(!menuactive);
				ScoreScript.PauseGame(menuactive);
			}
        }
        else //allow for fallback option in editor
        {
            if (fallbackbuttonpush)
            {
				fallbackbuttonpush = false;
				SetMenuVisible(!menuactive);
				ScoreScript.PauseGame(menuactive);
			}

		}
		

	}

	//-------------------------------------------------
	// Called every Update() while a Hand is hovering over this object
	//-------------------------------------------------
	private void HandHoverUpdate(Hand hand)
	{
		GrabTypes startingGrabType = hand.GetGrabStarting();
		bool isGrabEnding = hand.IsGrabEnding(this.gameObject);

		if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
		{

			// Call this to continue receiving HandHoverUpdate messages,
			// and prevent the hand from hovering over anything else
			hand.HoverLock(interactable);

			// Attach this object to the hand
			hand.AttachObject(gameObject, startingGrabType, attachmentFlags);
			//constraint.constraintActive = false;

		}
		else if (isGrabEnding)
		{
			// Detach this object from the hand
			hand.DetachObject(gameObject);

			// Call this to undo HoverLock
			hand.HoverUnlock(interactable);
			prev_offset = (transform.position - player.hmdTransform.position);
		}
	}
}
