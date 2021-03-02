using LoginDemo.Domain.Model;
using LoginDemo.Servcices;
using Moq;
using RestSharp;
using System;
using System.Collections.Generic;
using Xunit;

namespace LoginDemoTest
{
    public class FacebookWebRequestUnitTests
    {
        //Red, Green, Refactor
        [Fact]
        public void Should_ReturnLoginPage_When_Given_IncorrectUserAndPassword()
        {
            //Arrange
            var mockDocumentHelper = new Mock<DocumentHelper>();
            var mockCredential = new Mock<Credential>();
            var mockResponseCookie = new List<RestResponseCookie>();
            var facebookWebResquest = new FacebookWebRequest(
                mockDocumentHelper.Object,
                mockResponseCookie,
                mockCredential.Object);

            var user = "potato.potato123@gmail.com";
            var password = "344545e";
            var expectedSubstring = "id=\"m_login_email\"";

            //Act
            var sut = facebookWebResquest.GetHomePage(user, password);

            //Assert
            Assert.Contains(expectedSubstring, sut.Result);
        }

        [Fact]
        public void Should_ReturnFacebookHomePage_When_Given_CorrectUserAndPassword()
        {
            //Arrange
            var mockDocumentHelper = new Mock<DocumentHelper>();
            var mockCredential = new Mock<Credential>();
            var mockResponseCookie = new List<RestResponseCookie>();
            var facebookWebResquest = new FacebookWebRequest(
                mockDocumentHelper.Object,
                mockResponseCookie,
                mockCredential.Object);

            //Fill the variables with your facebook credential 
            //before run the test
            var user = "";
            var password = "";
            var expectedSubstring = "id=\"mbasic_logout_button\"";

            //Act
            var sut = facebookWebResquest.GetHomePage(user, password);

            //Assert
            Assert.Contains(expectedSubstring, sut.Result);
        }
    }
}
