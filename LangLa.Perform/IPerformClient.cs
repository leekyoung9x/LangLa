using System.IO;
using LangLa.IO;

namespace LangLa.Perform
{
	internal interface IPerformClient
	{
		Message Listen(BinaryReader Read);

		void Action(Message message);

		void Start(BinaryReader read, BinaryWriter writer);

		void Perform(BinaryWriter writer, Message m);

		void AddMessage(Message m);

		void close();
	}
}
