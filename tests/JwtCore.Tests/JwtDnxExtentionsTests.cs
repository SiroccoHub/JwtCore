using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using JwtCore;
using Xunit;

namespace JwtCore.Tests
{
    public class ExtentionsTests
    {

        private static DateTime _netDateTimeUtc = new DateTime(2038, 01, 19, 03, 14, 07, DateTimeKind.Utc);
        private static long _unixTimestamp = 2147483647L;

        [Fact]
        public void Should_Convert_from_DateTime_to_UnixTime()
        {
            long result = _netDateTimeUtc.ToUnixTimeSeconds();
            Assert.Equal(result, _unixTimestamp);
        }

        [Fact]
        public void Should_Convert_from_UnixTime_to_DateTime()
        {
            DateTime result = _unixTimestamp.ToDateTiemUtc();
            Assert.Equal(result, _netDateTimeUtc);
        }
    }
}
