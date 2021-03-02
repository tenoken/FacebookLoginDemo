using LoginDemo.Domain.Model;
using LoginDemo.Servcices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace LoginDemoTest
{
    public class DocumentHelperUnitTests
    {
        //Red, Green, Refactor
        [Fact]
        public void Should_GetLoginParams_When_Given_LoginPage()
        {
            //Arrange
            var documentHelper = new DocumentHelper();
            var credential = new Credential();
            var pageContent = "";

            using (FileStream fs = File.Open(@"../../../TestFiles/Login/Login.html", FileMode.Open))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);

                while (fs.Read(b, 0, b.Length) > 0)
                {
                    pageContent += temp.GetString(b);
                }
            }

            //Act
            var sut = documentHelper.GetParams(pageContent, ActionParams.Login, credential);

            //Assert
            Assert.Equal(9, sut.Keys.Count);
        }

        [Fact] 
        public void Should_GetLogoutParams_When_Given_LogoutPage()
        {
            //Arrange
            var documentHelper = new DocumentHelper();
            var credential = new Credential();
            var pageContent = "";

            using (FileStream fs = File.Open(@"../../../TestFiles/Logout/Logout.html", FileMode.Open))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);

                while (fs.Read(b, 0, b.Length) > 0)
                {
                    pageContent += temp.GetString(b);
                }
            }

            //Act
            var sut = documentHelper.GetParams(pageContent, ActionParams.Logout, credential);

            //Assert
            Assert.Equal(2, sut.Keys.Count);
        }
    }
}
