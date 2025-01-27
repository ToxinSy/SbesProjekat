﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Common
{
    public class SecretKey
    {
		public static string GenerateKey()
		{
			SymmetricAlgorithm symmAlgorithm;

			symmAlgorithm = DESCryptoServiceProvider.Create();

			return symmAlgorithm == null ? String.Empty : Encoding.ASCII.GetString(symmAlgorithm.Key);
		}

		public static void StoreKey(string secretKey, string outFile)
		{
			FileStream fOutput = new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.Write);
			byte[] buffer = Encoding.ASCII.GetBytes(secretKey);

			try
			{
				fOutput.Write(buffer, 0, buffer.Length);
			}
			catch (Exception e)
			{
				Console.WriteLine("SecretKeys.StoreKey:: ERROR {0}", e.Message);
			}
			finally
			{
				fOutput.Close();
			}
		}

		public static string LoadKey(string inFile)
		{
			FileStream fInput = new FileStream(inFile, FileMode.Open, FileAccess.Read);
			byte[] buffer = new byte[(int)fInput.Length];

			try
			{
				fInput.Read(buffer, 0, (int)fInput.Length);
			}
			catch (Exception e)
			{
				Console.WriteLine("SecretKeys.LoadKey:: ERROR {0}", e.Message);
			}
			finally
			{
				fInput.Close();
			}

			return Encoding.ASCII.GetString(buffer);
		}
	}
}
