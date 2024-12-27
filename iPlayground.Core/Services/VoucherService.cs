using HtmlAgilityPack;
using iPlayground.Core.Interfaces.Repositories;
using iPlayground.Core.Interfaces.Services;
using iPlayground.Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

 namespace iPlayground.Core.Services
{
     public class VoucherService : IVoucherService
    {
         private readonly decimal _hourlyDiscount = 2.0m;
        private readonly string _companyJIB;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly ISessionVoucherRepository _voucherRepository;


        public decimal HourlyDiscountAmount => 2.0m; // Fiksni iznos od 2 KM po satu

        public VoucherService(IConfiguration configuration, IHttpClientFactory httpClientFactory, ISessionVoucherRepository voucherRepository)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient("PoreskaUprava");
            _companyJIB = "4219055180039";
            _voucherRepository = voucherRepository;
        }
        public async Task<VoucherValidation> ValidateVoucherAsync(string qrCode)
        {
            try
            {
                // Prvo provjerimo da li je vaučer već korišten
                var existingVoucher = await _voucherRepository.FindAsync(
                    v => v.QRCode == qrCode || v.FiscalNumber == ExtractFiscalNumber(qrCode));

                if (existingVoucher.Any())
                {
                    return new VoucherValidation
                    {
                        IsValid = false,
                        ErrorMessage = "Ovaj vaučer je već iskorišten!"
                    };
                }

                // Postojeća validacija
                var url = DecodeQRCode(qrCode);
                var response = await _httpClient.GetStringAsync(url);

                var fiscalNumber = ExtractFiscalNumber(response);
                var amount = ExtractAmount(response);
                var jib = ExtractJIB(response);

                return new VoucherValidation
                {
                    FiscalNumber = fiscalNumber,
                    Amount = amount,
                    JIB = jib,
                    IsValid = jib == _companyJIB && amount >= 20.0m,
                    ErrorMessage = jib != _companyJIB ? "Vaučer nije od našeg objekta!" :
                                  amount < 20.0m ? "Iznos na računu mora biti veći od 20 KM!" : null
                };
            }
            catch (Exception ex)
            {
                return new VoucherValidation
                {
                    IsValid = false,
                    ErrorMessage = $"Greška pri validaciji vaučera: {ex.Message}"
                };
            }
        }

           
        public async Task<decimal> ApplyVoucherToSessionAsync(int sessionId, VoucherValidation voucher)
        {
            if (!voucher.IsValid)
                throw new InvalidOperationException("Vaučer nije validan.");

            // Primijeni popust od 2 KM po satu
            // Minimalna cijena je 0 KM
            decimal currentAmount = await GetCurrentSessionAmount(sessionId);
            decimal discount = _hourlyDiscount;

            return Math.Max(0, currentAmount - discount);
        }

        private string DecodeQRCode(string qrCode)
        {
            // Dekodiranje i formiranje URL-a za Poresku upravu
            var baseUrl = "https://suf.poreskaupravars.org/v/?vl=";
            return qrCode.Replace("httpsČ--", "https://")
                        .Replace("-", "/")
                        .Replace("_", "=")
                        .Replace("+", "&")
                        .Replace("*", "+");
        }

        private decimal ExtractAmount(string html)
        {
            // Tražimo ukupan iznos iz HTML-a
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var amountNode = doc.DocumentNode.SelectSingleNode("//span[@id='totalAmountLabel']");
            return decimal.Parse(amountNode.InnerText.Trim(), CultureInfo.InvariantCulture);
        }

        private string ExtractJIB(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var jibNode = doc.DocumentNode.SelectSingleNode("//span[@id='tinLabel']");
            return jibNode.InnerText.Trim();
        }

        private string ExtractFiscalNumber(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var fiscalNode = doc.DocumentNode.SelectSingleNode("//span[@id='invoiceNumberLabel']");
            return fiscalNode.InnerText.Trim();
        }

        private async Task<decimal> GetCurrentSessionAmount(int sessionId)
        {
            // TODO: Implementirati dohvatanje trenutnog iznosa sesije
            throw new NotImplementedException();
        }

        public async Task<List<VoucherValidation>> GetVouchersForSessionAsync(int sessionId)
        {
            // TODO: Implementirati dohvatanje svih vaučera za sesiju
            throw new NotImplementedException();
        }
    }
}
