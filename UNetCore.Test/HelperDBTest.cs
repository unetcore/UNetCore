using System;
using Xunit;

namespace UNetCore.Test {
    public class HelperDBTest {
        [Fact]
        public void Test1 () {
            UNetCore.Helper.DB.MySql ss = new UNetCore.Helper.DB.MySql ("server=120.79.11.51;database=dalianmao;uid=root;pwd=Lichao8888;port=3306;Convert Zero Datetime=True;Allow Zero Datetime=True;SslMode=none;charset=utf8");

            Console.WriteLine (ss.ExecuteNonQuery ("select count(id) from UserID").ToString ());

            // UNetCore.Helper.DB
        }

        [Fact]
        public void Test2 () {
            UNetCore.Helper.DB.MySql ss = new UNetCore.Helper.DB.MySql ("server=120.79.11.51;database=dalianmao;uid=root;pwd=Lichao8888;port=3306;Convert Zero Datetime=True;Allow Zero Datetime=True;SslMode=none;charset=utf8");

            Console.WriteLine (ss.ExecuteScalar ("select count(id) from UserID").ToString ());

            // UNetCore.Helper.DB
        }
    }
}