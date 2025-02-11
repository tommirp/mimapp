using Microsoft.Maui.ApplicationModel;
using System.Diagnostics;
using System.Threading.Tasks;

#if ANDROID
using Android.Content;
using Android.Net;
#endif

#if IOS
using Foundation;
using UIKit;
#endif

namespace MimApp.Utils;
public class MailHelper
{
    public static async Task OpenEmailClientAsync(string to, string subject, string body)
    {
        string emailUri = $"mailto:{to}?subject={System.Uri.EscapeDataString(subject)}&body={System.Uri.EscapeDataString(body)}";

        // Try Launcher for all platforms
        if (await Launcher.CanOpenAsync(emailUri))
        {
            await Launcher.OpenAsync(emailUri);
            return;
        }

#if ANDROID
        // Android Fallback: Open Email App
        var context = Android.App.Application.Context;
        var emailIntent = new Intent(Intent.ActionSendto);
        emailIntent.SetData(Android.Net.Uri.Parse(emailUri));
        emailIntent.AddFlags(ActivityFlags.NewTask);
        context.StartActivity(emailIntent);
#endif

#if IOS
        // iOS Fallback: Open Email App
        var nsUrl = new NSUrl(emailUri);
        UIApplication.SharedApplication.OpenUrl(nsUrl);
#endif

#if WINDOWS
        // Windows Fallback: Open Default Mail App
        Process.Start(new ProcessStartInfo
        {
            FileName = emailUri,
            UseShellExecute = true
        });
#endif
    }
}