﻿using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BI.Web.Results
{
    public class ChallengeResult : IHttpActionResult
    {
        public string LoginProvider { get; set; }

        public HttpRequestMessage Request { get; set; }

        public ChallengeResult(string loginProvider, ApiController controller)
        {
            LoginProvider = loginProvider;
            Request = controller.Request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            Request.GetOwinContext().Authentication.Challenge(LoginProvider);

            var response =
                new HttpResponseMessage(HttpStatusCode.Unauthorized) {RequestMessage = Request};

            return Task.FromResult(response);
        }
    }
}
