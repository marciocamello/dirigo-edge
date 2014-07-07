using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
    public class FileBrowserViewModel
    {
        public List<Folder> FolderListing;
        public List<MediaFile> FileListing;

        public FileBrowserViewModel(string folder)
        {
            FolderListing = new List<Folder>();
            FileListing = new List<MediaFile>();

            const string supportedExtensions = "*.jpg,*.gif,*.png,*.bmp,*.jpe,*.jpeg,*.svg,*.wmf,*.emf,*.xbm,*.ico,*.eps,*.tif,*.tiff,*.pdf,*.doc,*.docx";
            const string imageExtensions = "*.jpg,*.gif,*.png,*.bmp,*.jpe,*.jpeg,*.ico,*.eps,*.tif,*.tiff,*.svg";
            const string documentExtensions = "*.pdf,*.doc,*.docx";

            var directories = Directory.GetDirectories(HttpContext.Current.Server.MapPath(Utils.ContentGlobals.IMAGEUPLOADDIRECTORY));

            // Create list of folders
            foreach (var directory in directories)
            {
                var entry = new Folder();
                entry.Name = directory.Split('\\').Last();
                entry.FileCount = Directory.GetFiles(directory).Count();

                FolderListing.Add(entry);
            }

            // Create list of files for requested folder
            if (!String.IsNullOrEmpty(folder))
            {
                var files = Directory.GetFiles(folder, "*.*", SearchOption.TopDirectoryOnly).Where(s => supportedExtensions.Contains(Path.GetExtension(s).ToLower()));

                foreach (var file in files.Where(file => Path.GetFileName(file) != "ph"))
                {
                    var extension = Path.GetExtension(file);
                    string icon;

                    if (extension != null && imageExtensions.Contains(extension))
                    {
                        icon = "picture-o";
                    }
                    else if (extension != null && documentExtensions.Contains(extension))
                    {
                        icon = "file";
                    }
                    else
                    {
                        icon = "question";
                    }

                    FileListing.Add(new MediaFile
                        {
                            Folder = folder.Split('\\').Last(),
                            Filename = file.Split('\\').Last(),
                            WebPath = Utils.ContentGlobals.IMAGEUPLOADDIRECTORY + folder.Split('\\').Last() + "/" + file.Split('\\').Last(),
                            Icon = icon
                        });
                }
            }
        }
    }

    public class Folder
    {
        public String Name;
        public int FileCount;
    }

    public class MediaFile
    {
        public String Folder { get; set; }
        public String Filename { get; set; }
        public String WebPath { get; set; }
        public String Icon { get; set; }
    } 
}