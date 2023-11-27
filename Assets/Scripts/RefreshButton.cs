using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FirstDayIn.Network;

public class RefreshButton : MonoBehaviour
{
    private Button refreshButton;

    private void Awake() {
        if (refreshButton == null) {
            refreshButton = GetComponent<Button>();
        }

        refreshButton.onClick.AddListener(Refresh);
    }

    private void Refresh() {
        StartCoroutine(RefreshAndWait());
    }

    private IEnumerator RefreshAndWait() {
        refreshButton.interactable = false;
        // GameManager.instance.RefreshSessionListUI();

        yield return new WaitForSeconds(3f);
        refreshButton.interactable = true;
    }
}
