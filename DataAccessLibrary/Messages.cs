using Bgb_DataAccessLibrary.Contracts.IMessages;

namespace Bgb_DataAccessLibrary
{
    public class Messages : IMessages
    {
        public string SayHello() => "Hello Trainer";
        public string SayGoodbye() => "Congratulations! You've caught all 151 Pokemon.";
    }
}
