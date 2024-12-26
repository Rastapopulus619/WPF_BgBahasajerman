using System.Windows.Input;

namespace BgB_TeachingAssistant.Services
{
    public interface IPromptService
    {
        bool ShowPrompt(string title, object content, ICommand okCommand, ICommand cancelCommand);
    }

}
