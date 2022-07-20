using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VCMS.MVC4.Web.Models
{
    public class PostModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Image { get; set; }

        public string DateCreated { get; set; }
    }

    public class PostFormModel
    {
        public PostFormModel()
        {
            PostModels = new List<PostModel>();
        }
        public ICollection<PostModel> PostModels { get; set; }
    }
}