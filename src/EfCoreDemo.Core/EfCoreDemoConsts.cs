namespace EfCoreDemo.Core
{
    public class EfCoreDemoConsts
    {

        public const string ConnectionStringName = "Default";

        public const string ConnectionString = "Server=.; Database=EfCoreDemo_1; Uid=sa; Pwd=qwe123;";

        public const bool UseInMemory = false;

        public static string WebMvcFolder = null;

        public const int StringLength20 = 20;

        public const int StringLength50 = 50;
        /// <summary>
        /// 预热次数
        /// </summary>
        public const int Test_Warmup_Count = 2;
        /// <summary>
        /// 执行次数
        /// </summary>
        public const int Test_Target_Count = 12;

        public const string KeyWord = "金";
    }
}
