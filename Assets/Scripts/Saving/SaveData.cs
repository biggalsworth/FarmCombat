using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveData
{
    public SaveData instance;
    //PlayerStats stats = PlayerStats.instance;

    //Player Stats
    public int playerHealth;
    public int foodAmount;
    public int soulAmount;
    //Player Position
    public float[] playerPosition;

    //Player Inventory
    public string[] inv;

    //Player bosses
    public string[] bossList;

    //Active Territories
    public string[] activeAreas;

    //Current Quest
    public string Title;
    public string Descr;

    public bool Played;


    public SaveData(PlayerStats stats, Rendering renderer, QuestHolder Quest)
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = stats.health;
        foodAmount = stats.foodAmount;
        soulAmount = stats.soulAmount;

        Played = stats.PlayedBefore;

        playerPosition = new float[3];
        /*
        playerPosition[0] = Player.transform.position.x;
        playerPosition[1] = Player.transform.position.y;
        playerPosition[2] = Player.transform.position.z;
        */

        playerPosition[0] = stats.CheckPoint.x;
        playerPosition[1] = stats.CheckPoint.y;
        playerPosition[2] = stats.CheckPoint.z;

        inv = new string[stats.inv.Count];

        for(int i = 0; i < inv.Length; i++)
        {
            inv[i] = stats.inv[i];
            Debug.Log(stats.inv[i]);
        }

        bossList = new string[stats.bosses.Count];
        for (int i = 0; i < bossList.Length; i++)
        {
            bossList[i] = stats.bosses[i];
        }

        activeAreas = new string[renderer.activatedAreas.Count];
        for (int i = 0; i < activeAreas.Length; i++)
        {
            activeAreas[i] = renderer.activatedAreas[i];
        }

        Title = Quest.quest.title;
        Descr = Quest.quest.descr;
    }

}
