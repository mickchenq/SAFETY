using System;

namespace SAFETYService
{
    public class CommonService
    {
        /// <summary>
        /// 英數亂數
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string RandomString(int length)
        {
            //少了英文的IO和數字10，要避免使用者判斷問題時會使用到
            string allChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            //string allChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";//26個英文字母
            char[] chars = new char[length];
            Random rd = new Random(Guid.NewGuid().GetHashCode());
            rd.Next();
            for (int i = 0; i < length; i++)
            {
                chars[i] = allChars[rd.Next(0, allChars.Length)];
            }

            return new string(chars);
        }



        //end class
    }
}
