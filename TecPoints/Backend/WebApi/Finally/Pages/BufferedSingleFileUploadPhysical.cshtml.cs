using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using DemoWebApi.Utilities;
using Microsoft.AspNetCore.Hosting;
namespace DemoWebApi.Pages
{
    public class BufferedSingleFileUploadPhysicalModel : PageModel
    {
        private readonly long _fileSizeLimit;
        private readonly string[] _permittedExtensions = { ".zip" };
        private readonly string _targetFilePath;
        public BufferedSingleFileUploadPhysicalModel(IConfiguration config, IWebHostEnvironment hostingEnvironment)
        {
            _fileSizeLimit = config.GetValue<long>("FileSizeLimit");
            _targetFilePath = hostingEnvironment.ContentRootPath +"/" + config.GetValue<string>("StoredFilesPath");
        }
        [BindProperty]
        public BufferedSingleFileUploadPhysical FileUpload { get; set; }
        public string Result { get; private set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostUploadAsync()
        {
            if (!ModelState.IsValid)
            {
                Result = "Please correct the form.";
                return Page();
            }
            var formFileContent =
                await FileHelpers.ProcessFormFile<BufferedSingleFileUploadPhysical>(
                    FileUpload.FormFile, ModelState, _permittedExtensions,
                    _fileSizeLimit);
            if (!ModelState.IsValid)
            {
                Result = "Please correct the form.";
                return Page();
            }
            // For the file name of the uploaded file stored
            // server-side, use Path.GetRandomFileName to generate a safe
            // random file name.
            // var trustedFileNameForFileStorage = Path.GetRandomFileName();
            var filePath = Path.Combine(
                _targetFilePath, FileUpload.FormFile.FileName);
            // **WARNING!**
            // In the following example, the file is saved without
            // scanning the file's contents. In most production
            // scenarios, an anti-virus/anti-malware scanner API
            // is used on the file before making the file available
            // for download or for use by other systems. 
            // For more information, see the topic that accompanies 
            // this sample.
            using (var fileStream = System.IO.File.Create(filePath))
            {
                await fileStream.WriteAsync(formFileContent);
                // To work directly with a FormFile, use the following
                // instead:
                //await FileUpload.FormFile.CopyToAsync(fileStream);
            }
            return RedirectToPage("./Index");
        }
    }
    public class BufferedSingleFileUploadPhysical
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }
        [Display(Name = "Note")]
        [StringLength(50, MinimumLength = 0)]
        public string Note { get; set; }
    }
}