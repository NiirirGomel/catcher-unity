using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CandyController : MonoBehaviour {

    [SerializeField] bool loadCandy;
    [SerializeField] bool m_gameCandy;
    public bool gameCandy { get { return m_gameCandy; } }
    [SerializeField] string saveValueName;
    [SerializeField] Text scoreText;

    int m_candyCount;
    public int candyCount { get { return m_candyCount; } }

    void Start() {
        scoreText.text = m_candyCount.ToString();
    }

    public void Load() {
        if (loadCandy) {
            string value = GameSaves.LoadValue(saveValueName);
            if (value.Length > 0) {
                m_candyCount = int.Parse(value);
                scoreText.text = m_candyCount.ToString();
            }
        }
    }

    void Save() {
        GameSaves.SaveValue(saveValueName, m_candyCount.ToString());
    }

    public void AddCandy(int count) {
        m_candyCount += count;
        scoreText.text = m_candyCount.ToString();
        if (loadCandy) Save();
    }

    public bool TakeCandy(int count) {
        if (m_candyCount >= count) {
            m_candyCount -= count;
            scoreText.text = m_candyCount.ToString();
            if (loadCandy) Save();
            return true;
        } else {
            return false;
        }
    }

    public void SetCandy(int count) {
        m_candyCount = count;
        scoreText.text = m_candyCount.ToString();
        if (loadCandy) Save();
    }

    public void MoveCandy(CandyController candyController) {
        candyController.AddCandy(m_candyCount);
        if (m_gameCandy) GameScore.SetLastScore(m_candyCount);
    }
}
