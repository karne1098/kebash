using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathCount : MonoBehaviour
{

    public TextMeshProUGUI p1DeathCount;
    public TextMeshProUGUI p2DeathCount;
    public TextMeshProUGUI p3DeathCount;
    public TextMeshProUGUI p4DeathCount;
    private List<TextMeshProUGUI> deathCounts = new List<TextMeshProUGUI>();


    // Start is called before the first frame update
    void Start()
    {
        deathCounts.Add(p1DeathCount);
        deathCounts.Add(p2DeathCount);
        deathCounts.Add(p3DeathCount);
        deathCounts.Add(p4DeathCount);

        for(int i = 0; i < deathCounts.Count; i++) 
        {
            deathCounts[i].gameObject.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        int PlayerCount = MultiplayerManager.Instance.PlayerCount;
        List<Movement> playerList = MultiplayerManager.Instance.PlayerScripts;

        for(int i = 0; i < PlayerCount; i++) 
        {
            deathCounts[i].gameObject.SetActive(true);
            //deathCounts[i].text = string.Format("{0}", playerList[i].TimesDied);
            Debug.Log(playerList[i].TimesDied);
            //deathCounts[i].text = playerList[i].TimesDied.ToString;
            //deathCounts[i].setText(playerList[i].TimesDied.ToString);
        }

        
    }
}
