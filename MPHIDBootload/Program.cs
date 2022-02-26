/*
 * SPDX-FileCopyrightText: © 2021 Matthias Keller <mkeller_service@gmx.de>
 *
 * SPDX-License-Identifier: MIT
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.IO;
using IntelHex;
using Lib_mphidflashsharp;
using System.Runtime.InteropServices;

namespace MPHIDBootload
{

   

    class Program
    {
 

        static void Main(string[] args)
        {
            Console.WriteLine("USB-MEM Burn Firmware Update Tool");
            string basepath = Directory.GetCurrentDirectory();
            string hexfilename = "umbfirmware.bin";
            string hexfilepath = hexfilename;

           string tmpOutPath = Path.GetTempPath() + "USB_MEM_Burn_FWUpdate\\";
            //if (Directory.Exists(tmpOutPath))
            //    Directory.Delete(tmpOutPath, true);

            //Directory.CreateDirectory(tmpOutPath);
            //basepath = tmpOutPath;

            if (args.Length == 1)
            {
                if (File.Exists(args[0]))
                    hexfilepath = args[0];
            }
            else
            {
                hexfilepath = basepath +"\\"+ hexfilename;
            }

            //File.Copy(hexfilename, hexfilepath);
            if (!File.Exists(hexfilepath))
            {
                Console.WriteLine("File {0} not exist. Exit");
                return;
            }
            IntelHex.IntelHexFile ihex = new IntelHex.IntelHexFile();
            bool vailidhex=ihex.LoadFromHexFile(hexfilepath);
            if (ihex.FileValid)
            {
                ihex.WriteBinary(basepath + "umbfirmwareread.bin");
            }

            Process.Start(basepath);
            Lib_mphidflashsharp.Bootloader boot = new Lib_mphidflashsharp.Bootloader();
            //byte[] readdata=boot.ReadData(0x1FBF0,2);

            // boot.ReadFlash(@"D:\testboot\read23.bin");
            if (!boot.Connected)
                return;
            boot.EraseDevice();
            boot.WriteFlashFromBin(hexfilepath);
            boot.ReadFlash(basepath + "read_after.bin");
            boot.QueryExtended();
            boot.Disconnect();

        }

    }
}
