using System.Windows.Input;

namespace Bgb_DataAccessLibrary.Contracts.IServices.IDialog
{
    public interface IPromptService
    {
        /// <summary>
        /// Shows a prompt with OK and Cancel buttons, executing commands based on user choice.
        /// </summary>
        /// <param name="title">The title of the prompt.</param>
        /// <param name="content">The content or message to display.</param>
        /// <param name="okCommand">The command to execute when OK is clicked.</param>
        /// <param name="cancelCommand">The command to execute when Cancel is clicked.</param>
        /// <returns>True if OK was clicked, false otherwise.</returns>
        bool ShowOkCancelCommandChoicePrompt(string title, string message, ICommand okCommand, ICommand cancelCommand);

        /// <summary>
        /// Shows a basic prompt with OK and Cancel buttons, returning a bool based on user choice.
        /// </summary>
        /// <param name="title">The title of the prompt.</param>
        /// <param name="message">The message to display.</param>
        /// <returns>True if OK was clicked, false otherwise.</returns>
        bool ShowOkCancelPrompt(string title, string message);

        /// <summary>
        /// Shows an informational prompt with only an OK button, no further action is required.
        /// </summary>
        /// <param name="title">The title of the prompt.</param>
        /// <param name="message">The informational message to display.</param>
        void ShowInformationPrompt(string title, string message);
    }
}
