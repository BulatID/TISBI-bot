using System.IO;
using System.Threading.Tasks;
using Deployf.Botf;
using Microsoft.Extensions.Configuration;
using ParserTISBINew;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace TISBIHelperBot
{
    public class TelegramBot : BotController
    {
        private readonly IConfiguration _configuration;

        public TelegramBot(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Action("/start", "Запустить бота")]
        public void Start()
        {
            Photo("https://sun9-38.userapi.com/impf/bxcEPQoMs0evJHPxQabzJVGrWPRUAiO30l30sQ/locO7P56O04.jpg?size=1074x480&quality=96&sign=b7c077bda70fbbbc7b8f3b4c5d42eaf8&type=share");
            PushLL($"Привет, {Context!.GetUsername()!} 👋👋👋");
            PushLL("Я — неофициальный бот Университета Управления ТИСБИ 🤖");
            PushLL("Вот что я умею: ");
            PushL("— Узнаю твое расписание пар на сегодня 😍");
            PushL("— Поднимаю твое настроение ☀️");
            PushL("Хочешь узнать больше про функционал? Напиши /all, друг 😉");
            PushLL("И это не все! В будущем, мои разработчики добавят больше функции, чтобы облегчить твое обучение 😁");
            RowButton("Продолжить ➡️", Q(MainMenu));

            Send();
        }

        [Action("/delete", "Удалить личные данные")]
        public void deleteUserData()
        {
            //Тут он должен удалить логин и пароль от ису вуз

            Photo("https://chudo-prirody.com/uploads/posts/2021-08/1628756571_38-p-kot-plachet-foto-42.jpg");

            PushL("Данные успешно удалены 😢");
            PushLL("Обратите внимание, что из-за этого будет:");
            PushL("— невозможно больше узнать расписание");
            PushL("— исключение из общей беседы для студентов через некоторое время");
            PushLL("Чтобы вернуть эти преимущества, введите /data");

            RowButton("Продолжить 🏠", Q(Start));

            Send();
        }

        [Action("/data", "Сохранить данные от isu.tisbi.ru")]
        public void saveDataDialog()
        {
            PushL($"Вы уверены, что хотите дать доступ к своему аккаунту от isu.tisbi.ru боту @{Context!.Bot.Username}?");
            PushLL("Вы всегда можете удалить эти данные, написав /delete");
            RowButton("Да, продолжить", Q(saveDataFunc));
            RowButton("Нет", Q(Start));
            
            Send();
        }

        [Action]
        public async Task saveDataFunc()
        {
            PushL("Введите логин:");

            await Send();

            var Login = await AwaitText();

            PushL("Введите пароль:");

            await Send();

            var Password = await AwaitText();

            //Тут должно произойти сохранение в БД

            await Context.Bot.Client.SendTextMessageAsync(ChatId, "Ваш логин и пароль от сайта isu.tisbi.ru сохранен!");

            StudentMenu();
        }

        [Action("/paspisanie", "Узнать расписание")]
        public void raspisanie(string login, string password)
        {
            PushL("Получаю твое расписание 😉");
            Send();
            //Client.DeleteMessageAsync(ChatId, Context.Update.Message.MessageId);

            var isuVuzParser = new ISUVUZParser("https://isu.tisbi.ru/student/login", login, password);
            
            Context.Bot.Client.SendTextMessageAsync(ChatId, isuVuzParser.ScheduleParser());
        }

        [Action]
        void MainMenu()
        {
            //Photo("https://sun7-13.userapi.com/A_OprgHm_XVzD8jXCbl3DXXcOVyNOqqTdogdaQ/oFg2FEFbQAU.jpg");

            Photo("https://www.tisbi.ru/bundles/site/tisbi2/assets/bg__postupit.jpg");
            PushL("Выберите свою роль:");

            RowButton("Студент 👨‍🎓", Q(StudentMenu));
            RowButton("Преподаватель 👨‍🏫", Q(TeacherMenu));
            RowButton("Абитуриент 🌟", Q(AbiturientMenu));

            Send();
        }

        [Action]
        void StudentMenu()
        {
            Photo("https://sun9-38.userapi.com/impf/bxcEPQoMs0evJHPxQabzJVGrWPRUAiO30l30sQ/locO7P56O04.jpg?size=1074x480&quality=96&sign=b7c077bda70fbbbc7b8f3b4c5d42eaf8&type=share");

            PushL("Студент, вот что я умею:");

            RowButton("Узнать расписание на сегодня 🗓", Q(getRaspisanieOld));
            RowButton("Студенческий совет 🌟", Q(MainMenu));
            RowButton("Общая беседа для всех студентов университета 💬", Q(MainMenu));

            RowButton("Вернуться к выбору роли", Q(MainMenu));
            Send();
        }

        [Action]
        public async Task getRaspisanieOld() //Старый способ, пока не будет сделан новый
        {
            PushL("Введите логин:");
            RowButton("Отмена", Q(StudentMenu));

            await Send();

            var Login = await AwaitText();

            PushL("Введите пароль:");
            RowButton("Отмена", Q(StudentMenu));

            await Send();

            var Password = await AwaitText();

            PushL("Получаю твое расписание 😉");
            Send();

            var isuVuzParser = new ISUVUZParser("https://isu.tisbi.ru/student/login", Login, Password);

            await Context.Bot.Client.SendTextMessageAsync(ChatId, isuVuzParser.ScheduleParser());
        }

        [Action]
        void TeacherMenu()
        {
            PushLL("Мы работает над добавлением функционала для преподавателей 💪");
            
            MainMenu();
        }

        [Action("/abiturient", "Информация для Абитурентов")]
        public void AbiturientMenu()
        {
            Photo("https://rdb-rt.ru/800/600/https/p2.zoon.ru/e/c/5239c75940c08850138bb60b_5d1f064860ccb.jpg");
            PushLL("Дорогой Абитуриент, добро пожаловать в ТИСБИ!");
            PushLL("Университет управления «ТИСБИ» — первый негосударственный вуз Республики Татарстан с государственной аккредитацией, " +
                "основан в 1992 году. Наш главный приоритет — качество и конкурентоспособность подготовки студента.");
            PushLL("Наш бот всегда старается получать актуальную информацию с сайта!");
            PushLL("Выберите интересующую информацию:");

            RowButton("Информация о подаче документов", Q(Abitur1));
            RowButton("Перечень документов для поступления", Q(Abitur2));
            RowButton("Основные даты для поступления на бюджет", Q(Abitur3));
            RowButton("Основные даты для поступления на коммерцию", Q(Abitur4));
            RowButton("Схема поступления", Q(Abitur5));
            RowButton("Вступительные испытания", Q(Abitur6));
            RowButton("Как проходит зачисление", Q(Abitur7));
            RowButton("Расписание онлайн-консультаций с приемной комиссией", Q(Abitur8));
            RowButton("Расписание онлайн-консультаций с деканами факультетов", Q(Abitur9));
            RowButton("Расписание онлайн Дней открытых дверей", Q(Abitur10));
            Button("Узнать подробнее", "https://www.tisbi.ru/postupit/");
            RowButton("Вернуться в главное меню 🏠", Q(MainMenu));

            Send();
        }

        [Action]
        void Abitur1()
        {
            var tisbiRuParser = new TisbiRuParser("https://www.tisbi.ru/postupit/");
            Context.Bot.Client.SendTextMessageAsync(ChatId, "Загружаю информацию ⏳");
            Thread.Sleep(500);
            Context.Bot.Client.SendTextMessageAsync(ChatId, tisbiRuParser.ButtonOne());
        }

        [Action]
        void Abitur2()
        {
            var tisbiRuParser = new TisbiRuParser("https://www.tisbi.ru/postupit/");
            Context.Bot.Client.SendTextMessageAsync(ChatId, "Загружаю информацию ⏳");
            Thread.Sleep(500);
            Context.Bot.Client.SendTextMessageAsync(ChatId, tisbiRuParser.ButtonTwo());
        }

        [Action]
        void Abitur3()
        {
            var tisbiRuParser = new TisbiRuParser("https://www.tisbi.ru/postupit/");
            Context.Bot.Client.SendTextMessageAsync(ChatId, "Загружаю информацию ⏳");
            Thread.Sleep(500);
            Context.Bot.Client.SendTextMessageAsync(ChatId, tisbiRuParser.ButtonThree());
        }

        [Action]
        void Abitur4()
        {
            var tisbiRuParser = new TisbiRuParser("https://www.tisbi.ru/postupit/");
            Context.Bot.Client.SendTextMessageAsync(ChatId, "Загружаю информацию ⏳");
            Thread.Sleep(500);
            Context.Bot.Client.SendTextMessageAsync(ChatId, tisbiRuParser.ButtonFour());
        }

        [Action]
        void Abitur5()
        {
            var tisbiRuParser = new TisbiRuParser("https://www.tisbi.ru/postupit/");
            Context.Bot.Client.SendTextMessageAsync(ChatId, "Загружаю информацию ⏳");
            Thread.Sleep(500);
            Context.Bot.Client.SendTextMessageAsync(ChatId, tisbiRuParser.ButtonFive());
        }

        [Action]
        void Abitur6()
        {
            var tisbiRuParser = new TisbiRuParser("https://www.tisbi.ru/postupit/");
            Context.Bot.Client.SendTextMessageAsync(ChatId, "Загружаю информацию ⏳");
            Thread.Sleep(500);
            Context.Bot.Client.SendTextMessageAsync(ChatId, tisbiRuParser.ButtonSix());
        }

        [Action]
        void Abitur7()
        {
            var tisbiRuParser = new TisbiRuParser("https://www.tisbi.ru/postupit/");
            Context.Bot.Client.SendTextMessageAsync(ChatId, "Загружаю информацию ⏳");
            Thread.Sleep(500);
            Context.Bot.Client.SendTextMessageAsync(ChatId, tisbiRuParser.ButtonSeven());
        }

        [Action]
        void Abitur8()
        {
            var tisbiRuParser = new TisbiRuParser("https://www.tisbi.ru/postupit/");
            Context.Bot.Client.SendTextMessageAsync(ChatId, "Загружаю информацию ⏳");
            Thread.Sleep(500);
            Context.Bot.Client.SendTextMessageAsync(ChatId, tisbiRuParser.ButtonEight());
        }

        [Action]
        void Abitur9()
        {
            var tisbiRuParser = new TisbiRuParser("https://www.tisbi.ru/postupit/");
            Context.Bot.Client.SendTextMessageAsync(ChatId, "Загружаю информацию ⏳");
            Thread.Sleep(500);
            Context.Bot.Client.SendTextMessageAsync(ChatId, tisbiRuParser.ButtonNine());
        }

        [Action]
        void Abitur10()
        {
            var tisbiRuParser = new TisbiRuParser("https://www.tisbi.ru/postupit/");
            Context.Bot.Client.SendTextMessageAsync(ChatId, "Загружаю информацию ⏳");
            Thread.Sleep(500);
            Context.Bot.Client.SendTextMessageAsync(ChatId, tisbiRuParser.ButtonTen());
        }

        [Action("/about", "О боте")]
        public void aboutBot()
        {
            Photo("https://i.ibb.co/LNgsTVj/About.png");
            PushL("Наша цель — помочь и объединить всех студентов «Университета Управления «ТИСБИ»!");
            PushLL("Мы — два разработчика факультета информационных технологий стремимся сделать вашу жизнь лучше! 🧑‍💻");
            PushLL("Поддержите нас, отправив ссылку на бота другим студентам!");
            PushLL("Спасибо, что вы с нами ❤️");

            RowButton("Хорошо 😉", Q(MainMenu));
        }

        [On(Handle.Unknown)]
        public async Task Unknown()
        {
            PushL("Похоже, я сломался ☹️");
        }
    }
}