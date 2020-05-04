﻿﻿﻿using System.Collections.Generic;

   namespace Solaris.Web.CrewApi.Core.Models.Rabbit.Models
{
    public class PublishOptions
    {
        public string TargetQueue { get; set; }
        public Dictionary<string, object> Headers { get; set; }
        public string Message { get; set; }
    }
}