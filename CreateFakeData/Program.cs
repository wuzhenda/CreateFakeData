using CannyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateFakeData
{
    class Program
    {
        static void Main(string[] args)
        {
            //DbHelper.InsertElevators();
            //DbHelper.GetElevatorByRegCode("31303201112013010002");
            //elevatorRuntimeStatusList = dbConnection.Find<byetorrunstatu>(statement => statement
            //                .Where($"{nameof(byetorrunstatu.etor_id):I}={elevator.id}")
            //                .OrderBy($"{nameof(byetorrunstatu.id)} DESC"));
            //elevatorRuntimeStatusList = dbConnection.GetList<byetorrunstatu>("where etor_id=@etor_id",new byetorrunstatu { etor_id =  elevator.id });

            //DbHelper.GetElevatorByRegCode("");
            DbHelper.UpdateElevators();
            //DbHelper.Test();
            Console.ReadLine();
        }
    }
}
