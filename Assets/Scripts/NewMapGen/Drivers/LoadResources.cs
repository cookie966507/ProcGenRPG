﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadResources : MonoBehaviour {

	public static string ConversationFile = "./Assets/Resources/out.conv";

    public Sprite twoWay;
    public Sprite threeWay;
    public Sprite fourWay;
    public Sprite end;
    public Sprite corner;

    public GameObject spriteHolder;

    public GameObject grassyPath;
    public GameObject dungeon;

	public GameObject CommonItemDrop;
	public GameObject UncommonItemDrop;
	public GameObject RareItemDrop;
	public GameObject Chest;

    public Tile portal;

    public GameObject city;

	[HideInInspector]
	public List<uConversation> Conversations;

    public static LoadResources Instance;

    void Awake()
    {
        // First, check if there are any other instances conflicting.
        if (Instance != null && Instance != this)
        {
            // If so, destroy other instances.
            Destroy(this.gameObject);
        }

        //Save our singleton instance.
        Instance = this;

        DontDestroyOnLoad(this.gameObject);

		//Runtime load operations
		//Note: Quests are loaded in the questListener constructor
		//if boss level, these should already be loaded
		if (!MasterDriver.bossLevel) {
			loadConversations ();
		}
    }

	private void loadConversations() {
		//first read the package from the file, then unwrap it
		System.IO.FileStream fs = new System.IO.FileStream (ConversationFile, System.IO.FileMode.Open);
		ConversationPackage package = ConversationPackage.ParseFrom (fs);
		
		List<Conversation> conversationProtocols = new List<Conversation>();
		conversationProtocols.AddRange(package.ConversationsList);
		Conversations = new List<uConversation> ();

		foreach (Conversation c in conversationProtocols) {
			foreach (ConversationNode n in c.AllNodesList) {
				new uConversationNode(n.Uid);
			}
		}

		foreach (Conversation c in conversationProtocols) {
			Conversations.Add(new uConversation(c));
		}
	}

	public List<ConversationSave> getConversationSaveData() {
		List<ConversationSave> saveData = new List<ConversationSave> ();

		foreach (uConversation c in Conversations) {
			saveData.Add(c.getSaveData());
		}

		return saveData;
	}

	public void setConversationData(List<ConversationSave> saveData) {

		//O(n^2) because its quick to code and we only have 5ish conversations anyway
		foreach (ConversationSave save in saveData) {
			foreach (uConversation c in Conversations) {
				if (c.getName().Equals(save.Name)) {
					c.setFromSave(save);
				}
			}
		}
	}
	
}
