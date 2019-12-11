using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using QienUrenMVC.Models;

namespace QienUrenMVC.Controllers
{
    public class AttachmentController : Controller
    {
        private readonly IWebHostEnvironment hostingEnvironment;

        //public  Task<IActionResult> UploadAttachment(HoursFormModel updatedhours)
        //{
        //    string attachmentstring = Convert.ToString(updatedhours.Attachment);
        //    if (updatedhours.Attachment != null)
        //    {
        //        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Attachments");
        //        string filePath = Path.Combine(uploadsFolder, attachmentstring);
        //        System.IO.File.Delete(filePath);
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            updatedhours.Attachment.CopyTo(stream);
        //        }
        //    }
        //    return View();
        //}

        //public async Task<ViewResult>
        //    {}
    }
}