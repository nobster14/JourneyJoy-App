using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.ExternalAPI
{
    public abstract class BaseAPI
    {
        #region Protected fields

        protected bool IsEnabled;
        protected string APIKey;

        #endregion

        #region Constructors
        public BaseAPI(string APIKey, bool isEnabled)
        {
            this.APIKey = APIKey;
            this.IsEnabled = isEnabled;
        }
        #endregion

        #region Base methods
        public async Task<RestResponse> MakeGETCall(string url)
        {
            if (!IsEnabled)
                return null;

            var options = new RestClientOptions(url);
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            var response = await client.GetAsync(request);

            return response;
        }
        #endregion
    }
}
