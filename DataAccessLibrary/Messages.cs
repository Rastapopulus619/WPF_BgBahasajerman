using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bgb_DataAccessLibrary
{
    public class Messages : IMessages
    {
        public string SayHello() => "Hello Trainer";
        public string SayGoodbye() => "Congratulations! You've caught all 151 Pokemon.";
    }
}
