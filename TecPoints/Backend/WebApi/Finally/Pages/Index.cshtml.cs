using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using DemoWebApi.Models;

namespace DemoWebApi.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IFileProvider _fileProvider;

        public IndexModel(IFileProvider fileProvider)
        {

            _fileProvider = fileProvider;
        }

        public IList<AppFile> DatabaseFiles { get; private set; }
        public IDirectoryContents PhysicalFiles { get; private set; }

        public async Task OnGetAsync()
        {
            //DatabaseFiles = await _context.File.AsNoTracking().ToListAsync();
            PhysicalFiles = _fileProvider.GetDirectoryContents(string.Empty);
        }

        public IActionResult OnGetDownloadPhysical(string fileName)
        {
            var downloadFile = _fileProvider.GetFileInfo(fileName);

            return PhysicalFile(downloadFile.PhysicalPath, MediaTypeNames.Application.Octet, fileName);
        }
    }
}
