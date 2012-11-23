﻿using System;
using System.Collections.Generic;

namespace XamarinEvolveSSLibrary
{
    public class UserResponse
    {
        private List<User> _users;
        public List<User> Users 
        {
            get
            {
                return _users;
            }
            set
            {
                _users = value;

                foreach (User user in _users)
                {
                    // Never send back the password
                    user.password = null;
                }
            }
        }
        
        public Exception Exception { get; set; }
    }
}
