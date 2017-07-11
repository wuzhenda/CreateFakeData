using CannyData.Models;
using Dapper.FastCrud;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CannyData
{

    public static class DbHelper
    {
        const int NotUse = 0;
        const int SuZhouTeJianYuan=1;
        const int NanJinTeJianYuan = 2;

        public static Func<DbConnection> ConnectionFactory = () => new SqlConnection(ConnectionString.Connection);
        public static class ConnectionString
        {
            public static string Connection = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
        }

        static DbHelper()
        {
            OrmConfiguration.DefaultDialect = SqlDialect.MsSql;
        }

        public static class SqlText
        {
           // public static string Invoice_Select = "SELECT * FROM Invoice;";

        }

        public static byetor GetElevatorByRegCode(String regcode)
        {
            byetor etor;

            using (var dbConnection = ConnectionFactory())
            {
                dbConnection.Open();

                var etorList = dbConnection.Find<byetor>(statement => statement
                                .Where($"{nameof(byetor.field_1):C}=@field_1 And {nameof(byetor.field_2):C}=@field_2")
                                .OrderBy($"{nameof(byetor.id)} DESC")
                                .WithParameters(new { field_1= NanJinTeJianYuan, field_2 = regcode }));
                etor = etorList.FirstOrDefault();
            }

            return etor;
        }


        public static bydtu GetDtuByid(int dtu_Id)
        {
            bydtu dtu=null;

            using (var dbConnection = ConnectionFactory())
            {
                dbConnection.Open();
                dtu = dbConnection.Get(new bydtu { id = dtu_Id });
            }

            return dtu;
        }

        //获取南京特检院目标电梯
        public static void InsertElevators()
        {
            
            long G_Count = 10000;
            using (var dbConnection = ConnectionFactory())
            {
                dbConnection.Open();
                String eNu = "0000000000000001";
                var NumOne = dbConnection.Find<byetor>(statement => statement
                                .Where($"{nameof(byetor.bianhao):C}=@bianhao")
                                .WithParameters(new { bianhao = eNu })).First();

                for (int i = 6324; i < G_Count; i++)
                {
                   
                    var bianhao = String.Format("{0:d16}", i);

                    var dtu = dbConnection.Find<bydtu>(statement => statement
                                .Where($"{nameof(bydtu.bianhao):C}=@bianhao")
                                .WithParameters(new { bianhao = bianhao })).FirstOrDefault();
                    if (dtu == null)
                    {
                        continue;
                    }
                    var newEtor = new byetor()
                    {
                        bianhao = bianhao,
                        dtu_id = dtu.id,
                        etor_species_id = NumOne.etor_species_id,
                        project_id = NumOne.project_id,
                        organize_id = NumOne.organize_id,
                        dizhi_code = NumOne.dizhi_code,
                        build_name = NumOne.build_name,
                        build_number = NumOne.build_number,
                        build_etorindex = NumOne.build_etorindex,
                        dizhi_detail = NumOne.dizhi_detail,
                        zuobiao = NumOne.zuobiao,
                        zhuban_code = NumOne.zhuban_code,
                        zhuban_version = NumOne.zhuban_version,
                        zhuban_version_time = NumOne.zhuban_version_time,
                        floorcount = NumOne.floorcount,
                        weibao_user = NumOne.weibao_user,
                        weibao_userphone = NumOne.weibao_userphone,
                        weibao_manager = NumOne.weibao_manager,
                        weibao_managerphone = NumOne.weibao_managerphone,
                        weibao_interval = NumOne.weibao_interval,
                        weibao_type = NumOne.weibao_type,
                        nianjian_time = NumOne.nianjian_time,
                        use_place = NumOne.use_place,
                        brand_id = NumOne.brand_id,
                        etor_model = NumOne.etor_model,
                        product_model = NumOne.product_model,
                        create_time = NumOne.create_time,
                        create_userid = NumOne.create_userid,
                    };

                    dbConnection.Insert(newEtor);
                    Console.WriteLine("done:" + i);
                }

            }
        }//end fun

        //organize_id = 98 苏州特检院，organize_id = 6 苏州分公司
        //project_id = 1769 测试项目组
        //        update by_etor  set organize_id = 98 where project_id = 1769
        //SELECT count(*) from by_etor   where organize_id = 6 and project_id = 1769
        public static void UpdateElevators()
        {
            var sw = new Stopwatch();
            
            using (var dbConnection = ConnectionFactory())
            {
                Console.WriteLine("timeout:" + dbConnection.ConnectionTimeout);
                dbConnection.Open();
                sw.Start();
                try
                {
                    var NumOne = dbConnection.BulkUpdate<byetor>(new byetor { organize_id = 6 }, statement => statement
                                      .Where($"{nameof(byetor.project_id):C}=1769")
                                      .WithTimeout(TimeSpan.FromSeconds(300)));
                }catch(Exception e)
                {                   
                    Console.WriteLine(e);
                }

                sw.Stop();
                Console.WriteLine("do job MaintainDataUpload spandtime Ms:" + sw.ElapsedMilliseconds);
            }
        }//end fun

        public static void Test()
        {
            string cString = @"Data Source=192.168.30.49;Initial Catalog=NORTHWND;uid=cctv;pwd=123456;Max Pool Size=512";
            using (SqlConnection sc = new SqlConnection(cString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("waitfor delay '00:00:30';select  * from Region", sc);
                    cmd.CommandTimeout = 40;
                    Console.WriteLine("CommandTimeout:{0}", cmd.CommandTimeout);
                    sc.Open();
                    SqlDataReader r = cmd.ExecuteReader();

                    while (r.Read())
                        Console.WriteLine("{0}:{1}",r[0].ToString(),r[1].ToString());

                    sc.Close();
                } catch (SqlException se)
                {
                    Console.WriteLine(se);
                }
            }
        }


    }
}
