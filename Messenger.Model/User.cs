using System;
using System.Collections.Generic;
using System.Text;

namespace Messenger.Model
{
    public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public byte[] Avatar { get; set; }
    }
}
