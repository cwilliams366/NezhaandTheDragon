using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public enum GameState {Nothing, MainMenu, Paused, Playing, Dead, Dialogue, GameOver}
public class GameManagerScript : MonoBehaviour

{
    //Respawn Point
    Vector3 startPos;

    //Set GameState a Default State
    public GameState currentState = GameState.Nothing;

    //Singletons
    public static GameManagerScript S { get; private set; }
    public GameStateScript GameStartState { get; private set; }

    [Header("Game Essentials")]
    public PlayerManagerScript PlayerManager;
    public LevelManagerScript LevelManager;
    public GameStateScript InitialGameState;
    
    
    //Numerical Values
    [Header("Numerical Values")]
    public int currentLives = 3;
    public int pearlCount = 3;
    public int currentCoinAmount;
    public int currentTalismanAmount;
    public int currentSpiritManaAmount;

    //Game Text
    [Header("UI In-Game Text")]
    public TMP_Text displayCoins;
    public TMP_Text gameMessage;

    //Prefabs
    [Header("Game Prefabs")]
    //public GameObject currentPlayer;
   // public GameObject playerPrefab;
    private GameObject[] gameDisplay;
    

    private void Awake()
    {
        if(S)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            S = this;
            DontDestroyOnLoad(this);
        }
        GameStartState = Instantiate(InitialGameState);
        LevelManager.GameStartState = GameStartState;
        PlayerManager.GameStartState = GameStartState;
    }

    

    // Start is called before the first frame update
    private void Start()
    {
        //Set the Game Message
        gameMessage.text = "Press \"Enter\" to Start";
        gameMessage.enabled = true;

        //Hide UI
        gameDisplay = GameObject.FindGameObjectsWithTag("GameDisplay");
        for(int i = 0; i < gameDisplay.Length; i++)
        {
            gameDisplay[i].SetActive(false);
        }

        //Set the gameobject to player
     // currentPlayer = GameObject.Find("Nezha");
       // currentPlayer.SetActive(false);
        SetGameMenu();
    }

    // Update is called once per frame
    private void Update()
    {
        if(currentState == GameState.MainMenu)
        {
           
            if (Input.GetKeyDown(KeyCode.Return))
            {
                GameObject.Find("GameTextPanel").SetActive(false);
                InitializeGame();
            }
 
        }
        if(currentState == GameState.Paused)
        {
           gameMessage.text = "Game is Paused";
           gameMessage.enabled = true;
        }
        else if(currentState == GameState.Playing)
        {
            gameMessage.enabled = false;
        }
      
    }

    private void InstantiateObjects()
    {
        
       // currentPlayer.SetActive(true);
        SpawnEnemies.S.SpawnAllEnemies();
    }

    private void SetGameMenu()
    {
        //Start the Game at MainMenu
        currentState = GameState.MainMenu;
        //Play respective music for MainMenu
        SoundManagerScript.S.PlayBackgroundMusic(currentState);
        
    }

    private void InitializeGame()
    {
        //Show UI
        for (int i = 0; i < gameDisplay.Length; i++)
        {
            gameDisplay[i].SetActive(true);
        }
        //Set the number of lives
        currentLives = 3;
        //Set the number of coins
        currentCoinAmount = 0;
        //Set the number of pearls
        pearlCount = 0;
        //Display the current amount of lives
        HUDManagerScript.HUDSetter.IncrementLives(currentLives);
        //Set the amount of pearls for the HUD Manager
        HUDManagerScript.HUDSetter.GetAvailablePearls(pearlCount);
        //Set the health
        HUDManagerScript.HUDSetter.SetAvailableHealth(Player.S.health);
        //Instantiate the GameObjects
        InstantiateObjects();
      
        //Player Start
        PlayerStart();
    }

    private void PlayerStart()
    {
        //Game Starts
        currentState = GameState.Playing;
        //Set the starting position
        //startPos = playerPrefab.transform.position;

              
    }

    //Player has died check if the player has another chance
    public void PlayerDied()
    {
        //Player is Dead
        currentState = GameState.Dead;
        //Player Loses a Life
        currentLives--;
        //Display current lives
        HUDManagerScript.HUDSetter.RemoveLife();
        //Destroy the current player
        //Destroy(currentPlayer);

        if(currentLives <= 0)
        {
            //Game is Over
            GameOver();
        }
        else
        {
            //Restart Player at Last Point
            PlayerRespawnScript.S.Respawn();
            //InstantiateObjects();
            Player.S.HasDied = false;
            PlayerStart();
        }
    }

    //Player has lost the game
    public void GameOver()
    {
        //Game is Over
        currentState = GameState.GameOver;
        //Play the respective music for GameOver
        SoundManagerScript.S.PlayBackgroundMusic(currentState);
    }

    //private IEnumerator Respawn(float waitTime)
    //{
    //    PlayerController.S.rb.velocity = new Vector2(0, 0);
    //    PlayerController.S.rb.simulated = false;
    //    playerPrefab.transform.localScale = new Vector3(0, 0, 0);
    //    yield return new WaitForSeconds(waitTime);
    //    playerPrefab.transform.position = startPos;
    //    playerPrefab.transform.localScale = new Vector3(1, 1, 1);
    //    PlayerController.S.rb.simulated = true;

    //}

    //Add coin amount to player's total
    public void UpdateCoinAmount()
    {
        currentCoinAmount++;
        displayCoins.text = currentCoinAmount.ToString();
    }

    //Add talisman amount ot player's total
    public void UpdateTalismanAmount()
    {
        currentTalismanAmount++;
    }

    //Add spirit amount to player's total
    public void UpdateSpiritManaAmount()
    {
        currentSpiritManaAmount++;
        
    }

    //Add to the player's current pearl amount
    public void UpdatePearlAmount()
    {
        //Increment the pearl count
        pearlCount++;
        //Set the HUD for the pearls
        HUDManagerScript.HUDSetter.ChangePearl(HUDManagerScript.HUDSetter.PearlSet.childCount - pearlCount);
    }

    public void PlayerTookDamage()
    {
        HUDManagerScript.HUDSetter.ChangeLitHealthToDull();
    }
}
