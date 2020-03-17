using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TestGithub
{
    class Program
    {
        static void Main(string[] args)
        {
            string aaa = "";
            string re = "";
            string path = @"E:\FocalSpec\原始数据\J1_L1.txt";
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (StreamReader sr = new StreamReader(fs,Encoding.Default))
            {
                Stopwatch sp = new Stopwatch();
                sp.Start();
                StringBuilder sb = new StringBuilder();
                string txt = "";
                int width = 0;
                while ((txt = sr.ReadLine())!=null)
                {
                   sb.Append(txt.Split(' ')[2]+",");
                    
                    if (Convert.ToDouble(txt.Split(' ')[1])==0.00)
                    {
                        width++;
                    }
                }
                
                string[] a =sb.ToString().Split(',');
                float[] pointsZ = new float[a.Length];
                for (int i = 0; i < a.Length-1; i++)
                {
                    pointsZ[i] = Convert.ToSingle(a[i]);
                    
                }
                int height = pointsZ.Length / width;
                sp.Stop();
                long b = sp.ElapsedMilliseconds;

                try
                {
                    HObject tempImage;
                    int nSize = Marshal.SizeOf(typeof(float));
                    IntPtr ptImage = Marshal.AllocHGlobal(nSize * pointsZ.Length);
                    Marshal.Copy(pointsZ, 0, ptImage, pointsZ.Length);
                    HOperatorSet.GenImage1(out tempImage, "real", width, height,  ptImage);
                    Marshal.FreeHGlobal(ptImage);
                    HOperatorSet.WriteImage(tempImage, "tiff", 0, @"C:\Users\lgche\Desktop\tempImage\0318高度.tiff");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                //string a = sr.ReadToEnd();

                //string[] b = a.Split('\n');
                //float[] zArr = new float[b.Length];
                //for (int i = 0; i < b.Length; i++)
                //{
                //    zArr[i] = Convert.ToSingle(b[i].Split(' ')[2]);
                //}
                //byte[] mybyte = Encoding.UTF8.GetBytes(a);
                //text1.Text = Encoding.UTF8.GetString(mybyte);
            }
        }
    }
}
