using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QienUrenMVC.Models;

namespace QienUrenMVC.Controllers
{
    public class AttachmentController : Controller
    {
        public IActionResult Index(HoursFormModel updatedhours)
        {
            if (updatedhours.Attachment != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Images/ProfileImages");
                string filePath = Path.Combine(uploadsFolder, updatedhours.ImageProfileString);
                uniqueFilename = updatedAccount.ImageProfileString;
                System.IO.File.Delete(filePath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    updatedAccount.ProfileImage.CopyTo(stream);
                }
            }
            return View();
        }
    }
}