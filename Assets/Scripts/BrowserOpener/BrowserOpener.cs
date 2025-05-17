using System.Diagnostics;
using System.Runtime.InteropServices;
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

    //public void OpenBrowser()
    //{
    //    string argsChromeEdge;
    //    string argsFirefox;
    //    if (width > 0 && height > 0)
    //    {
    //        argsChromeEdge = $"--new-window \"{url}\" --window-size={width},{height} --window-position={x},{y}";
    //        argsFirefox = $"-new-window \"{url}\" --window-size={width},{height} --window-position={x},{y}";
    //    }
    //    else
    //    {
    //        argsChromeEdge = $"--new-window \"{url}\" --window-position={x},{y} --start-fullscreen";
    //        argsFirefox = $"-new-window \"{url}\" --window-position={x},{y} -fullscreen";
    //    }


    //    // Try Chrome
    //    if (TryStart("chrome", argsChromeEdge)) return;

    //    //// Try Edge
    //    if (TryStart("msedge", argsChromeEdge)) return;

    //     //Try Firefox (note: limited support for window position)
    //    if (TryStart("firefox", argsFirefox)) return;

    //    // Fallback: open in default browser
    //    Process.Start(new ProcessStartInfo
    //    {
    //        FileName = url,
    //        UseShellExecute = true
    //    });
    //}

    //private bool TryStart(string browser, string args)
    //{
    //    try
    //    {
    //        Process.Start(browser, args);
    //        return true;
    //    }
    //    catch
    //    {
    //        return false;
    //    }
    //}

    public void OpenBrowser()
    {
        string argsChromeEdge;
        string argsFirefox;

        if (width > 0 && height > 0)
        {
            argsChromeEdge = $"--new-window \"{url}\" --window-size={width},{height} --window-position={x},{y}";
            argsFirefox = $"-new-window \"{url}\" --window-size={width},{height} --window-position={x},{y}";
        }
        else
        {
            argsChromeEdge = $"--new-window \"{url}\" --window-position={x},{y} --start-fullscreen";
            argsFirefox = $"-new-window \"{url}\" --window-position={x},{y} -fullscreen";
        }

        // Windows
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            if (TryStart("chrome", argsChromeEdge)) return;
            if (TryStart("msedge", argsChromeEdge)) return;
            if (TryStart("firefox", argsFirefox)) return;

            // fallback: default browser
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        // macOS
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            var appleScript = $@"
tell application ""Safari""
    activate
    open location ""{url}""
    {(width > 0 && height > 0
            ? $"set bounds of front window to {{{x}, {y}, {x + width}, {y + height}}}"
            : "tell front window to set its fullscreen to true")}
end tell";
            if (TryStart("/usr/bin/osascript", $"-e \"{appleScript.Replace("\"", "\\\"")}\"")) return;
            if (TryStart("google chrome", $"-a \"Google Chrome\" \"{url}\"")) return;
            if (TryStart("firefox", argsFirefox)) return;

            Process.Start(new ProcessStartInfo
            {
                FileName = "open",
                Arguments = $"\"{url}\"",
                UseShellExecute = true
            });
        }
        // Linux
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            if (TryStart("google-chrome", argsChromeEdge)) return;
            if (TryStart("chromium-browser", argsChromeEdge)) return;
            if (TryStart("firefox", argsFirefox)) return;

            Process.Start(new ProcessStartInfo
            {
                FileName = "xdg-open",
                Arguments = $"\"{url}\"",
                UseShellExecute = true
            });
        }
        else
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
    }

    private bool TryStart(string fileName, string args)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = args,
                UseShellExecute = true
            });
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
