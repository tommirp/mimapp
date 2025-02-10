using Microsoft.Maui.Platform;
#if IOS
using UIKit;
#elif ANDROID
using Android.Views.InputMethods;
using Android.App;
using Android.Content;
#endif

namespace MimApp.Utils // Change "YourAppName" to match your project namespace
{
    public static class KeyboardHelper
    {
        public static void CloseKeyboard()
        {
#if ANDROID
                if (Platform.CurrentActivity is Android.App.Activity activity)
                {
                    var inputMethodManager = (InputMethodManager)activity
                        .GetSystemService(Context.InputMethodService);
                    var token = activity?.CurrentFocus?.WindowToken;
                    inputMethodManager?.HideSoftInputFromWindow(token, HideSoftInputFlags.None);
                }
#elif IOS
            if (UIApplication.SharedApplication.KeyWindow is { } window)
            {
                window.EndEditing(true);
            }
#endif
        }
    }
}