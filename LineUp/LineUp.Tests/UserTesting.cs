
using Newtonsoft.Json;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LineUp.Tests
{
    [TestClass]
    public class UserTesting
    {
        string BaseAddress = @"https://localhost:7166/";
        string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9zaWQiOiIxYTNhZDg4OS05NjUxLTRkMGUtYmJkMC0yZWE3YTdjYzE4ODMiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiemFpbi5pc2xhbUBQb3dlcnNvZnQxOS5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ6YWluLmlzbGFtQFBvd2Vyc29mdDE5LmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlRlc3RlciIsImV4cCI6MTY4MDA5Mzc4NiwiaXNzIjoiUG93ZXJzb2Z0MTlBVE0iLCJhdWQiOiJQb3dlcnNvZnQxOUFUTSJ9.cG4ty3Okg_ftALe7w7N9Shwn9nkcrm7U2j-rGBRv7dk";

        [TestMethod]

        [DataRow("zain.islam@powersoft19.com", "", 400, false)]
        [DataRow("", "@Z41n1sl4m", 400, false)]
        [DataRow("", "", 400, false)]
        [DataRow("zain.islam@powersoft19.com", "@Z41n1sl4m", 500, false)]
        [DataRow("zain.islam@powersoft19.com", "Welcome@123", 200, false)]

        public async Task SignInTest(string email, string password, HttpStatusCode statusCode, bool authorized)
        {
            var parameters = new Dictionary<string, string> { { "email", email }, { "password", password } };

            var response = await LineUpWebRequest(parameters, "api/User/Login", CommandType.Post, authorized);

            Assert.AreEqual(response.StatusCode, statusCode);

        }

        //Get Post test cases

        [TestMethod]

        [DataRow(1, 2, 200, true)]
        [DataRow(1, 2, 200, true)]
        [DataRow(1, 1000, 200, true)]
        [DataRow(1, 0, 400, true)]
        [DataRow(1, 0, 401, false)]

        public async Task GetPostTest(int page, int pageSize, HttpStatusCode statusCode, bool authorized)
        {

            var response = await LineUpWebRequest("", $"api/Post/Get?page={page}&pageSize={pageSize}", CommandType.Get, authorized);

            Assert.AreEqual(statusCode, response.StatusCode);
        }


        //Create Post test cases

        [TestMethod]

        [DataRow("title test...", "/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAkGBxITEhUSEhIVFhUVFRcVFhYYFxAVFRUWFRUXFhUWFxUYHSggGBolGxUVITEhJSkrLi4uFx8zODMsNygtLisBCgoKBQUFDgUFDisZExkrKysrKysrKysrKysrKysrKysrKysrKysrKysrKysrKysrKysrKysrKysrKysrKysrK//AABEIAOEA4QMBIgACEQEDEQH/xAAcAAEAAgIDAQAAAAAAAAAAAAAAAQgGBwIEBQP/xAA9EAABAwIEBAMGBAUCBwEAAAABAAIDBBEFBxIhBjFBURMiYTJCUmJxkRRygaEjM5KxwQhTFhc0Q2OC8RX/xAAUAQEAAAAAAAAAAAAAAAAAAAAA/8QAFBEBAAAAAAAAAAAAAAAAAAAAAP/aAAwDAQACEQMRAD8A3iiIgIiICIiAiIgIiICIiAiIgIiICIiAiIgIiXQEUXS6CUREBERAREQEREBERAREQEREBEUXQSiXUXQSii6XQSii6AoJREQFBUqCg4lYBmBmfT4feJg8We3sD2W/nPRdHNfMltE001MQ6oeLEg7Rg7fdVyqahz3Oe9xc5xu5x5knqUG6sDz4cZbVdM1sZHtRlxcD02PTmtzYZiEVRE2aJ4fG8XaR69PqqVArOcs8wJcNl0Pu+mefOz4fnb2t2QWoCldPCsSiqImTQvD43gOa4evfsV27oJRAiAiIgIiICIiAiIgIiIC61XWRxMdJI8MY3cuJsAF9nvABJ5AXP0CrDmpx4+vndFE4imjJDRuNdubiPqgznizPBjHFlDEJCHEGR/skdC1a2r8zcUkeXfi3sB5NZYNH7LD3FcUGVx5i4oCD+OmNuhLSD+yzPBs86pha2ohY9vUi4efXstQqboLQ4Bm7htRcOeYCP9zkf1Czeiropmh8T2vaeRBBCpPdd2gxeeEh0Uz2FpuLOdb7ckF1ECrZgGdNfCbT6Z225EBh/qAWw8BzroJQBUB0DyeVnPb/AFAINorAM3uM3YfTARfzprtafgFt3LMKDGIJwDFMx9xcAOBdb6LUH+ovDHlsFQLlgJY7bZu2xP1QaQqZ3PcXvJc5xu5x5knmV8VyK4oC5rgpQZzlpmBLhsuh130zz52fDe13t9VZzCsRiqImzQvD43i7XDl9PqqVArOctMwJcNl0uu+mefOz4b++36dkFqUXTwvEop4mTQvD2PALSPUXsexXbQSii5RBKIiAiIgIiICgqVxKDBc4uIvwmHvDXFsk/wDDjI5g7En7Krbiti54cR/ia4wscTHB5bdBJ7xWuEAqERAREQEREHIFLriiDt4diEsDxJC90bxyc02KsFl1x5BisBoa5rTMW6S11tMzQOY7OVcl9qWodG4PY4tc03aRsQQgzjM7L2TDpPEju6lefK/qwn3Heu3NYEVYzLvjqDFYDQ1zWmYs0uB9mZvIuHZ2/JawzOy8kw6QyR3fTPPkf1YSfYd2t0KDAUXIhLIIapKljLkAAknYDrf0XZqcNmjGp8UjG8rua4D7lBl2WeYMuGy6XXfTPPnZ8JPvt9VZzCsRiqImTQvD2PALXDsf8qlYCzzKzjqWgnbETqp5Xta9hPsFxA1jtbsgtKi8X/iek/32/cIg9pERAREQEREBeBxvjjKOjlne61mlrfzu2b+698rQv+oTiXVJHQsds3zzD5tiz9kGnauodI9z3m7nEuce5PNfBSoQEUhSgiyWUqSg4IiICIiApBUIg+9LUOje17HFr2m7XDYgjkQrDZdcdQYrAaGua0zadJDraZmj3h8yrkvtS1Do3Nexxa5pu1w5ghBnGZ2XsmHSeJGC+mefI7mWHfyP9duawSysVl3x1BisBoa4NMxbpcHWtO2wFx2dvyWsczcvZMOkMjLvpnu8jvgJ30O7ehQZlkDwvTyRyVsrA97ZDGwOsWtADXagO91uTE8LhnjMUsbXscLEED7j1WhslOPIaPVR1PkZI/WyToHGws7sNr3W3sX47w+CJ0pqY3houGscHOJ7ABBWrj7h8UNdLA32AS6PuGEmwK8SipnSSsjZcue9rW27uNgvU4mxeTEKySaxJkeRGwbmxPlaFvDKbLNtI1tVVNBqHC7G9Igd/wCpBr7/AJU4n6/dSrJ39UQTdSCq4YpnRWmq8WEBsLdhEeTtve7LdPA/GEGIwCWIgPA/iRn2mH6fDzsUGTIiICIiDqYnXMgifNIdLI2lzj2Cp5xLjD6qplqJDd0jib+g2b+wC39n7jD4aBsTDtO8sf8AlAuq2lAuoREE3Xp8O4LLVzsp4Rdzza/Ro6uPoF5rQrFZFcIinp/xkrB4s3sHfUyPqD2uQgyDhLLSho4tLomTSEDW94Drm2+kHkFrrNrK3wg6soWHw93SxDmz52j4fRb6socL/rzQUhLVBC3Pm7lf4ZfW0TLsJ1Swt5svzewdRzJWmSEHFERAREQFN1CIPtS1Lo3texxa9pBa4bEEdVYjLrjmDFYDRVrWmbTpINtMwAtqHZwVcV96WpfG9r43FrmkFrhzBCDOsysuZcPk1xNdJTPPkcASWXv5HdTYDmsIgo5HuDWRuc4kAANdzPJWayo4xOJUpE7R4sRDH9Q8ADzWPU3WYUmEwRkujhY0nckAINc5T5ZNpAKqqaDUOF2tPKIbH+r1W1LIFKCLIpRBorOHLMgurqNnl5yxAcuQ1NHb0WquGuIJ6Gds9O4hzeYN9LxuC1w+6uQQtF5uZX6dVbRM8vOWIDl8zR90G0OBuMYMSgEsR0vG0kZ9pjv8j1WTKmvDHEM9DO2ogdZw5j3Xt6td6K0vA3GEGIwCSI2e3aSM+0x307HogyVQVKgoNc54YA+pw/WznTu8SwBJcORAAVZSFd8tBFjvdaSzEycc576jD7AG7nQ8t/8AxoNGWU2Xvy8GYg02NFMP/Ur3OGcrMRqn2dEYGCxLpARt8o6lB52W/Crq+sZHY+E06pXDk0Dcfcq2NPE1jQ1osALADkAF4fBvCMGHQ+FCLk7vebann1WQoCWQKUHBzQdiOex9R2K0Pm7lho111EzybmaID2e72Dt6LfJCOF0FICFBC3Rm3lfo1VtEzyc5YQPZ7vYO3MlaYKDiiIgKbKFN0Cy7uE4ZLUSthhaXvcbAD+5TCsNkqJWwwtLnvNgB69SrN5a8ARYdEHuAdUPHnf8ADf3Qg7GWXBgw2m0OOqWQh8h7Gw8o9FmSgBSgIiICIiAoc2+ylCg0Rm7lhpLq2iZ5ecsQ935mjt6LVvDXEFRQztngcQ4cx7rxyLXD9Vcdw6f/ABV3zx4PpqWRlRA5rDMTqh9Ra7mj9UGzcLzUw+SkFVLJ4ZvpfHYuc1/YAbkeq7GEZoYXUSiKOoOp3LW1zB9yqp3U3QXea4HcG4PI91NlqrInip1TTuppXF0kFtJP+2dhdbWCCLJZSiAiIgIiICIhQcHi45XvtbuFonN3LDRrraJnlPmmhG+nu9g7ei3vZC3ZBSEtUBbozdyv8PVW0TPJ7U0LR7Pd7AOndaXKAu5hWGS1ErYYWFz3GwA/uUwnDZaiVsMLC+RxsAP7/RWby14Aiw6IOcA6oeBrd8PytQMtcv4sOj1Os+oePO/4b28reyzpcQFyQEREBERAREQFBUqCgxjjzjGHDacyPOqR20cfV7v8D1VWeI8enrZ3T1Dy57uXZo6NaOi2N/qJJ/Gwg8vBOn7rUqCFyCiyINj5W8X0mGMllkDnTSWaGgbaBvzW0+GM4KGpe2KS8L3Gzbjyfq7oqzXUgoLvMcDuN/XofouS19kljclThzfFN3RPMYPXSBtdbAuglERAREQEUXS6CF1f/wBOHxPC8aPXe2jU3XftpupxIPMUgj9vQ7R+ax0/uqfYjUVMVU98rntqWyEudc6g8HndBcksBFrbfe/1Wic3ssNBfW0TCWG7pogPZvuXsA6b8llmVWZTK1op6hwbUtGx5CUDqPVbLcLgi1/Tugpvwvj8tDUMqYbam7EHk5twXN9L2VpuCeLoMRgEsJs4W8SPbUx3X9PVanzfyy8LXXUbf4fOaIe4eZe0dG91gGX3EclDWRSsLtLnBkjQbB7XG249EFurKVwjdcA9xf7rmgIiICIiAiIgKCpRBg2aHAbcShBZZtRGD4buhHVjuwVdsQ4Lr4ZHRupJiWm12Me5p+hA3VwbKHIKW4phUtO4MmbpcRq0n2h6OHQrolZnm1TSMxSpMgI1v1MJ6tsALLDEELkFFkJQbG4MzOdhtN+HhgbJdxe57iQbnpYdlsPgvOeGpkENVGIHOIDHNJLCT8RPJV2U3tyQXfYQRt9+65LEcrcWfUYbBI8HUBovvuG7XWWkoJRRdLoBXhYhxdRQSiGWoja87Eah5fzb7L3Sqn5p4PNT4jOZmm0ry9jjyc09boLWxvDgCCCCLgjkQey19mjlyzEGGaEBlUwbHYCUfC4/5WAZSZnGnLaOscTETaOQm5jJ5AnsrAMkDhcEEEXBG4IQUukjmpprHVFLE71a5jgrC5XZmx1cYgq3tZUMA8ziA2UCwvc+96Ls5r5eR10bqiIBlTG0nVbaRoF9LrcztsVWl7XMdY3a5p/VpBQWazU4zpoKKaISMkkmY6IMa5pI1NI1GyrhgVM+SohYxpc4yMsB6OF18YoJZXeVr3ucbciSSdua3rlBlo+meK2sGmQfyo/gv7zvVBt+BtgB2A/svooClAREQEREBERAREQFBUogwTMzL6PE2B7ToqGAhjujh8LvRaadk7iuvR4cX5tZ0/fSrQLhI24I7iyCl2MYe6nmfA4gujcWutu3UOdj1C6C9/jmidFX1MbgbiV3O9yOh9V4RCCAuQXEqEGdMzRro4ooadzYWRNDbNAOu3V1+q2Nl/nEaiVlNWMDHPs1kjb2Lj8V+S0AFzY8jcEgg3BGxB9EF3brD+LsxaLD5GxTue55F7RgOsPm32XvcNyF1JTucbkwxknqTpG60XnlwfLFUOr2Bz4pfbPPw3drdAg3rgWNwVcTZoHh7COlrj0cOhXU4v4WgxCB0M7fVj7DVG62zmlVm4B40mw2cPaS6J38yO+xHcDurR8PY5DWQtngcHNcOV92nseyCqHF/C1Rh85hnbtfyPF9MjehBtz7rPcqM0TTWpKxxMPKOQ7mPs0/Ktz8X8LQYhAYZm+rHi2pjuhB7KpmOYaaaolgcQ4xSOYSORseaCx3G+ZtFBTv8GZs0kjSGBhDgCQbF3ZVria+aUDdz5Hi9tyS478l9sMweeoc1sML3lxsLNdb+rkt4ZV5VupnirrbeI3+XFz0H4nHqUGw+FOHYaWnijZGwOaxuo2FySN9/qvdQrkgiylEQEREBERAREQEREBERAUEKUQYRmBl1T4kA8kxztFhI0C7h0a/uFqiTJOsjDnyzQiJgLnFpdq0tF9gRa6scujjFJ4sMsQ5vjcwfVwIQUwqQ3UdBJbfyk7EjuV8V38ZwySmmfBKCHxuLTsRe3Uei6dkHEL6Qu0uDrA2INjyNuhWxcq8uH17xPO0tpmm/UGUjoPRZhmrlW1zDVUEelzB/EiHJwHvN9QEGX5ZccwV8DWCzJo2hro9uQFgWjtssyrKVkrHRyNDmPGlzTuCD0sqaYXiM1LM2WJzmSMPqDtzBCs5lxx9FiMVjZlQ0edlxv8AM3ug05mnlw+geZ4AXUrze+5MRPR23s9l4XAXGs2GzB7CXROP8SO+xHceqtfWUzJWOjkaHNcLOaQCCCqpZmcOx0NdJDC67D5mjqwH3Sg23ieeNEISYY5TKWnS1wADXfMb91oSuq31Ez5XbvleXH1c49F6XDXCNZXavwsWsMsHG9gCfVbly3yiFM9tTWkPlbYsi5tY7ufiQZtlzgrqXD6eF4brDbusOrt+f6rJkCkIIsuSIgIiICIiAiIgIiICIiAiIgIiICiylEGH8a5d0eI2dKCyQf8AcZpDiOzrjcLFcPyKo2Pa6SeV7QQdB0WdY8nbcltpEHwpKVkTGxxtDWtFmtaLAAdAvtZSiDTWbmV/i6qyiYBIN5Ym7a/maAPaWkcKxKalmbNE4skYfUHbmCFdFwWsePco4Kx5np3CGV1y4AeR5+nRBjrc9x+H/wCmJqbWvt4V+/O60/jGJy1U755TqfI6557X5ABZsMmcWvbw4rX5+IOXfktk8DZPwUj2z1LvGlbYtbbyMd9PeQe1lDw86kw9gkbpkk87x139n9lnFlAC5IIspREBERAREQEREBERAREQEREBERAREQEREBERAREQLJZEQRpSylEEWUoiAiIgIiICIiAiIgIiICIiAiIgIiICIiAiIgIiICIiAiIgIiICIiAiIgIiICIiAiIgIiICIiD/2Q==",
        200, true)]

        [DataRow("", "sadsad", 400, true)]
        [DataRow("title 123", "", 400, true)]
        [DataRow("title 123", "image", 401, false)]

        public async Task CreatePostTest(string title, string image, HttpStatusCode statusCode, bool authorized)
        {
            var parameters = new Dictionary<string, string> { { "title", title }, { "image", image } };

            var response = await LineUpWebRequest(parameters, "api/Post/Create", CommandType.Post, authorized);

            Assert.AreEqual(response.StatusCode, statusCode);
        }

        //Comment Post test cases

        [TestMethod]
        [DataRow("64241d8fb1324495588f303c", "Comment test MS unit test...123 comment.",200,true)]
        [DataRow("64241d8fb1324495588f303c", "Comment test MS unit test...123 comment.", 401, false)]
        [DataRow("", "Comment test MS unit test...123 comment.", 400, true)]
        [DataRow("64241d8fb1324495588f303c", "", 400, true)]

        public async Task CommentPostTest(string postId, string comment, HttpStatusCode statusCode, bool authorized)
        {
            var parameters = new Dictionary<string, string> { { "postId", postId }, { "comment", comment } };

            var response = await LineUpWebRequest(parameters, "api/Post/Comment", CommandType.Post, authorized);

            Assert.AreEqual(response.StatusCode, statusCode);

        }


        //Like Post test cases

        [TestMethod]
        [DataRow("64241d8fb1324495588f303c", 200, true)]
        [DataRow("64241d8fb1324495588f303c", 401, false)]
        [DataRow("64241d8fb1324495588f304c", 404, true)]
        [DataRow("", 400, true)]


        public async Task LikePostTest(string postId, HttpStatusCode statusCode, bool authorized)
        {
            var parameters = new Dictionary<string, string> { { "postId", postId }};

            var response = await LineUpWebRequest(parameters, "api/Post/Like", CommandType.Post, authorized);

            Assert.AreEqual(response.StatusCode, statusCode);
        }


        #region AR testing Helper Functions
        public enum CommandType
        {
            Post,
            Get,
            Delete,
            Put
        }
        public async Task<HttpResponseMessage> LineUpWebRequest(object parameters, string endPoint, CommandType commandType, bool authorized)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(50);//timeout
                string json = JsonConvert.SerializeObject(parameters);
                StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                if(authorized==true)
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }
                else
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + "");
                }
                switch (commandType)
                {
                   
                    case CommandType.Post:
                        response = await client.PostAsync(BaseAddress + endPoint, httpContent).ConfigureAwait(true);
                        break;
                    case CommandType.Get:
                        response = await client.GetAsync(BaseAddress+ endPoint).ConfigureAwait(true);
                        break;
                    case CommandType.Put:
                        response = await client.PutAsync(BaseAddress + endPoint, httpContent).ConfigureAwait(true);
                        break;
                    case CommandType.Delete:
                        response = await client.DeleteAsync(BaseAddress + endPoint).ConfigureAwait(true);
                        break;
                }
                return response;
            }
            catch
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
        }

        #endregion

    }
}
