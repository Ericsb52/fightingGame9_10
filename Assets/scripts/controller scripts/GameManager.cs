using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Lists")]
    public Color[] player_colors;
    public List<PlayerController> players_list = new List<PlayerController>();
    public Transform[] spawn_points;


    [Header("prefab refs")]
    
    public GameObject playerContPrefab;

    [Header("Components")]
    private AudioSource audio;
    public AudioClip[] game_fx;
    public Transform containerGroup;
    public TextMeshProUGUI timeText;

    [Header("level vars")]
    public float startTime;
    public float curTime;
    List<PlayerController> winningplayers;
    public bool canJoin;


    // singleton
    public static GameManager instance;

    private void Awake()
    {
        canJoin = true;
        instance = this;
        audio= GetComponent<AudioSource>();
        containerGroup = GameObject.FindGameObjectWithTag("UIContainer").GetComponent<Transform>();
        startTime = PlayerPrefs.GetFloat("roundTimer", 100);
        winningplayers = new List<PlayerController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        curTime = startTime;
        timeText.text = curTime.ToString();

    }
    public void FixedUpdate()
    {
        curTime -= Time.deltaTime;
        timeText.text = ((int)curTime).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(curTime <= 0 )
        {
            //find winner
            int highscore = 0;
            int index = 0;
           
            foreach ( PlayerController player in players_list)
            {

                if (player.score > highscore )
                {
                    winningplayers.Clear();
                    highscore = player.score;
                    index = players_list.IndexOf(player);
                    winningplayers.Add(player);
                }
                else if (player.score == highscore) {

                    winningplayers.Add(player);
                }
                
            }

            if (winningplayers.Count > 1)
            {
                canJoin = false;
                // this is a tie

                // play sound to indicate over time
                audio.PlayOneShot(game_fx[1]);

                foreach (PlayerController player in players_list)
                {
                    if (!winningplayers.Contains(player))
                    {
                        player.drop_out();
                    }
                }
                curTime = 30;
            }
            else
            {
                PlayerPrefs.SetInt("colorIndex", index);
                SceneManager.LoadScene("winScreen");
            }

            

        }
    }


    public void onPlayerJoined(PlayerInput player)
    {
        if (canJoin)
        {
            //play sound
            audio.PlayOneShot(game_fx[0]);
            //set player color when joined
            player.GetComponentInChildren<SpriteRenderer>().color = player_colors[players_list.Count];

            //create a ui container
            PlayerContainerUI cont = Instantiate(playerContPrefab, containerGroup).GetComponent<PlayerContainerUI>();
            // asigne cont to a player
            player.GetComponent<PlayerController>().setUI(cont);
            cont.initialize(player_colors[players_list.Count]);


            // added the player to the players list
            players_list.Add(player.GetComponent<PlayerController>());
            // choose Spawn point 
            player.transform.position = spawn_points[Random.Range(0, spawn_points.Length)].position;
        }
        
    }

}
