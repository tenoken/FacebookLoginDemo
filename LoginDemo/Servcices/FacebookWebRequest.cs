using LoginDemo.Servcices.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace LoginDemo.Servcices
{
    public class FacebookWebRequest : IRequest
    {
        private IList<RestResponseCookie> _cookieContainer;
        private DocumentHelper _documentHelper;
        private bool _isLogged;
        private LoginData _loginData;
        private string _pageContent;
        private const string _userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:46.0) Gecko/20100101 Firefox/46.0";
        private const string FACEBOOK_BASE_PATH = "https://mbasic.facebook.com/";
        private const string FACEBOOK_LOGIN_PATH = "https://mbasic.facebook.com/login.php";
        private const string DO_NOT_SAVE_LOGIN_PATH = "https://mbasic.facebook.com/logout.php?h=AfcWU37xLCs5fd-zTXA&amp;t=1610912815&amp;button_name=logout&amp;button_location=mbasic_save_pw_interstitial";
        private const string LOGOUT_PATH = "https://mbasic.facebook.com/login/save-password-interstitial/?ref_component=mbasic_footer&amp;ref_page=%2Fwap%2Fhome.php&amp;refid=8";

        public FacebookWebRequest()
        {
            _documentHelper = new DocumentHelper();
            _cookieContainer = new List<RestResponseCookie>();
            _pageContent = "";
            _loginData = new LoginData();
        }

        public async Task<string> GetHomePage(string user, string password)
        {
            if (!_isLogged)
            {
                _loginData.LoginDataBuilder(user, password);
                _pageContent = await Login();
            }

            _pageContent = await Get(FACEBOOK_BASE_PATH);

            _pageContent = await Logout();

            return _pageContent;
        }

        private async Task<string> Login()
        {
            _pageContent = await Get(FACEBOOK_BASE_PATH);
            _pageContent = await Post(FACEBOOK_LOGIN_PATH, _pageContent, ActionParams.Login);
            _isLogged = true;
            return _pageContent;
        }
        private async Task<string> Logout()
        {
            if (!_isLogged)
                return "";

            _pageContent = await Get(LOGOUT_PATH);
            _pageContent = await Post(DO_NOT_SAVE_LOGIN_PATH, _pageContent, ActionParams.Logout);
            ClearCookies();
            _isLogged = false;
            return _pageContent;
        }   

        private async Task<string> Get(string uri, string pageContent = null)
        {
            var client = new RestClient(uri);
            client.Timeout = -1;
            client.FollowRedirects = false;
            var request = new RestRequest(Method.GET);
            client.UserAgent = _userAgent;
            request.AddHeader("Pragma", "no-cache");
            AddCookie(request);

            IRestResponse response = client.Execute(request);

            SetHeadersCookies(response.Cookies);

            return response.Content;
        }

        private void AddCookie(RestRequest request)
        {
            foreach (var item in _cookieContainer)
            {
                request.AddCookie(item.Name, item.Value);
            }
        }

        private void ClearCookies()
        {
            _cookieContainer.Clear();
        }

        private async Task<string> Post(string uri, string pageContent = null, ActionParams action = ActionParams.Default)
        {
            var client = new RestClient(uri);
            client.Timeout = -1;
            client.FollowRedirects = false;
            var request = new RestRequest(Method.POST);
            client.UserAgent = _userAgent;
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            AddCookie(request);

            //TODO: transform in a keyvaluepair method
            if (!string.IsNullOrEmpty(pageContent))
            {
                var param = _documentHelper.GetParams(pageContent, action);

                for (int i = 0; i < param.Item1.Length; i++)
                {
                    request.AddParameter(param.Item1[i], param.Item2[i]);
                }

            }           

            if (!_isLogged)
                AddLoginDataParameter(request);

            IRestResponse response = client.Execute(request);

            SetHeadersCookies(response.Cookies);

            return response.Content;
        }

        private void AddLoginDataParameter(RestRequest request)
        {
            request.AddParameter("email", _loginData.User);
            request.AddParameter("pass", _loginData.Password);
        }

        private void SetHeadersCookies(IList<RestResponseCookie> cookies)
        {
            foreach (var cookie in cookies)
                _cookieContainer.Add(cookie);
        }

        //TODO: extract this class out
        private class LoginData
        {
            private string _user;
            private string _password;

            public LoginData()
            {              
            }

            internal void LoginDataBuilder(string user, string password)
            {
                _user = user;
                _password = password;
            }

            public string Password { get => _password;}
            public string User { get => _user;}
        }

        public enum ActionParams
        {
            //[Description("Action for default post request")]
            Default,
            //[Description("Get the parameters from body to perform post request in order to login")]
            Login,         
            //[Description("Get the parameters from body to perform post request in order to logout")]
            Logout
        }
    }
}