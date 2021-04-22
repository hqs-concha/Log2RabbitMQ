
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Dashboard.Services
{
    public class MongoService<T> where T : BaseModel
    {
        protected readonly IMongoCollection<T> Collection;

        public MongoService(IOptions<DatabaseSettings> options)
        {
            var config = options.Value;
            var client = new MongoClient(config.ConnectionString);
            var database = client.GetDatabase(config.DatabaseName);
            var tableName = ((TableAttribute)typeof(T).GetCustomAttributes(typeof(TableAttribute), true)[0]).Name;
            Collection = database.GetCollection<T>(tableName);
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        public List<T> Get()
        {
            return Collection.Find(T => !T.IsDelete).ToList();
        }

        /// <summary>
        /// 获取单个
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get(string id)
        {
            return Collection.Find<T>(T => T.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        public T Create(T T)
        {
            Collection.InsertOne(T);
            return T;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="TIn"></param>
        public void Update(string id, T TIn)
        {
            Collection.ReplaceOne(T => T.Id == id, TIn);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="TIn"></param>
        public void Remove(T TIn)
        {
            Collection.DeleteOne(T => T.Id == TIn.Id);
        }

        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="id"></param>
        public void Remove(string id)
        {
            Collection.DeleteOne(T => T.Id == id);
        }
    }
}
