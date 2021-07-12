using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime {
    public class Menu : MonoBehaviour {
        public GameObject settings;
        public GameObject credits;
        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void StartGame() {
            SceneManager.LoadScene("Rootscene");
        }

        public void OpenCredits() {
            credits.SetActive(!credits.activeSelf);
        }

        public void OpenSetting() {
            settings.SetActive(!settings.activeSelf);
        }

        public void Quit() {
            Application.Quit();
        }




    }
}
