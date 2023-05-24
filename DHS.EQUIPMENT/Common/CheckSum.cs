using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHS.EQUIPMENT
{
    class CheckSum
    {
        public string GetHexChecksum(string cmd, string param)
        {
            int sum = 0;
            foreach (char c in cmd)
            {
                sum += (int)c;
            }
            foreach(char c in param)
            {
                sum += (int)c;
            }
            string hexValue = sum.ToString("X");
            if (hexValue.Length == 3) return hexValue.Substring(1, 2);
            else return hexValue.Substring(0, 2);
        }
        public string CheckSum_Cal(string strData)
        {
            byte checksum = 0x00;
            byte[] aa = new byte[strData.Length];

            for (int i = 0; i < strData.Length; i++)
                aa[i] = (byte)strData[i];

            for (int i = 0; i < strData.Length; i++)
                checksum += (byte)strData[i];

            checksum = (byte)(checksum & 0x0F);   // 일반 checksum은 0~15까지. ASCII 값

            if (checksum >= 0x00 && checksum < 0x0A)
                checksum += 0x30;
            else //if (checksum >= 0x0A && checksum < 0x10)
                checksum += 0x57; // 소문자

            char kwon = Convert.ToChar(checksum);
            return kwon.ToString();
        }
    }
}
