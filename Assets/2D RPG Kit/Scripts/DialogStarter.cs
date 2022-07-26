using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

//Adds a BoxCollider2D component automatically to the game object
[RequireComponent(typeof(BoxCollider2D))]
public class DialogStarter : MonoBehaviour {

    [HideInInspector]
    public int numberOfItemsHeld;
    [HideInInspector]
    public int numberOfEquipItemsHeld;

    [Header("Dialog Lines")]
    //The lines the npcs say when the player talks to them
    [Tooltip("The lines the npcs say when the player talks to them. One line can fill an entire message box with appropriate line breaks")]
    public string[] lines;
    [Tooltip ("Inn and shop keepers will say these lines after closing their menus") ]
    public string[] sayGoodBye;

    //Check wheather the player is in range to talk to npc
    private bool canActivate;

    [Header("Activation")]
    //For different activation methods
    [Tooltip("Activates the dialog as soon as this script is activated. Keep in mind that the player character still has to be in the trigger zone")]
    public bool activateOnAwake;
    [Tooltip("Activates the dialog when the player presses the confirm button")]
    public bool activateOnButtonPress;
    [Tooltip("Activates the dialog when the player enters the trigger zone")]
    public bool activateOnEnter;
    [Tooltip("Activate a delay before showing the dialog")]
    public bool waitBeforeActivatingDialog;
    [Tooltip("Enter the duration of the delay in seconds")]
    public float waitTime;

    [Header("NPC Settings")]
    //Check if the player talks to a person for displaying a name tag
    [Tooltip("Display a name tag")]
    public bool displayName = true;

    //If npc should join your party
    [Tooltip("Let the NPC join the players party at the 2nd slot")]
    public bool addToPartyCharacter2 = false;
    [Tooltip("Let the NPC join the players party at the 3rd slot")]
    public bool addToPartyCharacter3 = false;

    [Header("Inn Settings")]
    //If npc should be an inn keeper
    [Tooltip("Activates the inn menu")]
    public bool isInn;
    [Tooltip("Set the price for one night")]
    public int innPrice;

    [Header("Shop Settings")]
    //If npc should be a shop keeper
    [Tooltip("Activates the shop menu")]
    public bool isShop;
    [Tooltip("Enter all items that should be on sale in this shop")]
    public string[] ItemsForSale = new string[40];

    [Header("Receive Settings")]
    [Tooltip("Receive an item after conversation")]
    public bool receiveItem;
    [Tooltip("Drag and drop an item prefab")]
    public Item item;
    [Tooltip("Receive gold after conversation")]
    public bool receiveGold;
    [Tooltip("The amount of gold received")]
    public int goldAmount;

    [Header("Quest Settings")]
    //For completing quests after dialog
    [Tooltip("Enter the quest that should be completed. This quest has to be registered in the Quest Manager")]
    public string questToMark;
    [Tooltip("Mark a quest as complete after the dialog")]
    public bool markComplete;

    [Header("Event Settings")]
    //For completing quests after dialog
    //public bool shouldActivateQuest;
    [Tooltip("Enter the event that should be completed. This quest has to be registered in the Quest Manager")]
    public string eventToMark;
    [Tooltip("Mark an event as complete after the dialog")]
    public bool markEventComplete;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //Check if dialog should be activated on awake or enter
        if (activateOnAwake || activateOnEnter)
        {
            //Check if player is in reach and if no other dialog is currently active
            if (canActivate && !DialogManager.instance.dialogBox.activeInHierarchy && !Inn.instance.innMenu.activeInHierarchy && !GameMenu.instance.menu.activeInHierarchy)
            {
                PlayerController.instance.canMove = false;
                //Set this to false to prevent activating dialog endlessly
                activateOnEnter = false;

                if (!DialogManager.instance.dontOpenDialogAgain)
                {
                    if (waitBeforeActivatingDialog)
                    {
                        //Disable player movement
                        PlayerController.instance.canMove = false;
                        StartCoroutine(waitCo());
                    }
                    else
                    {
                        activateOnAwake = false;

                        //Hide mobile controller during dialogs
                        GameMenu.instance.touchController.SetActive(false);

                        //Add item after conversation
                        if (receiveItem)
                        {
                            //Take the reference for isItem/isWeapon/isArmour from shop instance
                            Shop.instance.selectedItem = item;

                            //Calculate the amount of items / equipment held in inventory to prevent adding more items if inventory is full
                            numberOfItemsHeld = 0;
                            numberOfEquipItemsHeld = 0;

                            for (int i = 0; i < GameManager.instance.itemsHeld.Length; i++)
                            {
                                if (GameManager.instance.itemsHeld[i] != "")
                                {
                                    numberOfItemsHeld++;
                                }
                            }

                            for (int i = 0; i < GameManager.instance.equipItemsHeld.Length; i++)
                            {
                                if (GameManager.instance.equipItemsHeld[i] != "")
                                {
                                    numberOfEquipItemsHeld++;
                                }
                            }

                            if (item)
                            {
                                if (Shop.instance.selectedItem.item)
                                {
                                    if (numberOfItemsHeld < GameManager.instance.itemsHeld.Length)
                                    {
                                        GameMenu.instance.gotItemMessageText.text = "You found a " + item.itemName + "!";
                                        GameManager.instance.AddItem(item.itemName);
                                        receiveItem = false;
                                    }
                                    else
                                    {
                                        Shop.instance.promptText.text = "You found a " + Shop.instance.selectedItem.name + "." + "\n" + "But your item bag is full!";
                                        StartCoroutine(Shop.instance.PromptCo());
                                    }

                                }

                                if (Shop.instance.selectedItem.defense || Shop.instance.selectedItem.offense)
                                {
                                    if (numberOfEquipItemsHeld < GameManager.instance.equipItemsHeld.Length)
                                    {
                                        GameMenu.instance.gotItemMessageText.text = "You found a " + item.itemName + "!";
                                        //StartCoroutine(gotItemMessageCo());
                                        GameManager.instance.AddItem(item.itemName);
                                    }
                                    else
                                    {
                                        Shop.instance.promptText.text = "You found a " + Shop.instance.selectedItem.name + "." + "\n" + "But your equipment bag is full!";
                                        StartCoroutine(Shop.instance.PromptCo());
                                    }
                                }
                            }
                        }

                        //Add gold after conversation
                        if (receiveGold)
                        {
                            GameMenu.instance.gotItemMessageText.text = "You found " + receiveGold + " Gold!";
                            //StartCoroutine(gotItemMessageCo());
                            GameManager.instance.currentGold += goldAmount;
                            receiveGold = false;
                        }

                        //Add new member to party
                        if (addToPartyCharacter2)
                        {
                            DialogManager.instance.addToPartyCharacter2 = addToPartyCharacter2;
                            addToPartyCharacter2 = false; //prevents from adding multiple times to party by talking to npc again
                        }

                        //Add new member to party
                        if (addToPartyCharacter3)
                        {
                            DialogManager.instance.addToPartyCharacter3 = addToPartyCharacter3;
                            addToPartyCharacter3 = false; //prevents from adding multiple times to party by talking to npc again
                        }

                        //Show inn menu
                        if (isInn)
                        {
                            DialogManager.instance.isInn = isInn;
                            DialogManager.instance.innPrice = innPrice;
                            Inn.instance.sayGoodBye = sayGoodBye;
                        }

                        //Show shop menu
                        if (isShop)
                        {
                            DialogManager.instance.isShop = isShop;
                            Shop.instance.itemsForSale = ItemsForSale;
                            Shop.instance.sayGoodBye = sayGoodBye;
                        }
                        
                        DialogManager.instance.ShowDialogAuto(lines, displayName);
                        
                        DialogManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
                        if (markEventComplete)
                        {
                            DialogManager.instance.ActivateEventAtEnd(eventToMark, markEventComplete);
                        }
                        
                    }
                }
            }
        }

        //Check for button input
        if (Input.GetButtonDown("RPGConfirmPC") || Input.GetButtonDown("RPGConfirmJoy") || CrossPlatformInputManager.GetButtonDown("RPGConfirmTouch") && !DialogManager.instance.dialogBox.activeInHierarchy)
        {
            
            if (canActivate && !DialogManager.instance.dialogBox.activeInHierarchy && !Inn.instance.innMenu.activeInHierarchy && !GameMenu.instance.menu.activeInHierarchy && !GameManager.instance.battleActive)
            {
                PlayerController.instance.canMove = false;
                //activateOnEnterConfirm = false;
                if (!DialogManager.instance.dontOpenDialogAgain)
                {
                    if (waitBeforeActivatingDialog)
                    {
                        //Disable player movement
                        PlayerController.instance.canMove = false;
                        StartCoroutine(waitCo());
                    }else
                    {
                        activateOnAwake = false;
                        GameMenu.instance.touchController.SetActive(false);

                        //Add item after conversation
                        if (receiveItem)
                        {
                            //Take the reference for isItem/isWeapon/isArmour from shop instance
                            Shop.instance.selectedItem = item;

                            //Calculate the amount of items / equipment held in inventory to prevent adding more items if inventory is full
                            numberOfItemsHeld = 0;
                            numberOfEquipItemsHeld = 0;

                            for (int i = 0; i < GameManager.instance.itemsHeld.Length; i++)
                            {
                                if (GameManager.instance.itemsHeld[i] != "")
                                {
                                    numberOfItemsHeld++;
                                }
                            }

                            for (int i = 0; i < GameManager.instance.equipItemsHeld.Length; i++)
                            {
                                if (GameManager.instance.equipItemsHeld[i] != "")
                                {
                                    numberOfEquipItemsHeld++;
                                }
                            }

                            if (item)
                            {
                                if (Shop.instance.selectedItem.item)
                                {
                                    if (numberOfItemsHeld < GameManager.instance.itemsHeld.Length)
                                    {
                                        GameMenu.instance.gotItemMessageText.text = "You found a " + item.itemName + "!";
                                        GameManager.instance.AddItem(item.itemName);
                                        receiveItem = false;
                                    }
                                    else
                                    {
                                        Shop.instance.promptText.text = "You found a " + Shop.instance.selectedItem.name + "." + "\n" + "But your item bag is full!";
                                        StartCoroutine(Shop.instance.PromptCo());
                                    }

                                }

                                if (Shop.instance.selectedItem.defense || Shop.instance.selectedItem.offense)
                                {
                                    if (numberOfEquipItemsHeld < GameManager.instance.equipItemsHeld.Length)
                                    {
                                        GameMenu.instance.gotItemMessageText.text = "You found a " + item.itemName + "!";
                                        //StartCoroutine(gotItemMessageCo());
                                        GameManager.instance.AddItem(item.itemName);
                                    }
                                    else
                                    {
                                        Shop.instance.promptText.text = "You found a " + Shop.instance.selectedItem.name + "." + "\n" + "But your equipment bag is full!";
                                        StartCoroutine(Shop.instance.PromptCo());
                                    }
                                }
                            }                            
                        }

                        //Add gold after conversation
                        if (receiveGold)
                        {
                            GameMenu.instance.gotItemMessageText.text = "You found " + receiveGold + " Gold!";
                            //StartCoroutine(gotItemMessageCo());
                            GameManager.instance.currentGold += goldAmount;
                            receiveGold = false;
                        }

                        //Add new member to party
                        if (addToPartyCharacter2)
                        {
                            DialogManager.instance.addToPartyCharacter2 = addToPartyCharacter2;
                            addToPartyCharacter2 = false; //prevents from adding multiple times to party by talking to npc again
                        }

                        //Add new member to party
                        if (addToPartyCharacter3)
                        {
                            DialogManager.instance.addToPartyCharacter3 = addToPartyCharacter3;
                            addToPartyCharacter3 = false; //prevents from adding multiple times to party by talking to npc again
                        }

                        if (isInn)
                        {
                            DialogManager.instance.isInn = isInn;
                            DialogManager.instance.innPrice = innPrice;
                            Inn.instance.sayGoodBye = sayGoodBye;
                        }

                        if(isShop)
                        {
                            DialogManager.instance.isShop = isShop;
                            Shop.instance.itemsForSale = ItemsForSale;
                            Shop.instance.sayGoodBye = sayGoodBye;
                        }

                        
                            DialogManager.instance.ShowDialog(lines, displayName);
                        
                        //DialogManager.instance.SayGoodBye(sayGoodBye, isPerson);
                        DialogManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
                        if (markEventComplete)
                        {
                            DialogManager.instance.ActivateEventAtEnd(eventToMark, markEventComplete);
                        }
                        
                    }                    
                }
            }
        }
	}

    //Check if player enters trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canActivate = true;
            //DialogManager.instance.dontOpenDialogAgain = false;
            
        }
    }

    //Check if player exits trigger zone
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canActivate = false;

            if (!activateOnButtonPress)
            {
                activateOnEnter = true;
            }
        }
    }

    //Put in a slight delay between activating the dialog and showing the dialog
    IEnumerator waitCo()
    {

        yield return new WaitForSeconds(waitTime);
        waitBeforeActivatingDialog = false;
        
    }
}
