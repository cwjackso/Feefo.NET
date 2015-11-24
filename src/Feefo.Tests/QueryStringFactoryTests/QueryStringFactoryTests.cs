using System;
using System.Collections.Generic;
using System.Linq;
using Feefo.Requests;
using NUnit.Framework;

namespace Feefo.Tests.QueryStringFactoryTests
{
    [TestFixture]
    public class QueryStringFactoryTests
    {
        private QueryStringFactory _factory;
        private string _logon;
        private IFeefoSettings _settings;
        private string _result;

        [TestFixtureSetUp]
        public void GivenAQueryStringFactory()
        {
            _factory = new QueryStringFactory();
        }

        [SetUp]
        public void WhenCreatingAQueryStringFromALogon()
        {
            _logon = Guid.NewGuid().ToString();
            
            _settings = new FeefoSettings(_logon);

            _result = _factory.Create(_settings, WithFeedbackRequest());
        }

        protected virtual FeedbackRequest WithFeedbackRequest()
        {
            return new FeedbackRequest();
        }

        [Test]
        public void ThenTheQueryStringStartsWithAQuestionMark()
        {
            Assert.That(_result, Is.StringStarting("?"));
        }

        [Test]
        public void ThenTheQueryStringContainsJsonTrue()
        {
            var lookup = GetQueryLookup();

            Assert.That(lookup.ContainsKey("json"), Is.True);
            Assert.That(lookup["json"], Is.EqualTo("true"));
        }

        protected IDictionary<string, string> GetQueryLookup()
        {
            return _result.Substring(1).Split('&')
                .Select(pair => pair.Split('='))
                .ToDictionary(x => x[0], x => x[1]);
        }
    }
}
