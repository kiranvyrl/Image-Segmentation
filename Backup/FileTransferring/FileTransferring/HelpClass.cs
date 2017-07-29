using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Net.Sockets;
using System.Net;

namespace FileTransferring
{
    public static class HelpClass
    {

        public static Form1 Form { get; set; }
        public static List<Match> IndexOfAny(this string input, string[] param)
        {
            List<Match> output = new List<Match>();
            foreach (string i in param)
            {
                string pattern = "(" + Regex.Escape(i) + ")";
                MatchCollection match = Regex.Matches(input, pattern);
                foreach (Match ix in match)
                {
                    input = input.Replace(ix.Value, "");
                    output.Add(ix);
                }
            }
            output.Sort(new Comparison<Match>(CompareMatches));
            return output;
        }

        internal static byte[] ToArray(this byte[] bytes, int offset, int count)
        {
            byte[] result = new byte[count];
            for (int i = 0; i < count; i++)
                result[i] = bytes[offset + i];
            return result;
        }

        internal static byte[] Without(this byte[] bytes, int offset)
        {
            byte[] result = new byte[bytes.Length - offset];
            for (int i = offset; i < bytes.Length; i++)
                result[i - offset] = bytes[i];
            return result;
        }

        internal static byte[] Without(this byte[] bytes, long offset)
        {
            byte[] result = new byte[bytes.Length - offset];
            for (long i = offset; i < bytes.Length; i++)
                result[i - offset] = bytes[i];
            return result;
        }

        internal static byte[] Without(this byte[] bytes, long offset, long count)
        {
            byte[] result = new byte[count];
            for (long i = offset; i < count; i++)
                result[i - offset] = bytes[i];
            return result;
        }

        static int CompareMatches(Match x, Match y)
        {
            if (x.Index < y.Index)
                return -1;
            else
                return 1;
        }

        public static Binding ConfigureEndpoint()
        {
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None, false);
            binding.MaxBufferSize = (int)67108864;
            binding.CloseTimeout = new TimeSpan(0, 1, 0);
            binding.OpenTimeout = new TimeSpan(0, 0, 2);
            binding.ReceiveTimeout = new TimeSpan(0, 1, 0);
            binding.SendTimeout = new TimeSpan(0, 1, 0);
            binding.TransactionFlow = false;
            binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            int count = 67108864;
            binding.MaxBufferPoolSize = count;
            binding.MaxReceivedMessageSize = count;
            binding.ReaderQuotas.MaxDepth = 64;
            binding.ReaderQuotas.MaxStringContentLength = count;
            binding.ReaderQuotas.MaxArrayLength = count;
            binding.ReaderQuotas.MaxBytesPerRead = count;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
            binding.ReliableSession.Ordered = false;
            return binding;
        }

        public static int GetAvailablePort()
        {
            TcpListener list = new TcpListener(0);
            list.Start();
            int port = (list.LocalEndpoint as IPEndPoint).Port;
            list.Stop();
            return port;
        }
    }
}
