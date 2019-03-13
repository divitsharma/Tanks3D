using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{

    [SerializeField]
    GameObject scoreCardPrefab;

    Dictionary<int, Text> scoresByID = new Dictionary<int, Text>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddScoreCard(int playerId)
    {
        GameObject scoreCard = Instantiate(scoreCardPrefab, this.transform);
        foreach (Text text in scoreCard.GetComponentsInChildren<Text>())
        {
            if (text.gameObject.name == "PlayerName")
            {
                text.text = "Player " + playerId.ToString();
            }
            else if (text.gameObject.name == "Score")
            {
                scoresByID.Add(playerId, text);
            }
        }
    }

    public void HandleScoreChanged(int playerId, int newScore)
    {
        scoresByID[playerId].text = newScore.ToString();
    }
}
