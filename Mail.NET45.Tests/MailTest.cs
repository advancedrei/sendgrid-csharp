﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTestExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using SendGrid.SmtpApi;

namespace SendGrid.Tests
{
    [TestClass]
    public class MailTest : BaseTest
    {
        [TestMethod]
        public void TestDisableGravatar()
        {
            var header = new Header();
            var sendgrid = new Mail(header);

            sendgrid.DisableGravatar();

            String json = header.JsonString();
            Assert.AreEqual("{\"filters\" : {\"gravatar\" : {\"settings\" : {\"enable\" : \"0\"}}}}", json);
        }

        [TestMethod]
        public void TestDisableOpenTracking()
        {
            var header = new Header();
            var sendgrid = new Mail(header);

            sendgrid.DisableOpenTracking();

            String json = header.JsonString();
            Assert.AreEqual("{\"filters\" : {\"opentrack\" : {\"settings\" : {\"enable\" : \"0\"}}}}", json);
        }

        [TestMethod]
        public void DisableClickTracking()
        {
            var header = new Header();
            var sendgrid = new Mail(header);

            sendgrid.DisableClickTracking();

            String json = header.JsonString();
            Assert.AreEqual("{\"filters\" : {\"clicktrack\" : {\"settings\" : {\"enable\" : \"0\"}}}}", json);
        }

        [TestMethod]
        public void DisableSpamCheck()
        {
            var header = new Header();
            var sendgrid = new Mail(header);

            sendgrid.DisableSpamCheck();

            String json = header.JsonString();
            Assert.AreEqual("{\"filters\" : {\"spamcheck\" : {\"settings\" : {\"enable\" : \"0\"}}}}", json);
        }

        [TestMethod]
        public void DisableUnsubscribe()
        {
            var header = new Header();
            var sendgrid = new Mail(header);

            sendgrid.DisableUnsubscribe();

            String json = header.JsonString();
            Assert.AreEqual("{\"filters\" : {\"subscriptiontrack\" : {\"settings\" : {\"enable\" : \"0\"}}}}", json);
        }

        [TestMethod]
        public void DisableFooter()
        {
            var header = new Header();
            var sendgrid = new Mail(header);

            sendgrid.DisableFooter();

            String json = header.JsonString();
            Assert.AreEqual("{\"filters\" : {\"footer\" : {\"settings\" : {\"enable\" : \"0\"}}}}", json);
        }

        [TestMethod]
        public void DisableGoogleAnalytics()
        {
            var header = new Header();
            var sendgrid = new Mail(header);

            sendgrid.DisableGoogleAnalytics();

            String json = header.JsonString();
            Assert.AreEqual("{\"filters\" : {\"ganalytics\" : {\"settings\" : {\"enable\" : \"0\"}}}}", json);            
        }

        [TestMethod]
        public void DisableTemplate()
        {
            var header = new Header();
            var sendgrid = new Mail(header);

            sendgrid.DisableTemplate();

            String json = header.JsonString();
            Assert.AreEqual("{\"filters\" : {\"template\" : {\"settings\" : {\"enable\" : \"0\"}}}}", json);  
        }

        [TestMethod]
        public void DisableBcc()
        {
            var header = new Header();
            var sendgrid = new Mail(header);

            sendgrid.DisableBcc();

            String json = header.JsonString();
            Assert.AreEqual("{\"filters\" : {\"bcc\" : {\"settings\" : {\"enable\" : \"0\"}}}}", json);  
        }

        [TestMethod]
        public void DisableBypassListManagement()
        {
            var header = new Header();
            var sendgrid = new Mail(header);

            sendgrid.DisableBypassListManagement();

            String json = header.JsonString();
            Assert.AreEqual("{\"filters\" : {\"bypass_list_management\" : {\"settings\" : {\"enable\" : \"0\"}}}}", json);
        }

        [TestMethod]
        public void EnableGravatar()
        {
            var header = new Header();
            var sendgrid = new Mail(header);

            sendgrid.EnableGravatar();

            String json = header.JsonString();
            Assert.AreEqual("{\"filters\" : {\"gravatar\" : {\"settings\" : {\"enable\" : \"1\"}}}}", json);
        }

        [TestMethod]
        public void EnableOpenTracking()
        {
            var header = new Header();
            var sendgrid = new Mail(header);

            sendgrid.EnableOpenTracking();

            String json = header.JsonString();
            Assert.AreEqual("{\"filters\" : {\"opentrack\" : {\"settings\" : {\"enable\" : \"1\"}}}}", json);
        }

        [TestMethod]
        public void EnableClickTracking()
        {
            var header = new Header();
            var sendgrid = new Mail(header);
            bool includePlainText = true;
            sendgrid.EnableClickTracking(includePlainText);

            String json = header.JsonString();
            Assert.AreEqual("{\"filters\" : {\"clicktrack\" : {\"settings\" : {\"enable\" : \"1\",\"enable_text\" : \"1\"}}}}", json);
        }

        [TestMethod]
        public void EnableSpamCheck()
        {
            var header = new Header();
            var sendgrid = new Mail(header);

            var score = 5;
            var url = "http://www.example.com";
            sendgrid.EnableSpamCheck(score, url);

            String json = header.JsonString();
            Assert.AreEqual("{\"filters\" : {\"spamcheck\" : {\"settings\" : {\"enable\" : \"1\",\"maxscore\" : \"5\",\"url\" : \"http:\\/\\/www.example.com\"}}}}", json);
        }

        [TestMethod]
        public void EnableUnsubscribe()
        {
            var header = new Header();
            var sendgrid = new Mail(header);

            var text = "<% %>";
            var html = "<% name %>";

            var jsonText = "\"text\\/plain\" : \"" + text + "\"";
            var jsonHtml = "\"text\\/html\" : \"" + html + "\"";

            sendgrid.EnableUnsubscribe(text, html);

            String json = header.JsonString();
            Assert.AreEqual("{\"filters\" : {\"subscriptiontrack\" : {\"settings\" : {\"enable\" : \"1\","+
                jsonText+","+jsonHtml+"}}}}", json);

            header = new Header();
            sendgrid = new Mail(header);

            var replace = "John";
            var jsonReplace = "\"replace\" : \"" + replace + "\"";

            sendgrid.EnableUnsubscribe(replace);

            json = header.JsonString();
            Assert.AreEqual("{\"filters\" : {\"subscriptiontrack\" : {\"settings\" : {\"enable\" : \"1\"," + jsonReplace + "}}}}", json);

            text = "bad";
            html = "<% name %>";
            Assert.Throws<Exception>(() => sendgrid.EnableUnsubscribe(text, html));

            text = "<% %>";
            html = "bad";
            Assert.Throws<Exception>(() =>sendgrid.EnableUnsubscribe(text, html));

        }

        [TestMethod]
        public void EnableFooter()
        {
            var header = new Header();
            var sendgrid = new Mail(header);

            var text = "My Text";
            var html = "<body><p>hello, <% name %></p></body>";
            var escHtml = "<body><p>hello, <% name %><\\/p><\\/body>";

            sendgrid.EnableFooter(text, html);

            String json = header.JsonString();
            Assert.AreEqual("{\"filters\" : {\"footer\" : {\"settings\" : {\"enable\" : \"1\",\"text\\/plain\" : \""+text+"\",\"text\\/html\" : \""+escHtml+"\"}}}}", json);
        }

        [TestMethod]
        public void EnableGoogleAnalytics()
        {
            var header = new Header();
            var sendgrid = new Mail(header);

            var source = "SomeDomain.com";
            var medium = "Email";
            var term = "keyword1, keyword2, keyword3";
            var content = "PG, PG13";
            var campaign = "my_campaign";

            sendgrid.EnableGoogleAnalytics(source, medium, term, content, campaign);

            var jsonSource = "\"utm_source\" : \"SomeDomain.com\"";
            var jsonMedium = "\"utm_medium\" : \"" + medium + "\"";
            var jsonTerm = "\"utm_term\" : \"" + term + "\"";
            var jsonContent = "\"utm_content\" : \"" + content + "\"";
            var jsonCampaign = "\"utm_campaign\" : \"" + campaign + "\"";

            String json = header.JsonString();
            Assert.AreEqual("{\"filters\" : {\"ganalytics\" : {\"settings\" : {\"enable\" : \"1\","+
                            jsonSource+","+jsonMedium+","+jsonTerm+","+jsonContent+","+jsonCampaign+"}}}}", json);
        }

        [TestMethod]
        public void EnableTemplate()
        {
            var header = new Header();
            var sendgrid = new Mail(header);
            var html = "<% hadhdhd %>";

            var escHtml = "<% hadhdhd %>";
            sendgrid.EnableTemplate(html);

            String json = header.JsonString();
            Assert.AreEqual("{\"filters\" : {\"template\" : {\"settings\" : {\"enable\" : \"1\",\"text\\/html\" : \""+escHtml+"\"}}}}", json);

            escHtml = "bad";
            Assert.Throws<Exception>(() => sendgrid.EnableTemplate(escHtml));
        }

        [TestMethod]
        public void EnableBcc()
        {
            var header = new Header();
            var sendgrid = new Mail(header);

            var email = "somebody@someplace.com";
            sendgrid.EnableBcc(email);

            String json = header.JsonString();
            Assert.AreEqual("{\"filters\" : {\"bcc\" : {\"settings\" : {\"enable\" : \"1\",\"email\" : \"" + email + "\"}}}}", json);
        }

        [TestMethod]
        public void EnableBypassListManagement()
        {
            var header = new Header();
            var sendgrid = new Mail(header);

            sendgrid.EnableBypassListManagement();

            String json = header.JsonString();
            Assert.AreEqual("{\"filters\" : {\"bypass_list_management\" : {\"settings\" : {\"enable\" : \"1\"}}}}", json);
        }

        [TestMethod]
        public void CreateMimeMessage()
        {
            var message = Mail.GetInstance();
            var attachment = System.IO.Path.GetTempFileName();
            var text = "this is a test";
            var html = "<b>This<\b> is a better test";
            var headers = new KeyValuePair<String, String>("custom", "header");
            message.AddAttachment(attachment);
            message.Text = text;
            message.Html = html;
            message.AddTo("foo@bar.com");
            message.From = new MailAddress("foo@bar.com");
            message.AddHeaders(new Dictionary<string, string>{{headers.Key, headers.Value}});
            message.EnableGravatar();

            var mime = message.CreateMimeMessage();

            var sr = new StreamReader(mime.AlternateViews[0].ContentStream);
            var result = sr.ReadToEnd();
            Assert.AreEqual(text, result);

            sr = new StreamReader(mime.AlternateViews[1].ContentStream);
            result = sr.ReadToEnd();
            Assert.AreEqual(html, result);

            result = mime.Headers.Get(headers.Key);
            Assert.AreEqual(headers.Value, result);

            result = mime.Headers.Get("X-Smtpapi");
            var expected = "{\"filters\" : {\"gravatar\" : {\"settings\" : {\"enable\" : \"1\"}}}}";
            Assert.AreEqual(expected, result);

            result = mime.Attachments[0].Name;
            Assert.AreEqual(Path.GetFileName(attachment), result);
        }
    }
}
