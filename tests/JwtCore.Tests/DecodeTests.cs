using System;
using System.Collections.Generic;
using JwtCore;
using Xunit;
using FluentAssertions;

namespace JwtCore.Tests
{
    public class DecodeTests
    {
        private static readonly Customer customer = new Customer { FirstName = "Bob", Age = 37 };

        private const string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJGaXJzdE5hbWUiOiJCb2IiLCJBZ2UiOjM3fQ.cr0xw8c_HKzhFBMQrseSPGoJ0NPlRp_3BKzP96jwBdY";
        private const string malformedtoken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9eyJGaXJzdE5hbWUiOiJCb2IiLCJBZ2UiOjM3fQ.cr0xw8c_HKzhFBMQrseSPGoJ0NPlRp_3BKzP96jwBdY";

        private static readonly IDictionary<string, object> dictionaryPayload = new Dictionary<string, object>
        { 
            { "FirstName", "Bob" },
            { "Age", 37 }
        };

        [Fact]
        public void Should_Decode_Token_To_Json_Encoded_String()
        {
            var jsonSerializer = new DefaultJsonSerializer();
            var expectedPayload = jsonSerializer.Serialize(customer);

            string decodedPayload = JsonWebToken.Decode(token, "ABC", false);

            Assert.Equal(expectedPayload, decodedPayload);
        }

        [Fact]
        public void Should_Decode_Token_To_Dictionary()
        {
            object decodedPayload = JsonWebToken.DecodeToObject(token, "ABC", false);

            decodedPayload.ShouldBeEquivalentTo(dictionaryPayload, options => options.IncludingAllRuntimeProperties());
        }

        [Fact]
        public void Should_Decode_Token_To_Dictionary_With_Newtonsoft()
        {
            JsonWebToken.JsonSerializer = new DefaultJsonSerializer();

            object decodedPayload = JsonWebToken.DecodeToObject(token, "ABC", false);

            decodedPayload.ShouldBeEquivalentTo(dictionaryPayload, options => options.IncludingAllRuntimeProperties());
        }

        [Fact]
        public void Should_Decode_Token_To_Generic_Type()
        {
            Customer decodedPayload = JsonWebToken.DecodeToObject<Customer>(token, "ABC", false);

            decodedPayload.ShouldBeEquivalentTo(customer);
        }

        [Fact]
        public void Should_Decode_Token_To_Generic_Type_With_Newtonsoft()
        {
            JsonWebToken.JsonSerializer = new DefaultJsonSerializer();

            Customer decodedPayload = JsonWebToken.DecodeToObject<Customer>(token, "ABC", false);

            decodedPayload.ShouldBeEquivalentTo(customer);
        }

        [Fact]
        public void Should_Throw_On_Malformed_Token()
        {
            Exception ex = Assert.Throws<ArgumentException>(() =>
            {
                JsonWebToken.DecodeToObject<Customer>(malformedtoken, "ABC", false);
            });
        }

        [Fact]
        public void Should_Throw_On_Invalid_Key()
        {
            var invalidkey = "XYZ";

            Exception ex = Assert.Throws<SignatureVerificationException>(() =>
            {
                JsonWebToken.DecodeToObject<Customer>(token, invalidkey, true);
            });
        }

        [Fact]
        public void Should_Throw_On_Invalid_Expiration_Claim()
        {
            var invalidexptoken = JsonWebToken.Encode(new { exp = "asdsad" }, "ABC", JwtHashAlgorithm.HS256);

            Exception ex = Assert.Throws <SignatureVerificationException> (() =>
            {
                JsonWebToken.DecodeToObject<Customer>(invalidexptoken, "ABC", true);
            });
        }

        [Fact]
        public void Should_Throw_On_Expired_Token()
        {
            var anHourAgoUtc = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0));
            var unixTimestamp = (int)(anHourAgoUtc.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            var invalidexptoken = JsonWebToken.Encode(new { exp = unixTimestamp }, "ABC", JwtHashAlgorithm.HS256);

            Exception ex = Assert.Throws <SignatureVerificationException> (() =>
            {
                JsonWebToken.DecodeToObject<Customer>(invalidexptoken, "ABC", true);
            });
        }
    }

    public class Customer
    {
        public string FirstName { get; set; }

        public int Age { get; set; }
    }
}