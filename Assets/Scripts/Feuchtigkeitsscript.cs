using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime {
public class Feuchtigkeitsscript : MonoBehaviour
{
        public GameObject anzeigeBalken;
        public GameObject tropfen;
        RectTransform balken;
        RectTransform tropfenTransform;
        public float feuchtigkeit;

    // Start is called before the first frame update
    void Start()
    {
            tropfen.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            balken = anzeigeBalken.GetComponent<RectTransform>();
            tropfenTransform = tropfen.GetComponent<RectTransform>();
            Debug.Log(tropfenTransform.localPosition);
            Debug.Log((float)balken.rect.width * feuchtigkeit);
        }

    // Update is called once per frame
    void Update()
    {
            Debug.Log((float)balken.rect.width * feuchtigkeit);
            tropfenTransform.localPosition = new Vector3((float)balken.rect.width * feuchtigkeit, 0, 0);
    }
}
}