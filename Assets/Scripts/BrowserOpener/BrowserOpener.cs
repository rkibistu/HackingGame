using System.Diagnostics;
using UnityEngine;

public class BrowserOpener : MonoBehaviour
{
    [Header("Open Browser")]
    [SerializeField]
    private string url = "https://your-url.com";

    [SerializeField]
    private int width = 0;
    [SerializeField]
    private int height = 0;
    [SerializeField]
    private int x = 0;
    [SerializeField] 
    private int y = 0;

    public void OpenBrowser()
    {
        string args;
        if (width > 0 && height > 0)
        {
            args = $"--new-window \"{url}\" --window-size={width},{height} --window-position={x},{y}";
        }
        else
        {
            args = $"--new-window \"{url}\" --window-position={x},{y} -fullscreen\";";
        }
    

        // Try Chrome
        if (TryStart("chrome", args)) return;

        // Try Edge
        if (TryStart("msedge", args)) return;

        // Try Firefox (note: limited support for window position)
        if (TryStart("firefox", $"-new-window \"{url}\"")) return;

        // Fallback: open in default browser
        Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }

    private bool TryStart(string browser, string args)
    {
        try
        {
            Process.Start(browser, args);
            return true;
        }
        catch
        {
            return false;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
