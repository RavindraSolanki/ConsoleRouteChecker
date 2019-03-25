using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using Flurl.Http.Testing;

namespace RouteChecker.ConsoleAppTests
{
    [Binding]
    public class RoadCheckerStepDefinition : IDisposable
    {
        private HttpTest _httpMocker;
        private string _roadId;
        private string _consoleOutput;
        private int _lastExitCode;

        private const string SuccessfulRoadStatusResponse = @"
[
{
""$type"": ""Tfl.Api.Presentation.Entities.RoadCorridor,Tfl.Api.Presentation.Entities"",
""id"": ""a2"",
""displayName"": ""A2"",
""statusSeverity"": ""Good"",
""statusSeverityDescription"": ""No Exceptional Delays"",
""bounds"": ""[[-0.0857,51.44091],[0.17118,51.49438]]"",
""envelope"": ""[[-0.0857,51.44091],[-0.0857, 51.49438],[0.17118,51.49438],[0.17118,51.44091],[-0.0857,51.44091]]"",
""url"": ""/Road/a2""
}
]";

        private const string NotFoundRoadStatusResponse = @"
{
""$type"": ""Tfl.Api.Presentation.Entities.ApiError,Tfl.Api.Presentation.Entities"",
""timestampUtc"": ""2017-11-21T14:37:39.7206118Z"",
""exceptionType"": ""EntityNotFoundException"",
""httpStatusCode"": 404,
""httpStatus"": ""NotFound"",
""relativeUri"": ""/Road/A233"",
""message"": ""The following road id is not recognised: A233""
}";

        public RoadCheckerStepDefinition()
        {
            _httpMocker = new HttpTest();
        }

        public void Dispose()
        {
            _httpMocker.Dispose();
        }


        [Given(@"a valid road ID is specified")]
        public void GivenAValidRoadIDIsSpecified()
        {
            _roadId = "A2";
            _httpMocker.RespondWith(SuccessfulRoadStatusResponse, 200);

        }
        
        [Given(@"an invalid road ID is specified")]
        public void GivenAnInvalidRoadIDIsSpecified()
        {
            _roadId = "A223";
            _httpMocker.RespondWith(NotFoundRoadStatusResponse, 404);
        }
        
        [When(@"the client is run")]
        public void WhenTheClientIsRun()
        {
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                _lastExitCode = ConsoleApp.Program.Main(new[] { _roadId });
                _consoleOutput = writer.ToString();
            }
        }
        
        [Then(@"the road ‘displayName’ should be displayed")]
        public void ThenTheRoadDisplayNameShouldBeDisplayed()
        {
            var match = Regex.Match(_consoleOutput, "The status of the ([a-zA-Z0-9]+) is as follows");
            var displayName = match.Value;
            Assert.IsFalse(string.IsNullOrWhiteSpace(displayName));
        }
        
        [Then(@"the road ‘statusSeverity’ should be displayed as ‘Road Status’")]
        public void ThenTheRoadStatusSeverityShouldBeDisplayedAsRoadStatus()
        {
            var match = Regex.Match(_consoleOutput, "Road Status is ([a-zA-Z0-9]+)");
            var statusSeverity = match.Value;
            Assert.IsFalse(string.IsNullOrWhiteSpace(statusSeverity));
        }
        
        [Then(@"the road ‘statusSeverityDescription’ should be displayed as ‘Road Status Description’")]
        public void ThenTheRoadStatusSeverityDescriptionShouldBeDisplayedAsRoadStatusDescription()
        {
            var match = Regex.Match(_consoleOutput, "Road Status Description is ([a-zA-Z0-9 ]+)");
            var statusSeverityDescription = match.Value;
            Assert.IsFalse(string.IsNullOrWhiteSpace(statusSeverityDescription));
        }
        
        [Then(@"the application should return an informative error")]
        public void ThenTheApplicationShouldReturnAnInformativeError()
        {
            var match = Regex.Match(_consoleOutput, "([a-zA-Z0-9]+) is not a valid road");
            var informativeError = match.Value;
            Assert.IsFalse(string.IsNullOrWhiteSpace(informativeError));
        }
        
        [Then(@"the application should exit with a non-zero System Error code")]
        public void ThenTheApplicationShouldExitWithANon_ZeroSystemErrorCode()
        {
            Assert.AreEqual(1, _lastExitCode);
        }
    }
}
