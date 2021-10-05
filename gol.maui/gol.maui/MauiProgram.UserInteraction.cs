using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace gol.maui
{
    public static partial class MauiProgram
    {
        public delegate Task<string> DisplayPromptDelegate(string title,
        string message,
        string accept = "OK",
        string cancel = "Cancel",
        string placeholder = null,
        string initialValue = null,
        int maxLength = -1,
        Keyboard keyboard = null);

        public delegate Task<string> DisplayActionSheetDelegate(string title, string cancel, string destruction, params string[] buttons);

        private static async Task<string> ChooseFileNamePrompt(string title,
            string message,
            string accept = null,
            string cancel = null,
            string placeholder = null,
            string initialValue = null,
            int maxLength = -1,
            Keyboard keyboard = null)
        {
            var result = await Application.Current.MainPage.DisplayPromptAsync(title,
                message,
                accept,
                cancel,
                placeholder,
                maxLength,
                keyboard,
                initialValue);

            return result;
        }

        private static async Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons)
        {
            var result = await Application.Current.MainPage.DisplayActionSheet(title, cancel, destruction, buttons);
            return result;
        }
    }
}