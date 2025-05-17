using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace WebserverAPI
{
    public class GameProgressManager : MonoBehaviour
    {
        public  void GetProgressLevel()
        {
            var webclient = WebClientService.Instance;
            webclient.GetProgressLevel((success, message) =>
            {

                if (success)
                {
                    // nothing more to do here
                    Debug.Log("Progress retrieved successful: " + webclient.ProgressLevel);

                    // based on progress level, redirect or display the appropriate game
                }
                else
                {
                    // implement error handling logic here
                    Debug.LogError("  Error on retrieving progress level " + message);
                }
            });
        }

        public  void UpdateProgressLevel(int nextLevel)
        {
            int progressLevel;

            var webclient = WebClientService.Instance;

            // check if the level passed is greater than the current progress level
            if (nextLevel > webclient.ProgressLevel)
            {
                progressLevel = nextLevel;
                webclient.UpdateProgressLevel(progressLevel, (success, message) =>
                {
                    if (success)
                    {
                        // implement update progress level success logic here -> redirect to next game
                        Debug.Log("Update progress successful: " + webclient.ProgressLevel);
                    }
                    else
                    {
                        // implement update progress level failed logic here -> dk
                        Debug.LogError("Update progress failed: " + message);
                    }
                });
            }
            

            


        }
    }
}
