using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ImageUpload.Models;

namespace ImageUpload.Controllers
{
    public class ImageController : ApiController
    {
        [HttpPost]
        [Route("api/UploadImage")]
        public HttpResponseMessage UploadImage()
        {
            string imageName = null;
            var httpRequest = HttpContext.Current.Request;

            //Upload Image
            var postedFile = httpRequest.Files["Image"];

            //Create custom filename
            imageName = new string(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
            var filePath = HttpContext.Current.Server.MapPath("~/Images/" + imageName);
            postedFile.SaveAs(filePath);

            //Save to DB
            using (ImageLibraryEntities db = new ImageLibraryEntities())
            {
                Image image = new Image()
                {
                    ImageCaption = httpRequest["ImageCaption"],
                    ImageName = imageName
                };
                db.Images.Add(image);
                db.SaveChanges();
            }
            return Request.CreateResponse(HttpStatusCode.Created);
        }
    }
}

