//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Threading.Tasks;
//using TMS.Common.ViewModels;

//namespace TMS.Common
//{
//    public class HolidayGet
//    {
//        public static bool GetHoliday(out List<HolidayModel> holidays, int? day, int? month, int? year)
//        {
//            using (var client = new HttpClient())
//            {
//                // HTTP
//                client.BaseAddress = new Uri(Common.ConfigHelper.GetByKey("ApiGetHoliday"));
//                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//                try
//                {
//                    var response = client.GetAsync("get?day=" + day + "&month=" + month + "&year=" + year).Result;
//                    string res = "";
//                    using (HttpContent content = response.Content)
//                    {
//                        // ... Read the string.
//                        Task<string> result = content.ReadAsStringAsync();
//                        res = result.Result;
//                        holidays = JsonConvert.DeserializeObject<List<HolidayModel>>(res);
//                        return true;
//                    }
//                }
//                catch (Exception ex)
//                {
//                    holidays = null;
//                }
//                return false;
//            }
//        }
//    }
//}
