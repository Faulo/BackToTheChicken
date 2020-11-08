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

        // Start is called before the first frame update
        void Start() {
            tropfen.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            balken = anzeigeBalken.GetComponent<RectTransform>();
            tropfenTransform = tropfen.GetComponent<RectTransform>();
            //Debug.Log(tropfenTransform.localPosition);
            //Debug.Log((float)balken.rect.width * feuchtigkeit);
        }

        // Update is called once per frame
        void Update() {
            //Debug.Log(balken.rect.width * feuchtigkeit);
            if (target) {
                tropfenTransform.localPosition = new Vector3(balken.rect.width * target.wetness, 0, 0);
            }
        }
    }
}