using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathCountManager : MonoBehaviour
{
    private int[] deathCounts = new int[4];
    [SerializeField] private TextMeshProUGUI topLeft;
    [SerializeField] private TextMeshProUGUI topRight;
    [SerializeField] private TextMeshProUGUI bottomLeft;
    [SerializeField] private TextMeshProUGUI bottomRight;

    public void incrementDeath(int playerNumber) {
        deathCounts[playerNumber] += 1;
    }

    void Update() {
        
    }
}
