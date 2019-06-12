using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MRApiCommon.Infrastructure.Enum;
using MRApiCommon.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace MRApiCommon.Infrastructure.Database
{
    /// <summary>
    /// Common interpretation of Entity
    /// </summary>
    public class MREntity : IMREntity<string>
    {
        /// <summary>
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public MREntityState State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public void GenerateKey()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }
    }
}
