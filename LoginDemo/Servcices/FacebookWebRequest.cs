using LoginDemo.Domain.Model;
using LoginDemo.Domain.Models.Interfaces;
using LoginDemo.Servcices.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginDemo.Servcices
{
    public partial class FacebookWebRequest : WebRequest//, IRequest
    {
        private bool _isLogged;
        private ActionParams _action;
        private string _pageContent;
        private const string FACEBOOK_BASE_PATH = "https://mbasic.facebook.com/";
        private const string FACEBOOK_LOGIN_PATH = "https://mbasic.facebook.com/login.php";
        private const string DO_NOT_SAVE_LOGIN_PATH = "https://mbasic.facebook.com/logout.php?h=AfcWU37xLCs5fd-zTXA&amp;t=1610912815&amp;button_name=logout&amp;button_location=mbasic_save_pw_interstitial";
        private const string LOGOUT_PATH = "https://mbasic.facebook.com/login/save-password-interstitial/?ref_component=mbasic_footer&ref_page=%2Fwap%2Fhome.php&refid=8";

        private const string HOME_LOGINFORM_ID = "id=\"m_login_email\"";

        public bool IsLogged { get => _isLogged; }

        public FacebookWebRequest(IDocumentHelper documentHelper, IList<RestResponseCookie> responseCookies, Credential credential):
            base(documentHelper,responseCookies, credential)
        {
            _pageContent = "";
            _action = ActionParams.Default;
        }

        public override async Task<string> GetHomePage(string user, string password)
        {
            //TODO: Throw argument exception when user is not able to login
            await VerifyIfIsNecessaryDoLogin(user, password);
            var homePageContent = await Get(FACEBOOK_BASE_PATH);
            VerifyIfIsSuccessfullyLoged(homePageContent);
            Logout();
            return homePageContent;
        }

        private void VerifyIfIsSuccessfullyLoged(string pageContent)
        {
            if (!pageContent.Contains(HOME_LOGINFORM_ID))
                _isLogged = true;
        }

        private async Task VerifyIfIsNecessaryDoLogin(string user, string password)
        {
            if (!_isLogged)
            {
                _credential.CredentialBuilder(user, password);
                _pageContent = await Login();
            }
        }

        private async Task<string> Login()
        {
            _pageContent = await Get(FACEBOOK_BASE_PATH);
            _pageContent = await Post(FACEBOOK_LOGIN_PATH, _pageContent, ActionParams.Login);

            return _pageContent;
        }

        private async void Logout()
        {
            if (!_isLogged)
                return;

            _pageContent = await TryGetLogoutPageContent();
            _pageContent = await Post(DO_NOT_SAVE_LOGIN_PATH, _pageContent, ActionParams.Logout);
            ClearCookies();
            _isLogged = false;
        }

        private async Task<string> TryGetLogoutPageContent()
        {
            var attempts = 0;
            var content = "";

            while (_pageContent.Length < 100 && attempts < 3)
            {
                content = await Get(LOGOUT_PATH);
                attempts++;
            }

            return content;
        }

        private void ClearCookies()
        {
            _cookieContainer.Clear();
        }
    }
}