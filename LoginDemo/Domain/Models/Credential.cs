using LoginDemo.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoginDemo.Domain.Model
{
    public class Credential : ICredential
    {
        private string _user;
        private string _password;

        public Credential()
        {
        }

        public void CredentialBuilder(string user, string password)
        {
            _user = user;
            _password = password;
        }

        public string GetUser()
        {
            return _user;
        }

        public string GetPassword()
        {
            return _password;
        }

        public string Password { get => _password; }
        public string User { get => _user; }
    }
}
