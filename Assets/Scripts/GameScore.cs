using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScore : MonoBehaviour {

    static GameScore m_gameScore;
    public static GameScore gameScore { get { return m_gameScore; } }

    [SerializeField] CandyController candy;
    [SerializeField] UnityEngine.UI.Text lastScoreText;
    [SerializeField] UnityEngine.UI.Image lastRating;
    [SerializeField] UnityEngine.UI.Button multiplicationBtn;
    [SerializeField] Rating[] ratings;
    [SerializeField] ScoreItem[] gameItems;

    int multiplication;
    int lastScore;

    public static void SetLastScore(int score) {
        if (m_gameScore != null) {
            m_gameScore.lastScore = score;
            m_gameScore.lastScoreText.text = m_gameScore.lastScore.ToString();
        }
    }

    void Awake () {
        m_gameScore = this;
        gameObject.SetActive(false);
    }

    public void CatchItem(ItemController item) {
        for (int i = 0; i < gameItems.Length; i++) {
            if (gameItems[i].item.name + "(Clone)" == item.name) {
                gameItems[i].catchCount++;
                break;
            }
        }
    }

    public void CrashItem(ItemController item) {
        for (int i = 0; i < gameItems.Length; i++) {
            if (gameItems[i].item.name + "(Clone)" == item.name) {
                gameItems[i].crashCount++;
                break;
            }
        }
    }

    public void StartGame() {
        for (int i = 0; i < gameItems.Length; i++) {
            gameItems[i].catchCount = 0;
            gameItems[i].crashCount = 0;
        }
    }

    public void EndGame() {
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        int presentCount = 0;
        float profit = 0;
        for (int i = 0; i < gameItems.Length; i++) {
            gameItems[i].textField.text = gameItems[i].catchCount + " / " + (gameItems[i].catchCount + gameItems[i].crashCount);
            //lastItems[i].textField.text = " x " + lastItems[i].catchCount.ToString();
            if (gameItems[i].catchCount + gameItems[i].crashCount > 0 && gameItems[i].item.GetComponent<PresentController>() != null) {
                profit += (float) gameItems[i].catchCount / (gameItems[i].catchCount + gameItems[i].crashCount);
                presentCount++;
            }
        }
        if (presentCount > 0) profit /= presentCount;
        for (int i = 0; i < ratings.Length; i++) {
            if (profit <= ratings[i].profit) {
                lastRating.sprite = ratings[i].sprite;
                multiplication = ratings[i].multiplication;
                multiplicationBtn.GetComponentInChildren<UnityEngine.UI.Text>().text = "x " + ratings[i].multiplication.ToString();
                if (multiplication > 1) {
                    multiplicationBtn.interactable = true;
                } else {
                    multiplicationBtn.interactable = false;
                }
                break;
            }
        }
    }

    public void MultiplicationScore() {
        candy.AddCandy(lastScore * (multiplication - 1));
        lastScore *= multiplication;
        lastScoreText.text = lastScore.ToString();
    }

    [System.Serializable]
    struct ScoreItem {
        public ItemController item;
        public UnityEngine.UI.Text textField;
        public int catchCount;
        public int crashCount;
    }

    [System.Serializable]
    struct Rating {
        public float profit;
        public int multiplication;
        public Sprite sprite;
    }
}
