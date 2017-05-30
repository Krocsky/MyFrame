using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
    /// <summary>
    /// 上传控件参数
    /// </summary>
    public class UploadOptions
    {
        public int MaxFileCount { get; set; }
        public int MaxFileSize { get; set; }
        public string FileUploaded { get; set; }
        public string FileUploadError { get; set; }
        public string FileRemoved { get; set; }
        public string AllowedFileExtensions { get; set; }

        public UploadOptions SetMaxFileCount(int maxFileCount)
        {
            this.MaxFileCount = maxFileCount;
            return this;
        }

        public UploadOptions SetMaxFileSize(int maxFileSize)
        {
            this.MaxFileSize = maxFileSize;
            return this;
        }

        public UploadOptions SetOnFileUploadedCallBack(string fileUploaded)
        {
            this.FileUploaded = fileUploaded;
            return this;
        }

        public UploadOptions SetOnFileUploadErrorCallBack(string fileUploadError)
        {
            this.FileUploadError = fileUploadError;
            return this;
        }

        public UploadOptions SetOnFileRemovedCallBack(string fileRemoved)
        {
            this.FileRemoved = fileRemoved;
            return this;
        }
    }

    public enum AllowedFileType
    {
        Image,
        Html,
        Text,
        Video,
        Audio,
        Flash,
        Object
    }
}
