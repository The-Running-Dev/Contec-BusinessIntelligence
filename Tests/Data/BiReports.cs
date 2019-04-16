using System;
using System.Collections.Generic;

using Contec.Data.Models;

namespace BI.Web.Tests.Data
{
    public static class BIReports
    {
        public static List<BIReport> Data;

        static BIReports()
        {
            Data = new List<BIReport>();

            for (var i = 1; i < 11; i++)
            {
                Data.Add(New(Guid.NewGuid()));
            }
        }

        public static BIReport New(Guid id)
        {
            return new BIReport()
            {
                Id = id,
                Name = $"Name {id}",
                Description = $"Description {id}",
                EmbedSource = $"EmbedSource {id}",
                CreatedBy = "TestUser",
                CreatedOn = DateTime.Now.ToUniversalTime()
            };
        }
    }
}