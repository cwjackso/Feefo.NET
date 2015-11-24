using System;
using System.Collections.Generic;
using System.Linq;
using Feefo.Requests;
using Feefo.Responses;
using Moq;
using NUnit.Framework;
using Mode = Feefo.Responses.Mode;

namespace Feefo.Tests
{
    [TestFixture]
    public class FeefoClientTests
    {
        private StubHttpMessageHandler _httpMessageHandler;
        private FeefoClient _client;
        private FeefoClientResponse _result;

        [TestFixtureSetUp]
        public void GivenAFeefoClientWithAFakeClientHandler()
        {
            var baseUri = "http://example.kevsoft.net/feefo.jsp";
            var feefoSettings = new FeefoSettings(new Uri(baseUri), "www.examplesupplier.com");

            _httpMessageHandler = new StubHttpMessageHandler(
                new Uri(baseUri),
                new ResourceHelper().GetStringResource("Feefo.Tests.FeefoRssFeed.json"));
            _client = new FeefoClient(_httpMessageHandler, Mock.Of<IQueryStringFactory>(), feefoSettings);
        }

        [SetUp]
        public void WhenGettingFeedback()
        {
            _result = _client.GetFeedbackAsync(new FeedbackRequest()).Result;
        }

        [Test]
        public void ThenTheSummaryIsCorrect()
        {
            Assert.That(_result.FeedbackList.Summary.Average, Is.EqualTo(85));
            Assert.That(_result.FeedbackList.Summary.Best, Is.EqualTo(100));
            Assert.That(_result.FeedbackList.Summary.Count, Is.EqualTo(3863));
            Assert.That(_result.FeedbackList.Summary.FeedGeneration, Is.EqualTo("Tue Sep 15 22:20:39 BST 2015"));
            Assert.That(_result.FeedbackList.Summary.Limit, Is.EqualTo(20));
            Assert.That(_result.FeedbackList.Summary.Mode, Is.EqualTo(Mode.Service));
            Assert.That(_result.FeedbackList.Summary.ProductBad, Is.EqualTo(323));
            Assert.That(_result.FeedbackList.Summary.ProductExcellent, Is.EqualTo(1009));
            Assert.That(_result.FeedbackList.Summary.ProductGood, Is.EqualTo(1233));
            Assert.That(_result.FeedbackList.Summary.ProductPoor, Is.EqualTo(308));
            Assert.That(_result.FeedbackList.Summary.ServiceBad, Is.EqualTo(426));
            Assert.That(_result.FeedbackList.Summary.ServiceExcellent, Is.EqualTo(5798));
            Assert.That(_result.FeedbackList.Summary.ServiceGood, Is.EqualTo(1084));
            Assert.That(_result.FeedbackList.Summary.ServicePoor, Is.EqualTo(203));
            Assert.That(_result.FeedbackList.Summary.Start, Is.EqualTo(1));
            Assert.That(_result.FeedbackList.Summary.SupplierLogo,
                Is.EqualTo("http://www.feefo.com/feefo/getnonpublicfile.jsp?vendorimage=13&vendorimageext=file.png"));
            Assert.That(_result.FeedbackList.Summary.Title, Is.EqualTo("Example Supplier Ltd."));
            Assert.That(_result.FeedbackList.Summary.TotalProductCount, Is.EqualTo(2890));
            Assert.That(_result.FeedbackList.Summary.TotalResponses, Is.EqualTo(3871));
            Assert.That(_result.FeedbackList.Summary.TotalServiceCount, Is.EqualTo(3866));
            Assert.That(_result.FeedbackList.Summary.VendorLogon, Is.EqualTo("www.examplesupplier.com"));
            Assert.That(_result.FeedbackList.Summary.VendorRef, Is.Not.Null);
            Assert.That(_result.FeedbackList.Summary.Worst, Is.EqualTo(0));
        }
        
        [Test]
        public void ThenTheResultContainsTheCorrectAmountOfFeedback()
        {
            var expectedCount = GetExpectedFeedback().Count;

            Assert.That(_result.FeedbackList.Feedback.Count, Is.EqualTo(expectedCount));
        }

        [Test]
        public void ThenTheFeedbackIsCorrect()
        {
            var expectedFeedback = GetExpectedFeedback();

            foreach(var feedback in _result.FeedbackList.Feedback)
            {
                var expected = expectedFeedback.First(x => x.FeedbackId == feedback.FeedbackId);

                Assert.That(feedback.Count, Is.EqualTo(expected.Count));
                Assert.That(feedback.CustomerComment, Is.EqualTo(expected.CustomerComment));
                Assert.That(feedback.Date, Is.EqualTo(expected.Date));
                Assert.That(feedback.Description, Is.EqualTo(expected.Description));
                Assert.That(feedback.FacebookShareLink, Is.EqualTo(expected.FacebookShareLink));
                Assert.That(feedback.DateTime, Is.EqualTo(expected.DateTime));
                Assert.That(feedback.HReviewRating, Is.EqualTo(expected.HReviewRating));
                Assert.That(feedback.Link, Is.EqualTo(expected.Link));
                Assert.That(feedback.ProductCode, Is.EqualTo(expected.ProductCode));
                Assert.That(feedback.ProductRating, Is.EqualTo(expected.ProductRating));
                Assert.That(feedback.ReadmMoreUrl, Is.EqualTo(expected.ReadmMoreUrl));
                Assert.That(feedback.ServiceRating, Is.EqualTo(expected.ServiceRating));
                Assert.That(feedback.ShortCustomerComment, Is.EqualTo(expected.ShortCustomerComment));
            }
        }

        [TestFixtureTearDown]
        public void Kill()
        {
            _client.Dispose();
        }

        public List<Feedback> GetExpectedFeedback()
        {
            return new List<Feedback>()
            {
                new Feedback
                {
                    Count = 1,
                    CustomerComment = "Service rating : Really good service<br/>Product : Good camera for the price.",
                    Date = "56 minutes ago",
                    Description = "Konica Minolta Z20",
                    FacebookShareLink =
                        "http://www.facebook.com/sharer.php?u=http%3A%2F%2Fwww.feefo.com%2FGB%2Fen%2Freviews%2FExample-Supplier-Ltd%2F%3Fid%3D13%26servicefeedbackid%3D7445488",
                    FeedbackId = 7445488,
                    DateTime = new DateTime(2015, 09, 10, 11, 25, 58),
                    HReviewRating = Rating.Good,
                    Link =
                        "http://www.examplesupplier.com/product_info.php?products_id=1333OSCommerceOSCommerceOSCommerce",
                    ProductCode = 133,
                    ProductRating = Rating.Good,
                    ReadmMoreUrl =
                        "http://www.feefo.com/GB/en/reviews/Example-Supplier-Ltd/?id=13&servicefeedbackid=7445488",
                    ServiceRating = Rating.Excellent,
                    ShortCustomerComment =
                        "Service rating : Really good service<br/>Product : Good camera for the price."
                },
                new Feedback
                {
                    Count = 2,
                    CustomerComment =
                        "Service rating : This is a test service reviewwwwwwww0000000000000000000001<br/>Product : This is a test product reviewwwwwwww0000000000000000000001",
                    Date = "56 minutes ago",
                    Description = "this is a test",
                    FacebookShareLink =
                        "http://www.facebook.com/sharer.php?u=http%3A%2F%2Fwww.feefo.com%2FGB%2Fen%2Freviews%2FExample-Supplier-Ltd%2F%3Fid%3D13%26servicefeedbackid%3D7445479",
                    FeedbackId = 7445479,
                    /*FurtherCommentsThread = new FurtherCommentsThread()
                    {
                        Posts = new List<Post>()
                        {
                            new Post
                            {
                                Date = new DateTime(2015, 09, 08),
                                VendorComment = "This is a reply comment through the UI.<br><br>Adrian"
                            },
                            new Post
                            {
                                CustomerComment = "this is a re-rated review - PRODUCT",
                                Date = new DateTime(2015, 09, 08),
                                ProductRating = Rating.Excellent,
                                ServiceRating = Rating.Excellent
                            }
                        }
                    },*/
                    DateTime = new DateTime(2015, 09, 08, 14, 57, 50),
                    Link = "http://www.examplesupplier.com/product_info.php?products_id=Comment013OSCommerceOSCommerce",
                    ProductCode = "Comment01",
                    ProductLatest = Rating.Excellent,
                    ProductRating = Rating.Withdrawn,
                    ReadmMoreUrl =
                        "http://www.feefo.com/GB/en/reviews/Example-Supplier-Ltd/?id=13&servicefeedbackid=7445479",
                    ServiceLatest = Rating.Excellent,
                    ServiceRating = Rating.Withdrawn,
                    ShortCustomerComment =
                        "Service rating : This is a test service reviewwwwwwww0000000000000000000001<br/>Product : This is a test product reviewwwwwwww0000000000000000000001",
                    ShortVendorComment = "This is a reply comment through the UI.<br/><br/>Adrian",
                    VendorComment = "This is a reply comment through the UI.<br/><br/>Adrian"
                },
                new Feedback
                {
                    Count = 3,
                    CustomerComment =
                        "Service rating : I had an excellent experience<br/>Product : Great! It's the best thing ever,",
                    Date = "56 minutes ago",
                    Description = "Falcon Mach V",
                    FacebookShareLink =
                        "http://www.facebook.com/sharer.php?u=http%3A%2F%2Fwww.feefo.com%2FGB%2Fen%2Freviews%2FExample-Supplier-Ltd%2F%3Fid%3D13%26servicefeedbackid%3D7445404",
                    FeedbackId = 7445404,
                    /*FurtherCommentsThread = new FurtherCommentsThread
                    {
                        Posts = new List<Post>()
                        {
                            new Post
                            {
                                Date = new DateTime(2015, 07, 24),
                                VendorComment = "Product :test<br><br>Adrian"
                            },
                            new Post
                            {
                                Date = new DateTime(2015, 07, 27),
                                VendorComment = "this is a test comment<br><br>Adrian"
                            },
                            new Post
                            {
                                Date = new DateTime(2015, 07, 27),
                                VendorComment = "Product :this is a test for the second time.<br><br>Adrian"
                            }
                        }
                    },*/
                    DateTime = new DateTime(2015, 07, 08, 21, 57, 22),
                    HReviewRating = Rating.Excellent,
                    Link = "http://www.examplesupplier.com/product_info.php?products_id=1323OSCommerceOSCommerce",
                    ProductCode = 132,
                    ProductRating = Rating.Excellent,
                    ReadmMoreUrl =
                        "http://www.feefo.com/GB/en/reviews/Example-Supplier-Ltd/?id=13&servicefeedbackid=7445404",
                    ServiceRating = Rating.Excellent,
                    ShortCustomerComment =
                        "Service rating : I had an excellent experience<br/>Product : Great! It's the best thing ever,",
                    ShortVendorComment =
                        "test<br/><br/>Adrian<br/><br/>this is a test comment<br/><br/>Adrian<br/><br/>this is a test for the second time.<br/><br/>Adrian",
                    VendorComment =
                        "test<br/><br/>Adrian<br/><br/>this is a test comment<br/><br/>Adrian<br/><br/>this is a test for the second time.<br/><br/>Adrian"
                },
                new Feedback
                {
                    Count = 4,
                    CustomerComment = "As Above",
                    Date = "56 minutes ago",
                    Description = "Test",
                    FacebookShareLink =
                        "http://www.facebook.com/sharer.php?u=http%3A%2F%2Fwww.feefo.com%2FGB%2Fen%2Freviews%2FExample-Supplier-Ltd%2F%3Fid%3D13%26servicefeedbackid%3D7445167",
                    FeedbackId = 7445167,
                    /*FurtherCommentsThread = new FurtherCommentsThread
                    {
                        Posts = new List<Post>
                        {
                            new Post
                            {
                                Date = new DateTime(2015, 04, 17),
                                VendorComment = "Test<br><br>Adrian"
                            }
                        }
                    },*/
                    DateTime = new DateTime(2015, 04, 17, 09, 52, 13),
                    HReviewRating = Rating.Excellent,
                    Link = "http://www.examplesupplier.com/product_info.php?products_id=Test3OSCommerceOSCommerce",
                    ProductCode = "Test",
                    ProductRating = Rating.Excellent,
                    ReadmMoreUrl =
                        "http://www.feefo.com/GB/en/reviews/Example-Supplier-Ltd/?id=13&servicefeedbackid=7445167",
                    ServiceRating = Rating.Excellent,
                    ShortCustomerComment = "As Above",
                    ShortVendorComment = "Test<br/><br/>Adrian",
                    VendorComment = "Test<br/><br/>Adrian"
                }
            };
        }
    }
}
