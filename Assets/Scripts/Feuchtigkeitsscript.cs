using Runtime.Physics;
using UnityEngine;

namespace Runtime {
    public class Feuchtigkeitsscript : MonoBehaviour {
        [SerializeField]
        WindRecipient target = default;
        [SerializeField]
        GameObject anzeigeBalken = default;
        [SerializeField]
        GameObject tropfen = default;

        RectTransform balken;
        RectTransform tropfenTransform;

        void Start() {
            tropfen.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            balken = anzeigeBalken.GetComponent<RectTransform>();
            tropfenTransform = tropfen.GetComponent<RectTransform>();
        }
        void Update() {
            if (target) {
                tropfenTransform.localPosition = new Vector3(balken.rect.width * target.wetness, 0, 0);
            }
        }
    }
}