using Runtime.Player;
using TMPro;
using UnityEngine;

namespace Runtime {
    public class FeatherCounter : MonoBehaviour {
        [SerializeField]
        CameraController target = default;
        [SerializeField]
        TextMeshProUGUI text = default;
        void Start() {
            target = FindObjectOfType<CameraController>();
        }
        void Update() {
            text.text = target.featherCount.ToString();
        }
    }
}