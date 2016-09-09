﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using epay3.Web.Api.Sdk.Api;
using epay3.Web.Api.Sdk.Model;
using epay3.Web.Api.Sdk.Client;
using System.Net;
using System.Linq;

namespace epay3.Web.Api.Tests
{
    [TestClass]
    public class TokenPageSessionsFixture
    {
        private TokenPageSessionsApi _tokenPageSessionsApi;
        private const long ProcessingAccountId = 3;

        private string Uri
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ApiUri"];
            }
        }

        private string Key
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ApiKey"];
            }
        }

        private string Secret
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ApiSecret"];
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            _tokenPageSessionsApi = new TokenPageSessionsApi(Uri);

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(Key + ":" + Secret);

            _tokenPageSessionsApi.Configuration.AddDefaultHeader("Authorization", "Basic " + System.Convert.ToBase64String(plainTextBytes));
        }

        [TestMethod]
        public void Should_Return_An_Id_Upon_Success_With_No_Processing_Id()
        {
            var postTokenPageSessionRequestModel = new PostTokenPageSessionRequestModel
            {
                AttributeValues = new System.Collections.Generic.Dictionary<string, string>
                {
                    { "parameter 1", "value 1" },
                    { "parameter 2", "value 2" }
                },
                SuccessUrl = "https://www.example.com"
            };

            var id = _tokenPageSessionsApi.TokenPageSessionsPost(postTokenPageSessionRequestModel, null);

            // Should return a valid Id.
            Assert.IsNotNull(id);
        }

        [TestMethod]
        public void Should_Return_An_Id_Upon_Success_With_A_Processing_Id()
        {
            var postTokenPageSessionRequestModel = new PostTokenPageSessionRequestModel
            {
                AttributeValues = new System.Collections.Generic.Dictionary<string, string>
                {
                    { "param1", "parameter value 1" },
                    { "param2", "parameter value 2" }
                }
            };

            var id = _tokenPageSessionsApi.TokenPageSessionsPost(postTokenPageSessionRequestModel, ProcessingAccountId);

            // Should return a valid Id.
            Assert.IsNotNull(id);
        }

        [TestMethod]
        public void Should_Fail_With_An_Invalid_Processing_Account_Id()
        {
            var postTokenPageSessionRequestModel = new PostTokenPageSessionRequestModel
            {

            };

            try
            {
                var id = _tokenPageSessionsApi.TokenPageSessionsPost(postTokenPageSessionRequestModel, 9999999);

                Assert.Fail();
            }
            catch(ApiException exception)
            {
                Assert.AreEqual(401, exception.ErrorCode);
            }
        }
    }
}
