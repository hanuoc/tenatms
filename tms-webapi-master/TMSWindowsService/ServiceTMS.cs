using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace TMSWindowsService
{
    public partial class ServiceTMS : ServiceBase
    {
        private System.Timers.Timer timer = null;
        public ServiceTMS()
        {
            InitializeComponent();
        }
        string folderPath = AppDomain.CurrentDomain.BaseDirectory;
        string fullPath = AppDomain.CurrentDomain.BaseDirectory + "\\TMSWindowServiceConfig.txt";
        protected override void OnStart(string[] args)
        {
            Utilities.WriteLogError("Run TMSWindowsService 1st");
            if (!Directory.Exists(folderPath))
            {
                try
                {
                    Directory.CreateDirectory(folderPath);
                    Utilities.WriteLogError("Create folder:" + folderPath);
                }
                catch (Exception ex)
                {
                    Utilities.WriteLogError("Create folder error:" + ex.Message);
                }

            }
            if (!File.Exists(fullPath))
            {
                Utilities.WriteLogError("Create file config !");
                try
                {
                    FileStream fs = File.Create(fullPath);
                    fs.Close();
                }
                catch (Exception ex)
                {
                    Utilities.WriteLogError("Create file config Error:" + ex.Message);
                }
            }
            try
            {
                Utilities.WriteLogError("Set default config...");
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Security=cmcglobal@12345");
                sb.AppendLine("Url=http://tms.cmcglobal.com.vn:43306");
                sb.AppendLine("JobTimeSheet=01:30");
                sb.AppendLine("JobChangeStatus=02:30");
                sb.AppendLine("JobResetEntitleDay=02:00");
                sb.AppendLine("JobUpdateEntitleDay=23:30");
                File.WriteAllText(fullPath, sb.ToString());
            }
            catch (Exception ex)
            {
                Utilities.WriteLogError("Set default config error:" + ex.Message);
            }
            timer = new System.Timers.Timer();
            timer.Interval = 60000;
            timer.Elapsed += timer_Tick;
            timer.Enabled = true;
        }
        private async void timer_Tick(object sender, ElapsedEventArgs args)
        {
            // Xử lý một vài logic ở đây
            try
            {
                if (DateTime.Now.TimeOfDay.ToString(@"hh\:mm") == GetTimeFromFileConfig("JobTimeSheet").Trim())
                {
                    Utilities.WriteLogError("Now:" + DateTime.Now.ToString() + ". Import Time Sheet");
                    var apiTimeSheet = "/api/schedule/auto-import-timesheet";
                    for (int i = 0; i < 5; i++)
                    {
                        Utilities.WriteLogError("Call api time sheet " + (i+1));
                        if (await CallApi(apiTimeSheet))
                            break;
                        Thread.Sleep(1000);
                    }
                }
                if (DateTime.Now.TimeOfDay.ToString(@"hh\:mm") == GetTimeFromFileConfig("JobChangeStatus").Trim())
                {
                    Utilities.WriteLogError("Now:" + DateTime.Now.ToString() + ". Job change status");
                    var apiChangeStatus = "/api/schedule/job-change-status";
                    await CallApiChangeStatus(apiChangeStatus);
                }
                if (DateTime.Now.TimeOfDay.ToString(@"hh\:mm") == GetTimeFromFileConfig("JobResetEntitleDay").Trim())
                {
                    Utilities.WriteLogError("Now:" + DateTime.Now.ToString() + ". Job entitle day");
                    var apiEntitleDay = "/api/schedule/job-entitle-day";
                    await CallApi(apiEntitleDay);
                }
                if (DateTime.Now.TimeOfDay.ToString(@"hh\:mm") == GetTimeFromFileConfig("JobUpdateEntitleDay").Trim())
                {
                    Utilities.WriteLogError("Now:" + DateTime.Now.ToString() + ". Job sub entitle day by request");
                    var apiUpdateEntitleDayByRequest = "/api/schedule/job-entitle-day-by-request";
                    await CallApi(apiUpdateEntitleDayByRequest);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLogError("Error: " + ex.Source + "\n" + ex.Message + ".Inner Ex:" + ex.InnerException.Message);
            }
            Utilities.WriteLogError("Check time each minutes!!!");
        }

        protected override void OnStop()
        {
            // Ghi log lại khi Services đã được stop
            timer.Enabled = true;
            Utilities.WriteLogError("WindowsService TMS has been stop");
        }
        private async Task<bool> CallApiChangeStatus(string api)
        {
            // Create an HttpClient instance
            HttpClient client = new HttpClient();
            // Usage
            string[] security = new string[] { };
            string[] url = new string[] { };
            try
            {
                var config = File.ReadAllLines(fullPath);
                security = config.Where(x => x.Contains("Security=")).FirstOrDefault().Split('=');
                url = config.Where(x => x.Contains("Url=")).FirstOrDefault().Split('=');
            }
            catch (Exception ex)
            {
                Utilities.WriteLogError("Error read file config:" + ex.Message + ". Inner Ex:" + ex.InnerException.Message);
                return false;
            }
            if (url.Count() == 2)
            {
                client.BaseAddress = new Uri(url[1]);
            }
            else
            {
                Utilities.WriteLogError("Error read url from file config");
                return false;
            }
            if (security.Count() == 2)
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(api + "?security=" + CreateMD5(security[1])).Result;
                    Utilities.WriteLogError("Call api : " + api + "?security=" + CreateMD5(security[1]) + "&date=" + DateTime.Now.Date.ToString("dd/MM/yyyy"));
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        Utilities.WriteLogError("Call API Susscessfully ! Content:" + result);
                        return true;
                    }
                    else
                    {
                        Utilities.WriteLogError(response.Content.ReadAsStringAsync().Result.ToString());
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Utilities.WriteLogError("Call API Error:" + ex.Message + ".Inner Ex:" + ex.InnerException.Message);
                    return false;
                }
            }
            else
            {
                Utilities.WriteLogError("Security invalid format !\n" + security);
                return false;
            }
        }
        private async Task<bool> CallApi(string api)
        {
            // Create an HttpClient instance
            HttpClient client = new HttpClient();
            // Usage
            string[] security = new string[] { };
            string[] url = new string[] { };
            try
            {
                var config = File.ReadAllLines(fullPath);
                security = config.Where(x => x.Contains("Security=")).FirstOrDefault().Split('=');
                url = config.Where(x => x.Contains("Url=")).FirstOrDefault().Split('=');
            }
            catch (Exception ex)
            {
                Utilities.WriteLogError("Error read file config:" + ex.Message + ". Inner Ex:" + ex.InnerException.Message);
                return false;
            }
            if (url.Count() == 2)
            {
                client.BaseAddress = new Uri(url[1]);
            }
            else
            {
                Utilities.WriteLogError("Error read url from file config");
                return false;
            }
            if (security.Count() == 2)
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(api + "?security=" + CreateMD5(security[1])+ "&date="+ DateTime.Now.Date.ToString("dd/MM/yyyy")).Result;
                    Utilities.WriteLogError("Call api : "+ api + "?security=" + CreateMD5(security[1]) + "&date=" + DateTime.Now.Date.ToString("dd/MM/yyyy"));
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        Utilities.WriteLogError("Call API Susscessfully ! Content:" + result);
                        return true;
                    }
                    else
                    {
                        Utilities.WriteLogError(response.Content.ReadAsStringAsync().Result.ToString());
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Utilities.WriteLogError("Call API Error:" + ex.Message + ".Inner Ex:" + ex.InnerException.Message);
                    return false;
                }
            }
            else
            {
                Utilities.WriteLogError("Security invalid format !\n" + security);
                return false;
            }
        }
        private string GetTimeFromFileConfig(string key)
        {
            try
            {
                var config = File.ReadAllLines(fullPath);
                var lineConfig = config.Where(x => x.Contains(key + "=")).FirstOrDefault().Split('=');
                if (lineConfig.Count() == 2)
                {
                    return lineConfig[1];
                }
                else
                {
                    Utilities.WriteLogError("Get time by file config error: Line config invalid!");
                    return "";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLogError("Get time by file config error:" + ex.Message);
                return "";
            }
        }
        private static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
