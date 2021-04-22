using System;
using System.Collections.Generic;
using System.Linq;
using Dashboard.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Dashboard.Services
{
    public class LogService : MongoService<LogModel>
    {
        public LogService(IOptions<DatabaseSettings> options) : base(options)
        {
        }

        public (List<LogModel> data, long total) Get(SearchLogModel model)
        {
            var filters = FilterDefinition<LogModel>.Empty;
            var appName = Builders<LogModel>.Filter.Eq("AppName", model.AppName);
            filters = Builders<LogModel>.Filter.And(filters, appName);

            if (!string.IsNullOrEmpty(model.Level))
            {
                var level = Builders<LogModel>.Filter.Eq("LogLevel", model.Level);
                filters = Builders<LogModel>.Filter.And(filters, level);
            }

            if (model.ValidFrom.HasValue)
            {
                var validFrom = Builders<LogModel>.Filter.Gte("LogTime", model.ValidFrom);
                filters = Builders<LogModel>.Filter.And(filters, validFrom);
            }

            if (model.ValidTo.HasValue)
            {
                var validTo = Builders<LogModel>.Filter.Lte("LogTime", model.ValidTo);
                filters = Builders<LogModel>.Filter.And(filters, validTo);
            }

            var skip = (model.PageIndex - 1) * 10;
            skip = skip < 0 ? 0 : skip;
            var logs = Collection.Find(filters).SortByDescending(p => p.AddTime).Skip(skip).Limit(10).ToList();
            var count = Collection.Find(filters).CountDocuments();
            return (logs, count);
        }

        public List<string> GetAppNames()
        {
            var document = Collection.Find(p => !p.IsDelete)
                .Project(Builders<LogModel>.Projection.Include("AppName")).ToList();

            var data = document.GroupBy(p => p["AppName"]).Select(p => p.Key.ToString()).ToList();
            return data;
        }
    }
}
