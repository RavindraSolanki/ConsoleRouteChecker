using System.Collections.Generic;
using System.Threading.Tasks;
using System.Configuration;
using System.Linq;
using Flurl;
using Flurl.Http;

namespace RouteChecker.RoadServices
{
    public class RoadStatusService
    {
        private readonly string _ftlApiRoot;
        private readonly string _tflApiApplicationId;
        private readonly string _tflApiApplicationKeys;

        public RoadStatusService()
        {
            _ftlApiRoot = ConfigurationManager.AppSettings["TflApiRoot"];
            _tflApiApplicationId = ConfigurationManager.AppSettings["TflApiApplicationId"];
            _tflApiApplicationKeys = ConfigurationManager.AppSettings["TflApiApplicationKeys"];

        }

        public async Task<RoadStatus> GetStatus(string roadId)
        {
            try
            {
                var roadStatusArray = await _ftlApiRoot
                    .AppendPathSegments("road", roadId)
                    .SetQueryParam("app_id", _tflApiApplicationId)
                    .SetQueryParam("app_key", _tflApiApplicationKeys)
                    .GetJsonAsync<IEnumerable<RoadStatus>>();

                return roadStatusArray.Single();

            } catch (FlurlHttpException ex)
            {
                if (ex.Call.HttpStatus == System.Net.HttpStatusCode.NotFound)
                {
                    throw new RoadNotFoundException(ex.Call.Response.ReasonPhrase);
                }
                else
                {
                    throw ex;
                }
            }
        }
    }
}
