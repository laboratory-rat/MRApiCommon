using System;

namespace MRApiCommon.Infrastructure.Attr
{
    /// <summary>
    /// Simplify target collection for entity
    /// </summary>
    public class CollectionAttr : Attribute
    {
        /// <summary>
        /// Collection name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public CollectionAttr(string name)
        {
            Name = name;
        }
    }
}
