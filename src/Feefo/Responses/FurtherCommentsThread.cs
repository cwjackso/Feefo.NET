using System;
using System.Collections.Generic;
using Feefo.JsonConverters;
using Newtonsoft.Json;

namespace Feefo.Responses
{
    
    public class FurtherCommentsThread
    {
        /// <summary>
        /// Contains a customer or vendor responses.
        /// </summary>
        [JsonProperty(PropertyName = "POST")]
        [JsonConverter(typeof(SingleValueArrayConverter<Post>))]
        public List<Post> Posts { get; set; }
    }
}