using System;
using System.IO;
using UnityEngine;

namespace Runtime.Effects {
    [CreateAssetMenu(fileName = "Screenshot_New", menuName = "Effects/Screenshot")]
    public class ScreenshotEffect : Effect {
        [SerializeField]
        string outputDirectory = "Screenshots";
        [SerializeField]
        string fileBaseName = "yyyy-MM-dd HH-mm-ss";
        [SerializeField, Range(1, 8)]
        int sizeMultiplier = 1;

        public override void Invoke(GameObject context) {
            var file = new FileInfo(Path.Combine(outputDirectory, $"{DateTime.Now.ToString(fileBaseName)}.png"));
            if (!file.Directory.Exists) {
                file.Directory.Create();
            }
            ScreenCapture.CaptureScreenshot(file.FullName, sizeMultiplier);
            Debug.Log($"Saved screenshot to: {file}");
        }
    }
}