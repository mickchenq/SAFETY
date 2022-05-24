using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Web;
using System.Threading.Tasks;
using SAFETYService;
using SAFETYModel;

namespace SAFETYService
{
    public class FileService
    {
        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <param name="sFilePath">上傳固定的主目錄</param>
        /// <param name="sSaveDir">各別的檔案子目錄</param>
        /// <param name="sFileTypeLimit">檔案格式</param>
        /// <returns></returns>
        public async Task<UploadResult> UploadFile(string sFilePath, string sSaveDir, string sFileTypeLimit, IFormFile file)
        {             
            UploadResult result = new UploadResult();
            result.Success = true;
            result.FilePath = sSaveDir;            
            sSaveDir = sFilePath +"\\" + sSaveDir;           
            if (file != null && file.Length > 0)
            {
                try
                {
                    string sFileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string sFileExtension = Path.GetExtension(file.FileName);
                    //檢查類型
                    if (sFileTypeLimit.Trim() != "")
                    {
                        if (sFileTypeLimit.ToLower().IndexOf("|" + sFileExtension.ToLower() + "|") < 0) //類型不存在
                        {
                            result.Success = false;
                            result.Message = "上傳檔案類型不正確";
                            return result;
                        }
                    }

                    string sSavePath = Path.Combine(sSaveDir, sFileName + sFileExtension);
                    if (!Directory.Exists( sSaveDir))
                    {
                        Directory.CreateDirectory(sSaveDir);
                    }
                    if (File.Exists(sSavePath))
                    {
                        //重複的檔名加 _n  
                        int n = 1;
                        while (File.Exists(sSaveDir + "\\" + sFileName + "_" + n.ToString() + sFileExtension))
                        {
                            n++;
                        }
                        sFileName = sFileName + "_" + n.ToString();
                    }
                  
                    using (Stream fileStream = new FileStream(Path.Combine(sSaveDir, sFileName + sFileExtension), FileMode.Create, FileAccess.Write))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    //檔名+副檔名
                    result.FileName = sFileName + sFileExtension;                    
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.Message= e.Message.Replace("'", "").Replace("\r\n", "");
                }
            }
            return result;
        }

        /// <summary>
        /// 刪除實體檔案
        /// </summary>
        /// <param name="sFilePath">欲刪除的檔案(包含完整目錄)</param>
        /// <returns></returns>
        public UploadResult DeleteFile(string sFullFileName)
        {
            UploadResult result = new UploadResult();
            result.Success = true;            
            try
            {
                File.Delete(sFullFileName);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message.Replace("'", "").Replace("\r\n", "");
            }
            return result;
        }

            //end class
        }
}
