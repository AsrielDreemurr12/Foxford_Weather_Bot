using System;
using static System.Console;
using Telegram.Bot;
using System.Net.Http;

namespace Weather_bot
{
	class Program
	{
		static void Main(string[] args)
		{
			string token = "1839900183:AAECEuh1i-Nc0ADs9fnjz_spxvW0GV7iDwg";
			TelegramBotClient client = new TelegramBotClient(token);
			string sendMessage = "Моя твоя не понимать";

			client.OnMessage += (s, args) => {
				WriteLine();
				string msg = args.Message.Text;
				string[] param = msg.Split(' ');

				switch (param[0])
				{
					case "/start":
						sendMessage = "Привет! Готов служить!\n 1)/weather - Узнать погоду в городе(после /weather через пробел пишем нужный нам город)";
						sendMessage += "\n 2)/time - Узнать точное время в городе";
						break;
					case "/weather":
						try
						{
							sendMessage = Weather(region: param[1]);
						}
						catch
						{
							sendMessage = "Сорян, но что-то пошло не так:(";
						}
						
						break;
					
					case "/time":
						try
						{
							sendMessage = Time(region: param[1]);
						}
						catch
						{
							sendMessage = "Сорян, но что-то пошло не так:(";
						}
						break;

				}				
				WriteLine(args.Message.Text);

				client.SendTextMessageAsync(
					chatId: args.Message.Chat.Id,
					text: sendMessage
					);
			};
			client.StartReceiving();
			ReadLine();
		}

		private static string Weather(string region) {
			HttpClient client = new HttpClient();
			string answer = client.GetStringAsync($"https://wttr.in/{region}?format=4").Result;
			return answer;
		}		
		private static string Time(string region)
		{
			HttpClient client = new HttpClient();
			string answer = client.GetStringAsync($"https://wttr.in/{region}?format=" + @"""%l: +%T""").Result;
			return answer.Substring(1, answer.Length - 7) + $"; UT + {answer[answer.Length-4]}";
		}
	}
}
