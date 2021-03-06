﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using SendGrid.SmtpApi;

namespace SendGrid.Tests
{
    [TestClass]
    public class MailMessageSetupTest
    {
		[TestMethod]
        public void TestAddHeaderTo()
        {
            var mock = new Mock<Header>();
		    var sg = new Mail(mock.Object);

			var strings = new string[2] {"eric@sendgrid.com", "tyler@sendgrid.com"};
			sg.Header.SetTo(strings);
			Assert.AreEqual(strings, sg.Header.To, "check the X-Smtpapi to array");
        }


        [TestMethod]
        public void TestAddTo()
        {
            var foo = new Mock<IHeader>();

            var sg = new Mail(foo.Object);
            sg.AddTo("eric@sendgrid.com");
            Assert.AreEqual("eric@sendgrid.com", sg.To.First().ToString(), "Single To Address" );

            sg = new Mail(foo.Object);
            var strings = new String[2];
            strings[0] = "eric@sendgrid.com";
            strings[1] = "tyler@sendgrid.com";
            sg.AddTo(strings);
            Assert.AreEqual("eric@sendgrid.com", sg.To[0].ToString(), "Multiple To addresses, check first one");
            Assert.AreEqual("tyler@sendgrid.com", sg.To[1].ToString(), "Multiple To addresses, check second one");

            sg = new Mail(foo.Object);
            var a = new Dictionary<String, String>();
            a.Add("DisplayName", "Eric");
            var datastruct = new Dictionary<string, IDictionary<string, string>> {{"eric@sendgrid.com", a}};
            sg.AddTo(datastruct);
            Assert.AreEqual("Eric", sg.To.First().DisplayName, "Single address with args");
        }

        [TestMethod]
        public void TestAddCc()
        {
            var foo = new Mock<IHeader>();

            var sg = new Mail(foo.Object);
            sg.AddCc("eric@sendgrid.com");
            Assert.AreEqual("eric@sendgrid.com", sg.Cc.First().ToString(), "Single CC Address");

            sg = new Mail(foo.Object);
            var strings = new String[2];
            strings[0] = "eric@sendgrid.com";
            strings[1] = "tyler@sendgrid.com";
            sg.AddCc(strings);
            Assert.AreEqual("eric@sendgrid.com", sg.Cc[0].ToString(), "Multiple CC addresses, check first one");
            Assert.AreEqual("tyler@sendgrid.com", sg.Cc[1].ToString(), "Multiple CC addresses, check second one");

            sg = new Mail(foo.Object);
            var a = new Dictionary<String, String>();
            a.Add("DisplayName", "Eric");
            var datastruct = new Dictionary<string, IDictionary<string, string>> { { "eric@sendgrid.com", a } };
            sg.AddCc(datastruct);
            Assert.AreEqual("Eric", sg.Cc.First().DisplayName, "Single CC address with args");
        }

        [TestMethod]
        public void TestAddBcc()
        {
            var foo = new Mock<IHeader>();

            var sg = new Mail(foo.Object);
            sg.AddBcc("eric@sendgrid.com");
            Assert.AreEqual("eric@sendgrid.com", sg.Bcc.First().ToString(), "Single Bcc Address");

            sg = new Mail(foo.Object);
            var strings = new String[2];
            strings[0] = "eric@sendgrid.com";
            strings[1] = "tyler@sendgrid.com";
            sg.AddBcc(strings);
            Assert.AreEqual("eric@sendgrid.com", sg.Bcc[0].ToString(), "Multiple addresses, check first one");
            Assert.AreEqual("tyler@sendgrid.com", sg.Bcc[1].ToString(), "Multiple addresses, check second one");

            sg = new Mail(foo.Object);
            var a = new Dictionary<String, String>();
            a.Add("DisplayName", "Eric");
            var datastruct = new Dictionary<string, IDictionary<string, string>> { { "eric@sendgrid.com", a } };
            sg.AddBcc(datastruct);
            Assert.AreEqual("Eric", sg.Bcc.First().DisplayName, "Single address with args");
        }

        [TestMethod]
        public void TestSendGridHeader()
        {
            var foo = new Mock<IHeader>();
            var sg = new Mail(foo.Object);

            sg.Subject = "New Test Subject";
            Assert.AreEqual("New Test Subject", sg.Subject, "Subject set ok");
            sg.Subject = null;
            Assert.AreEqual("New Test Subject", sg.Subject, "null subject does not overide previous subject");
        }

        [TestMethod]
        public void TestAddSubstitution()
        {
            var substitutionStrings = new List<String> {"foo", "bar", "beer"};
            var mock = new Mock<IHeader>();

            var sg = new Mail(mock.Object);
            sg.AddSubstitution("-name-", substitutionStrings);

            mock.Verify(foo => foo.AddSubstitution("-name-", substitutionStrings));
        }

        [TestMethod]
        public void TestAddUniqueIdentifier()
        {

            var kvp = new Dictionary<String, String> { {"foo", "bar"}, {"beer", "good"} };
            var mock = new Mock<IHeader>();

            var sg = new Mail(mock.Object);
            sg.AddUniqueIdentifiers(kvp);

            mock.Verify(foo => foo.AddUniqueArgs(kvp));
        }

        [TestMethod]
        public void TestSetCategory()
        {
            var cat = "foo";
            var mock = new Mock<IHeader>();

            var sg = new Mail(mock.Object);
            sg.SetCategory(cat);

            mock.Verify(foo => foo.SetCategory(cat));
        }

        [TestMethod]
        public void TestGetRcpts()
        {
            var foo = new Mock<IHeader>();
            var sg = new Mail(foo.Object);

            sg.AddTo("eric@sendgrid.com");
            sg.AddCc("tyler@sendgrid.com");
            sg.AddBcc("cj@sendgrid.com");
            sg.AddBcc("foo@sendgrid.com");

            var rcpts = sg.GetRecipients();
            Assert.AreEqual("eric@sendgrid.com", rcpts.First(), "getRecipients check To");
            Assert.AreEqual("tyler@sendgrid.com", rcpts.Skip(1).First(), "getRecipients check Cc");
            Assert.AreEqual("cj@sendgrid.com", rcpts.Skip(2).First(), "getRecipients check Bcc");
            Assert.AreEqual("foo@sendgrid.com", rcpts.Skip(3).First(), "getRecipients check Bcc x2");
        }

        [TestMethod]
        public void TestAddAttachment()
        {
            var foo = new Mock<IHeader>();
            var sg = new Mail(foo.Object);

            //var data = new Attachment("pnunit.framework.dll", MediaTypeNames.Application.Octet);
            sg.AddAttachment("pnunit.framework.dll");
            sg.AddAttachment("pnunit.framework.dll");
            //Assert.AreEqual(data.ContentStream, sg.Attachments.First().ContentStream, "Attach via path");
            //Assert.AreEqual(data.ContentStream, sg.Attachments.Skip(1).First().ContentStream, "Attach via path x2");

            sg = new Mail(foo.Object);
            //sg.AddAttachment(data);
            //sg.AddAttachment(data);
            //Assert.AreEqual(data.ContentStream, sg.Attachments.First().ContentStream, "Attach via attachment");
            //Assert.AreEqual(data.ContentStream, sg.Attachments.Skip(1).First().ContentStream, "Attach via attachment x2");

            sg = new Mail(foo.Object);
            //sg.AddAttachment(data.ContentStream, data.ContentType);
            //sg.AddAttachment(data.ContentStream, data.ContentType);
            //Assert.AreEqual(data.ContentStream, sg.Attachments.First().ContentStream, "Attach via stream");
            //Assert.AreEqual(data.ContentStream, sg.Attachments.Skip(1).First().ContentStream, "Attach via stream x2");
        }
    }
}
