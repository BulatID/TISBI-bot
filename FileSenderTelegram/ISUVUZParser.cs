using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserTISBINew
{
    internal class ISUVUZParser : PageLoader
    {
        public string Text { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public ISUVUZParser(string url) : base(url)
        {


        }


        public void LoginAndPassword()
        {

            Login = Console.ReadLine();
            Password = Console.ReadLine();
        }
        public string ScheduleParser()  //Парсит инфу с сайта ису вуз
        {
            OpenPage();  //Открывает сайт


            Thread.Sleep(1500);
            driver.FindElement(By.XPath(@"//input[contains(@name,'Login')]")).SendKeys(Login);  // Вводит логин


            driver.FindElement(By.XPath(@"//input[contains(@name,'Password')]")).SendKeys(Password);  //Вводит пароль
            Thread.Sleep(1500);

            driver.FindElement(By.XPath(@"//button[@type='button'][contains(.,'Войти')]")).Click(); //Нажимает кнопку Войти
            Thread.Sleep(5000);
            try
            {
                driver.FindElement(By.XPath(@"//button[@type='button'][contains(.,'Начать работу')]")).Click();  // Нажимает на кнопку Начать Работу
            }
            catch (Exception e)
            {
                Text = "Ошибка при вводе логина или пароля, повторите попытку:";
                return Text;
                LoginAndPassword();
            }

            using (TisbiContext db = new TisbiContext())
            {
                User user = new User { _Login = Login, _Password = Password, Admin = false };

                db.Users.Add(user);
                db.SaveChanges();
                Console.WriteLine("Changes Saved!");

                var users = db.Users.ToList();
                foreach (User u in users)
                {
                    Console.WriteLine($"{u.Id}, {u._Login}, {u._Password}");
                }
            }
            Thread.Sleep(5000);
            for (int i = 0; i < 10; i++)
            {
                try
                {

                    driver.FindElement(By.XPath(@$"(//button[contains(.,'Ознакомиться')])[{i}]")).Click();  // Ищет кнопку Ознакомиться, если её нет то он продолжает её искать

                }
                catch (Exception ex)
                {
                    continue;
                }
            }


            driver.FindElement(By.XPath(@"//a[contains(@data-toggle,'dropdown')]")).Click();  //Открывает меню для взаимодействия с сайтом
            Thread.Sleep(5000);
            driver.FindElement(By.XPath(@"//a[@class='dropdown-item'][contains(.,'Расписание занятий')]")).Click(); // Нажимает на Расписание занятий
            Console.Clear();
            Thread.Sleep(5000);

            for (int i = 1; i <= 6; i++)
            {
                try
                {

                    Text += driver.FindElement(By.XPath($@"(//td[@class='align-middle table-primary'])[{i}]")).Text;  // Ищет ячейки таблицы с синим цветом и помещает в переменную


                }
                catch (Exception e)
                {
                    continue;
                }


            }


            driver.Close();
            if (Text is null)
            {
                Text = "На сегодня занятий нет";
                return Text;
            }
            return Text;  //Возвращает значение переменной с расписанием внутри, иначе ничего не возвращает.

        }
    }
}