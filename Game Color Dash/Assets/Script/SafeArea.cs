using UnityEngine;

public class SafeArea : MonoBehaviour {
    RectTransform rectTransform;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
        RefreshSafeArea();
    }

    void RefreshSafeArea() {
        var safeArea = Screen.safeArea;
        var anchorMin = safeArea.position;
        var anchorMax = anchorMin + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }
}