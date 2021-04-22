using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Dashboard.Models
{
    public class BaseModel
    {
        [BsonId]        //标记主键
        [BsonRepresentation(BsonType.ObjectId)]     //参数类型  ， 无需赋值
        public string Id { get; set; }

        [BsonElement(nameof(AddTime))]   //指明数据库中字段名为CreateDateTime
        public DateTime AddTime { get; set; }

        [BsonElement(nameof(IsDelete))]
        public bool IsDelete { get; set; }

        public BaseModel()
        {
            AddTime = DateTime.Now;
            IsDelete = false;
        }
    }
}
