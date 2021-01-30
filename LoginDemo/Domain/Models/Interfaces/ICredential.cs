using System;
using System.Collections.Generic;
using System.Text;

namespace LoginDemo.Domain.Models.Interfaces
{
    public interface ICredential
    {
        void CredentialBuilder(string user, string password);
        string GetUser();
        string GetPassword();
    }
}
