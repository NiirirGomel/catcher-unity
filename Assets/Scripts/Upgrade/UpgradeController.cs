using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour {

    [SerializeField] CandyController candyController;
    [SerializeField] string m_title;
    [SerializeField] ItemData[] m_levelData;
    [SerializeField] byte m_nextLevel;
    [SerializeField] string saveValueName;

    public string title { get { return m_title; } }
    public ItemData[] levelData { get { return m_levelData; } }
    public byte nextLevel { get { return m_nextLevel; } }

    void Start() {
        DrawLevel();
    }

    public void Load() {
        string value = GameSaves.LoadValue(saveValueName);
        if (value.Length > 0) {
            m_nextLevel = byte.Parse(value);
            m_levelData[m_nextLevel - 1].upgrade.Invoke();
            DrawLevel();
        }
    }

    void Save() {
        GameSaves.SaveValue(saveValueName, m_nextLevel.ToString());
    }

    void DrawLevel() {
        Image[] levelIndicators = transform.GetChild(2).GetComponentsInChildren<Image>();
        for (int i = 0; i < levelIndicators.Length; i++) {
            if (i <= m_nextLevel) {
                levelIndicators[i].enabled = true;
            } else {
                levelIndicators[i].enabled = false;
            }
        }
    }

    public void UpgradeEvent() {
        if (candyController.TakeCandy(m_levelData[m_nextLevel].cost)) {
            m_levelData[m_nextLevel].upgrade.Invoke();
            m_nextLevel++;
            Save();
            DrawLevel();
            GetComponent<Button>().onClick.Invoke();
        }
    }

    [System.Serializable]
    public class ItemData {
        public int cost;
        public string description;
        public UnityEvent upgrade = new UnityEvent();
    }
}
