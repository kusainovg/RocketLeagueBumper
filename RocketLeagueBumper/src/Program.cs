using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using OpenQA.Selenium.Interactions;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

            Random random = new Random();

            WebDriverWait wait = new WebDriverWait(chrDriver, TimeSpan.FromSeconds(5));

            Actions actions = new Actions(chrDriver);

            while (true)
            {
                chrDriver.Navigate().GoToUrl(settings.urlTrade);

                

                if(chrDriver.FindElementByXPath("//i[@class='fa fa-times']").Displayed)
                    wait.Until(x => x.FindElement(By.XPath("//i[@class='fa fa-times']"))).Click();

                List<IWebElement> chrElements = chrDriver.FindElementsByXPath("//button[@class='rlg-trade__action rlg-trade__bump ']").ToList();

                Console.WriteLine("Number of trades: " + chrElements.Count);

                LinkedList<string> list = new LinkedList<string>();

                foreach(var element in chrElements)
                {
                    list.AddLast("https://rocket-league.com/trade/" + element.GetAttribute("data-alias"));
                }

                foreach(var item in list)
                {
                    Console.WriteLine(item);
                }

                foreach(var item in list)
                {
                    chrDriver.Navigate().GoToUrl(item);

                    if (chrDriver.FindElementByXPath("//button[@class='rlg-trade__action rlg-trade__bump ']").Displayed)
                        chrDriver.FindElementByXPath("//button[@class='rlg-trade__action rlg-trade__bump ']").Click();

                    System.Threading.Thread.Sleep(3000);
                }

                Console.WriteLine("Waiting 15 min");
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
#region previous loop
//while (true)
//{
//    chrDriver.Navigate().GoToUrl(settings.urlTrade);

//    if (chrDriver.FindElementByXPath("//i[@class='fa fa-times']").Displayed)
//        wait.Until(x => x.FindElement(By.XPath("//i[@class='fa fa-times']"))).Click();

//    List<IWebElement> chrElements = chrDriver.FindElementsByXPath("//button[@class='rlg-trade__action rlg-trade__bump ']").ToList();

//    Console.WriteLine("Number of trades: " + chrElements.Count);

//    foreach (var item in chrElements)
//    {
//        if (chrDriver.FindElementByXPath("//i[@class='fa fa-times']").Displayed)
//            chrDriver.FindElementByXPath("//i[@class='fa fa-times']").Click();

//        actions.MoveToElement(item);

//        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(item)).Click();


//        System.Threading.Thread.Sleep(2000 + random.Next(1, 50));

//        //wait.Until(x => x.FindElement(By.XPath("//i[@class='fa fa-times']"))).Click();

//        if (chrDriver.FindElementByXPath("//i[@class='fa fa-times']").Displayed)
//            chrDriver.FindElementByXPath("//i[@class='fa fa-times']").Click();

//        System.Threading.Thread.Sleep(2000 + random.Next(1, 1000));
//    }
//    Console.WriteLine("Waiting 15 min");
//    System.Threading.Thread.Sleep(1000000 + random.Next(1, 30000));
//}
#endregion