using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIManager : MonoBehaviour
{
    [SerializeField] private Button[] LevelButtonList = new Button[8];
    [SerializeField] private Transform[] StarArray = new Transform[8];
    [SerializeField] private InfoPopups InfoPopups;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < LevelButtonList.Length; i++)
        {
            int levelIndex = i + 1;
            LevelButtonList[i].onClick.AddListener(() => {
                LevelSelectionManager.Instance.GoToLevel(levelIndex);
            });

            LevelButtonList[i].interactable = LevelSelectionManager.Instance.GetLevelUnlock(levelIndex);
            StarArray[i].gameObject.SetActive(LevelSelectionManager.Instance.GetLevelCompleted(levelIndex));
        }

        if(!LevelSelectionManager.Instance.TutorialShown)
            Invoke("ActivateInfoPopups", 1f);
        LevelSelectionManager.Instance.TutorialShown = true;
    }

    void ActivateInfoPopups()
    {
        InfoPopups.InfoButtonPressed();
    }
}
