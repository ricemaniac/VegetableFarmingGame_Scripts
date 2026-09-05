using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_PushButton : MonoBehaviour
{
    public Button targetButton; 

    void OnEnable()
    {
        // ボタンがクリックされたら、下の「OnButtonClicked」を実行する予約を入れる
        if (targetButton != null)
        {
            targetButton.onClick.AddListener(OnButtonClicked);
        }
    }

    void OnDisable()
    {
        if (targetButton != null)
        {
            targetButton.onClick.RemoveListener(OnButtonClicked);
        }
    }

    void OnButtonClicked()
    {
        TutorialManager.Instance.AdvanceTutorial();
    }
}
