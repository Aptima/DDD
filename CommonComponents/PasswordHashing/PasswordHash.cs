using System;
using System.Collections.Generic;
using System.Text;

namespace Aptima.Asim.DDD.CommonComponents.PasswordHashing
{
    public static class PasswordHashUtility
    {
        /*
         * This is a VERY simple hash to not send plain-text passwords over the network, and to 
         * comply with ITAR and EAR restrictions.  The bit strength must be less than 64-bit, 
         * and this should be WELL under that.
         */

        private const int _hashOffset = 3;


        /// <summary>
        /// Users Hash a password before sending from the client or server.
        /// </summary>
        /// <param name="unhashedPassword"></param>
        /// <returns></returns>
        public static string HashPassword(string unhashedPassword)
        {
            string hashedPassword = string.Empty;
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(unhashedPassword);

            for (int x = 0; x < bytes.Length; x++)
            {
                try
                {
                    bytes[x] += _hashOffset;
                }
                catch(Exception)
                {
                    Console.WriteLine("Error offsetting password hash");
                }
            }

            hashedPassword = System.Text.Encoding.UTF8.GetString(bytes);
            return hashedPassword;
        }

        /// <summary>
        /// The recipient unhashes the password after receiving a password from a distributed client.
        /// </summary>
        /// <param name="hashedPassword"></param>
        /// <returns></returns>
        public static string UnHashPassword(string hashedPassword)
        {
            string unhashedPassword = string.Empty;

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(hashedPassword);

            for (int x = 0; x < bytes.Length; x++)
            {
                try
                {
                    bytes[x] += _hashOffset;
                }
                catch (Exception)
                {
                    Console.WriteLine("Error offsetting hashed password");
                }
            }

            unhashedPassword = System.Text.Encoding.UTF8.GetString(bytes);

            return unhashedPassword;
        }
    }
}
