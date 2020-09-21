using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Linq;

namespace RocketLeagueBumper
{
    class Program
    {
        static void Main(string[] args)
        {
            MySettings settings = MySettings.Load();

            if(settings.urlTrade == null)
            {
                Console.WriteLine("Input url of trades: "); 
                settings.urlTrade = Console.ReadLine();
            }

            settings.Save();

            ChromeOptions chrOptions = new ChromeOptions();

            chrOptions.AddArgument("--user-data-dir=" + settings.userDataDir);
            chrOptions.AddArgument("--profile-directory=Profile 1");
            chrOptions.AddArgument("--start-maximized");

            ChromeDriver chrDriver = new ChromeDriver(chrOptions);

            chrDriver.Navigate().GoToUrl(settings.urlTrade);

            Random random = new Random();

            while(true)
            {
                List<IWebElement> chrElements = chrDriver.FindElementsByXPath("//button[@class='rlg-trade__action rlg-trade__bump ']").ToList();

                Console.WriteLine(chrElements.Count);

                foreach(var item in chrElements)
                {
                    item.Click();

                    System.Threading.Thread.Sleep(1000 + random.Next(1, 50));

                    chrDriver.FindElementByXPath("//i[@class='fa fa-times']").Click();

                    System.Threading.Thread.Sleep(3000 + random.Next(1, 1000));
                }
                
                System.Threading.Thread.Sleep(900000 + random.Next(1, 30000));
            }
        }
        class MySettings : AppSettings<MySettings>
        {
            public string urlTrade;
            public string userDataDir = AppDomain.CurrentDomain.BaseDirectory + "google chrome user\\1";
        }
    }
    public class AppSettings<T> where T : new()
    {
        private const string DEFAULT_FILENAME = "settings.json";

        public void Save(string fileName = DEFAULT_FILENAME)
        {
            File.WriteAllText(fileName, JsonConvert.SerializeObject(this));
        }

        public static void Save(T pSettings, string fileName = DEFAULT_FILENAME)
        {
            File.WriteAllText(fileName, JsonConvert.SerializeObject(pSettings));
        }

        public static T Load(string fileName = DEFAULT_FILENAME)
        {
            T t = new T();
            if (File.Exists(fileName))
                t = JsonConvert.DeserializeObject<T>(File.ReadAllText(fileName));
            return t;
        }
    }
}
