using System;
using System.Configuration;
using System.Data.SqlClient;


namespace SimpleForm
{
	class Program
	{
		static string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
		static void Main(string[] args)
		{
			Console.WriteLine("Welcome To Our Simple Form \n");
			Console.WriteLine("for Sign Up press 0 & Sign In 1");

			string signOperation = Console.ReadLine();

			while (signOperation != "0" && signOperation != "1")
			{
				Console.WriteLine("Please Enter 0 or 1");
				signOperation = Console.ReadLine();
			}

			int operation = Convert.ToInt32(signOperation);

			if (operation == (int)Operation.signUp)
			{

				Console.WriteLine("Please enter your username");

				string userName;
				int userFound;
				do
				{
					userName = Console.ReadLine();
					Console.WriteLine("Checking if username available ...");
					using (SqlConnection sqlConnection = new SqlConnection(CS))
					{
						SqlCommand cmd = new SqlCommand("SELECT * FROM college.dbo.student WHERE username = @username", sqlConnection);
						cmd.Parameters.AddWithValue("@username", userName);
						sqlConnection.Open();
						userFound = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
					}

					if (userFound > 0)
						Console.WriteLine("Sorry username already exists , please try again");

				} while (userFound > 0);

				Console.WriteLine("username is available");
				Console.WriteLine("Please enter a strong password that is not less than 8 characters");

				string password = "";
				ConsoleKeyInfo key;

				do
				{
					key = Console.ReadKey(true);
					if (key.Key != ConsoleKey.Enter)
					{
						password += key.KeyChar;
						Console.Write("*");
					}

				} while (key.Key != ConsoleKey.Enter || password.Length < 8);
				Console.WriteLine();

				Console.WriteLine("Please select your gender");
				Console.WriteLine("for male select m for female select f");

				string gender = Console.ReadLine();
				gender = gender.ToUpper();

				while(gender != "M" && gender != "F")
				{
					Console.WriteLine("Please Enter m or f");
					gender = Console.ReadLine();
					gender = gender.ToUpper();
				}

				using (SqlConnection sqlConnection = new SqlConnection(CS))
				{
					SqlCommand cmd = new SqlCommand("INSERT INTO college.dbo.student VALUES (@username,@password,@gender)",sqlConnection);
					cmd.Parameters.AddWithValue("@username",userName);
					cmd.Parameters.AddWithValue("@password",password);
					cmd.Parameters.AddWithValue("@gender",gender);

					sqlConnection.Open();
					cmd.ExecuteNonQuery();
				}
				Console.WriteLine("Signed up successfully");
				
			}
			else if (operation == (int)Operation.signIn)
			{ 
				int userFound;
				do {
					Console.WriteLine("Please enter your username");
					string userName = Console.ReadLine();
					Console.WriteLine("Please enter your password");

					string password = "";
					ConsoleKeyInfo key;

					do
					{
						key = Console.ReadKey(true);
						if (key.Key != ConsoleKey.Enter)
						{
							password += key.KeyChar;
							Console.Write("*");
						}

					} while (key.Key != ConsoleKey.Enter || password.Length < 8);
					Console.WriteLine();

					using (SqlConnection sqlConnection = new SqlConnection(CS))
					{
						SqlCommand cmd = new SqlCommand("SELECT * FROM college.dbo.student WHERE username = @username AND password = @password", sqlConnection);
						cmd.Parameters.AddWithValue("@username", userName);
						cmd.Parameters.AddWithValue("@password", password);

						sqlConnection.Open();
						userFound = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
					}
					if(userFound == 0)
						Console.WriteLine("Invalid username or password");


				} while (userFound == 0);

				Console.WriteLine("Signed In successfully");
			}
			else
			{
				Console.WriteLine("Error ocurred , Please try again later");
			}
		}

		enum Operation
		{
			signUp,
			signIn
		}
	}
}
