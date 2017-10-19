﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Messenger.Model
{
    public class Chat
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<User> Members { get; set; }
    }
}