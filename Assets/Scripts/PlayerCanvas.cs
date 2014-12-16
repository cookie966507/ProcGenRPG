﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerCanvas : MonoBehaviour {

	public Sprite common, uncommon, rare, anomaly;

	public static List<GameObject> enemieswithhealthbars;

	private Animator playerAnim;

	private Player playerRef;

	private CanvasGroup inGameGUI;
	private Image bg, a1, a2, cursor;
	private Image byteXP, byteXPSmooth;
	private Text byteText, playerName;

	private CanvasGroup consoleGUI;

	private RectTransform weaponXPGroup;
	private Text curWeapon;
	private Text weaponXPPercentage;
	private Image weaponXPImg;
	private float weaponXPTimeOffset;
	private int tempWeaponXPVal;

	private Button strengthButton, defenseButton, efficiencyButton, securityButton, encryptionButton;
	private Text playerStrengthText, playerDefenseText, playerEfficiencyText, playerSecurityText, playerEncryptionText, algorithmPointsText, weaponStatsInfo;

	private Text integrityPercentage, RMAPercentage;
	private Image integrityBar, RMABar;

	public static bool inConsole = false;

	private GameObject minimap, mainCam, mainCamWithEffects;
	private Vector3 playerCanvasOffset;

	private RectTransform quickAccessBar, activeWeaponIcon, activeHackIcon;

	private GameObject VRCursor;

	private Image testEnemyHealthBarThing;

	public static void RegisterEnemyHealthBar(GameObject enemy) {
		if(!enemieswithhealthbars.Contains(enemy)) {
			enemieswithhealthbars.Add(enemy);
		}
	}

	// Use this for initialization
	void Start () {
		playerCanvasOffset = this.transform.position - Player.playerPos.position;

		minimap = GameObject.Find("MiniMapCam");
		mainCam = GameObject.Find("Main Camera");
		mainCamWithEffects = GameObject.Find("Main Camera With Effects");

		playerAnim = GameObject.Find("PlayerObj").GetComponent<Animator>();
		playerRef = GameObject.Find("PlayerObj").GetComponent<Player>();

		inGameGUI = transform.GetChild(0).GetComponent<CanvasGroup>();
		consoleGUI = transform.GetChild(1).GetComponent<CanvasGroup>();
	
		a1 = GameObject.Find("Attack1").GetComponent<Image>();
		a2 = GameObject.Find("Attack2").GetComponent<Image>();
		bg = GameObject.Find("AttackBG").GetComponent<Image>();
		cursor = GameObject.Find("AttackPointer").GetComponent<Image>();

		byteText = GameObject.Find("ByteText").GetComponent<Text>();
		playerName = GameObject.Find("PlayerName").GetComponent<Text>();
		playerName.text = playerRef.GetName();
		byteXP = GameObject.Find("ByteXP").GetComponent<Image>();
		byteXPSmooth = GameObject.Find("ByteXPSmooth").GetComponent<Image>();

		weaponXPGroup = GameObject.Find("WeaponXP").GetComponent<RectTransform>();
		curWeapon = GameObject.Find("WeaponName").GetComponent<Text>();
		weaponXPImg = GameObject.Find("WeaponByteXP").GetComponent<Image>();
		tempWeaponXPVal = playerRef.GetWeapon().GetBytes();
		curWeapon.text = playerRef.GetWeapon().GetName();
		weaponXPPercentage = GameObject.Find("WeaponXPPercentage").GetComponent<Text>();

		strengthButton = GameObject.Find("StrengthButton").GetComponent<Button>();
		defenseButton = GameObject.Find("DefenseButton").GetComponent<Button>();
		efficiencyButton = GameObject.Find("EfficiencyButton").GetComponent<Button>();
		securityButton = GameObject.Find("SecurityButton").GetComponent<Button>();
		encryptionButton = GameObject.Find("EncryptionButton").GetComponent<Button>();

		playerStrengthText = GameObject.Find("PlayerStrengthText").GetComponent<Text>();
		playerDefenseText= GameObject.Find("PlayerDefenseText").GetComponent<Text>();
		playerEfficiencyText = GameObject.Find("PlayerEfficiencyText").GetComponent<Text>();
		playerSecurityText = GameObject.Find("PlayerSecurityText").GetComponent<Text>();
		playerEncryptionText = GameObject.Find("PlayerEncryptionText").GetComponent<Text>();
		algorithmPointsText = GameObject.Find("AlgorithmPointsText").GetComponent<Text>();
		weaponStatsInfo = GameObject.Find("WeaponStatInfo").GetComponent<Text>();

		integrityBar = GameObject.Find("IntegrityBar").GetComponent<Image>();
		integrityPercentage = GameObject.Find("IntegrityPercentText").GetComponent<Text>();
		RMABar = GameObject.Find("RMABar").GetComponent<Image>();
		RMAPercentage = GameObject.Find("RMAPercentText").GetComponent<Text>();

		quickAccessBar = GameObject.Find("QuickAccessBar").GetComponent<RectTransform>();
		activeWeaponIcon = GameObject.Find("ActiveWeaponIcon").GetComponent<RectTransform>();
		activeHackIcon = GameObject.Find("ActiveHackIcon").GetComponent<RectTransform>();

		testEnemyHealthBarThing = GameObject.Find("TestEnemyHealthBarThing").GetComponent<Image>();
		enemieswithhealthbars = new List<GameObject>();

		VRCursor = GameObject.Find("VRCursor");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		AnimatorStateInfo info = playerAnim.GetCurrentAnimatorStateInfo(0);
		if (info.IsName("Base.Slash1")) {
			bg.enabled = true;
			a1.enabled = true;
			a2.enabled = false;
			cursor.enabled = true;
			cursor.rectTransform.localPosition = new Vector3((info.normalizedTime)*4 - 2, cursor.rectTransform.localPosition.y, cursor.rectTransform.localPosition.z);
		} else if (info.IsName("Base.Slash2")) {
			bg.enabled = true;
			a1.enabled = false;
			a2.enabled = true;
			cursor.enabled = true;
			cursor.rectTransform.localPosition = new Vector3((info.normalizedTime)*4 - 2, cursor.rectTransform.localPosition.y, cursor.rectTransform.localPosition.z);
		} else {
			bg.enabled = false;
			a1.enabled = false;
			a2.enabled = false;
			cursor.enabled = false;
		}
	}

	void Update () {
		for(int i = 0; i < playerRef.quickAccessItems.Count; i++) {
			if(playerRef.quickAccessItems[i] != null) {
				switch(playerRef.quickAccessItems[i].RarityVal) {
					case Rarity.Common:
						quickAccessBar.transform.GetChild(i).GetComponent<Image>().sprite = common;
						break;
					case Rarity.Uncommon:
						quickAccessBar.transform.GetChild(i).GetComponent<Image>().overrideSprite = uncommon;
						break;
					case Rarity.Rare:
						quickAccessBar.transform.GetChild(i).GetComponent<Image>().overrideSprite = rare;
						break;
					case Rarity.Anomaly:
						quickAccessBar.transform.GetChild(i).GetComponent<Image>().overrideSprite = anomaly;
						break;
				}
				if(playerRef.GetWeapon().Equals(playerRef.quickAccessItems[i])) {
					activeWeaponIcon.SetParent(quickAccessBar.GetChild(i), false);
//					activeWeaponIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2();
				}
				if(playerRef.GetHack().Equals(playerRef.quickAccessItems[i])) {
					activeHackIcon.SetParent(quickAccessBar.GetChild(i), false);
				}
				quickAccessBar.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = playerRef.quickAccessItems[i].icon;
			}
		}

		foreach (GameObject g in enemieswithhealthbars) {
			Vector3 tempPos = mainCam.camera.WorldToViewportPoint(g.transform.position);
			testEnemyHealthBarThing.rectTransform.anchoredPosition = new Vector2(11.612f*tempPos.x, -6.53f*(1-tempPos.y));
		}


		if(VRCursor != null) {
			VRCursor.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(Input.mousePosition.x/Screen.width*11.612f,Input.mousePosition.y/Screen.height*6.53f - 6.53f);
		}

		if(playerRef.GetWeapon() != null) {
			if(tempWeaponXPVal != playerRef.GetWeapon().GetBytes()) {
				curWeapon.text = playerRef.GetWeapon().GetName();
				GetComponent<Animator>().SetTrigger("ShowWeaponXP");
				tempWeaponXPVal = playerRef.GetWeapon().GetBytes();
			} else {
				GetComponent<Animator>().ResetTrigger("ShowWeaponXP");
			}
			weaponXPImg.rectTransform.localScale = new Vector3(playerRef.GetWeapon().GetVersionPercent(), 1, 1);
			weaponXPPercentage.text = (playerRef.GetWeapon().GetVersionPercent()*100).ToString("F2") + "%";
		}

		RMABar.rectTransform.localScale = new Vector3(playerRef.GetRMAPercentage(), 1f);
		RMAPercentage.text = (playerRef.GetRMAPercentage()*100).ToString("F2") + "%";
		integrityBar.rectTransform.localScale = new Vector3(playerRef.GetIntegrityPercentage(),1f);

		playerName.text = playerRef.GetName();

		byteText.text = "Bytes: " + Utility.ByteToString(playerRef.GetBytes());

		byteXP.rectTransform.localScale = new Vector3(playerRef.XPPercentage(), 1f, 1f);
		if(byteXPSmooth.rectTransform.localScale.x < playerRef.XPPercentage()) {
			byteXPSmooth.rectTransform.localScale = new Vector3(Mathf.MoveTowards(byteXPSmooth.rectTransform.localScale.x,playerRef.XPPercentage(),Time.deltaTime/10f), 1f, 1f);
		} else {
			byteXPSmooth.rectTransform.localScale = new Vector3(playerRef.XPPercentage(), 1f, 1f);
		}

		if(Input.GetKeyDown(KeyCode.BackQuote)) {
			inConsole = !inConsole;
		}

		GetComponent<Animator>().SetBool("ShowingConsole", inConsole);

		if(PlayerControl.PLAYINGWITHOCULUS) {
			Screen.showCursor = false;
			Screen.lockCursor = true;
		}

		if(inConsole) {
			minimap.SetActive(false);
			consoleGUI.interactable = true;
			if (Player.algorithmPoints > 0) {
				algorithmPointsText.text = "Algorithm Points Available: " + Player.algorithmPoints;
				defenseButton.interactable = true;
				strengthButton.interactable = true;
				efficiencyButton.interactable = true;
				encryptionButton.interactable = true;
				securityButton.interactable = true;
			} else {
				algorithmPointsText.text = Utility.ByteToString((int)(playerRef.GetXPBytes()/playerRef.XPPercentage()) - playerRef.GetXPBytes()) + " To Level Up"; 
				defenseButton.interactable = false;
				strengthButton.interactable = false;
				efficiencyButton.interactable = false;
				encryptionButton.interactable = false;
				securityButton.interactable = false;
			}
			playerDefenseText.text = "Defense: " + Player.defense;
			playerStrengthText.text = "Strength: " + Player.strength;
			playerEfficiencyText.text = "Efficiency: " + Player.efficiency;
			playerEncryptionText.text = "Encryption: " + Player.encryption;
			playerSecurityText.text = "Security: " + Player.security;
			weaponStatsInfo.text = playerRef.GetWeapon().InfoString();

			mainCam.camera.enabled = false;
			mainCamWithEffects.camera.enabled = true;
			mainCamWithEffects.GetComponent<Blur>().blur = Mathf.MoveTowards(mainCamWithEffects.GetComponent<Blur>().blur, 5, Time.deltaTime*5f);

		} else {
			consoleGUI.interactable = false;
			minimap.SetActive(true);

			mainCamWithEffects.GetComponent<Blur>().blur = Mathf.MoveTowards(mainCamWithEffects.GetComponent<Blur>().blur, 0, Time.deltaTime*5f);
			if(mainCamWithEffects.GetComponent<Blur>().blur == 0) {
				mainCam.camera.enabled = true;
				mainCamWithEffects.camera.enabled = false;
			}

		}
	}

	public void HandleDefenseClick() {
		Player.algorithmPoints--;
		Player.defense++;
	}

	public void HandleStrengthClick() {
		Player.algorithmPoints--;
		Player.strength++;
	}

	public void HandleEfficiencyClick() {
		Player.algorithmPoints--;
		Player.efficiency++;
	}

	public void HandleEncryptionClick() {
		Player.algorithmPoints--;
		Player.encryption++;
	}

	public void HandleSecurityClick() {
		Player.algorithmPoints--;
		Player.security++;
	}

}
 