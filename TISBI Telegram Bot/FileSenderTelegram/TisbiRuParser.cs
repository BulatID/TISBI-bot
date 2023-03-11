using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserTISBINew
{
    internal class TisbiRuParser : PageLoader
    {
        public string Text { get; set; }


        public TisbiRuParser(string url) : base(url)
        {

        }
        // Эти методы просто выводит информацию для абитуриентов
        public string ButtonOne()
        {
            OpenPage();
            IWebElement tag = driver.FindElement(By.ClassName(@"covid-message"));
            Text = tag.Text;
            driver.Close();
            Console.Clear();
            
            return Text;
        }

        public string ButtonTwo()
        {
            OpenPage();
            IWebElement tag = driver.FindElement(By.ClassName(@"covid-scheme"));
            Text = tag.Text;
            driver.Close();
            Console.Clear();
            return Text;
        }

        public string ButtonThree()
        {
            OpenPage();
            return ParseText(3, 11);

        }

        public string ButtonFour()
        {
            OpenPage();
            return ParseText(12, 16);
        }

        public string ButtonFive()
        {
            OpenPage();
            return ParseText(14, 23);
        }

        public string ButtonSix()
        {
            OpenPage();
            return ParseText(17, 19);
        }

        public string ButtonSeven()
        {
            OpenPage();
            Text += ParseText(18, 20);
            for (int i = 11; i <= 21; i++)
            {

                IWebElement tag = driver.FindElement(By.XPath($@"(//li[contains(@class,'postupit-tabs__li')])[{i}]"));
                Text += tag.Text;

            }
            return Text;
        }

        public string ButtonEight()
        {
            OpenPage();
            return ParseText(23, 26);
        }

        public string ButtonNine()
        {
            OpenPage();
            return ParseText(27, 32);
        }

        public string ButtonTen()
        {
            OpenPage();
            return ParseText(33, 37);
        }
    }
}