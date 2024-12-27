using iPlayground.Core.Interfaces.Services;
using iPlayground.Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace iPlayground.Core.Services
{
   public class PrinterService : IPrinterService
{
    private readonly IConfiguration _configuration;
    private const string ESC = "\x1B";
    private const string GS = "\x1D";
    private const string INIT = ESC + "@";
    private const string CUT = GS + "V" + "\x41" + "\x00";
    private const string ALIGN_CENTER = ESC + "a" + "\x01";
    private const string ALIGN_LEFT = ESC + "a" + "\x00";
    private const string BOLD_ON = ESC + "E" + "\x01";
    private const string BOLD_OFF = ESC + "E" + "\x00";
    private const string DOUBLE_ON = GS + "!" + "\x11";
    private const string DOUBLE_OFF = GS + "!" + "\x00";

    public PrinterService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task PrintReceiptAsync(Session session, decimal amount, string pn)
    {
            try
            {
                string printerName = pn;

                // Create raw data for printer
                string rawData =
                       INIT +
                       ALIGN_CENTER +
                       DOUBLE_ON +
                       "iPlayground\n" +
                       DOUBLE_OFF +
                       "\n" +
                       ALIGN_LEFT +
                       $"Datum: {DateTime.Now:dd.MM.yyyy HH:mm}\n" +
                       $"Dijete: {session.Child.FirstName} {session.Child.LastName}\n" +
                       $"Roditelj: {session.Child.Parent.Name}\n" +
                       $"Telefon: {session.Child.Parent.Phone}\n" +
                       "--------------------------------\n" +
                       $"Početak: {session.StartTime:HH:mm}\n" +
                       $"Kraj: {DateTime.Now:HH:mm}\n" +
                       $"Trajanje: {(DateTime.Now - session.StartTime).Hours}h {(DateTime.Now - session.StartTime).Minutes}min\n" +
                       "--------------------------------\n" +
                       BOLD_ON +
                       $"UKUPNO: {amount:N2} KM\n" +
                       BOLD_OFF +
                       "\n" +
                       ALIGN_CENTER +
                       "Hvala na posjeti!\n" +
                       "\n" +
                       BOLD_ON +
                       "www.itable.app\n" +
                       BOLD_OFF +
                       "\n" +
                       CUT;

                // Send to printer using RawPrinterHelper
                RawPrinterHelper.SendStringToPrinter(printerName, rawData);
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška prilikom štampanja: {ex.Message}");
            }
        }
        public async Task PrintFiscalReceiptAsync(Session session, decimal amount, string pn)
        {
            // TODO: Dodati fiskalne elemente nakon integracije sa fiskalnom kasom
            await PrintReceiptAsync(session, amount, pn);
        }

        public async Task PrintNonFiscalReceiptAsync(Session session, decimal amount, string pn)
        {
            await PrintReceiptAsync(session, amount, pn);
        }
    }

    
    // RawPrinterHelper.cs
    internal class RawPrinterHelper
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }

        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        public static bool SendStringToPrinter(string szPrinterName, string szString)
        {
            IntPtr hPrinter;
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false;

            di.pDocName = "Receipt Document";
            di.pDataType = "RAW";

            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    if (StartPagePrinter(hPrinter))
                    {
                        byte[] bytes = Encoding.ASCII.GetBytes(szString);
                        int dwCount = bytes.Length;
                        IntPtr pBytes = Marshal.AllocCoTaskMem(dwCount);
                        Marshal.Copy(bytes, 0, pBytes, dwCount);

                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out int dwWritten);
                        EndPagePrinter(hPrinter);
                        Marshal.FreeCoTaskMem(pBytes);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            return bSuccess;
        }
    }


   
}
